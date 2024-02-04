using System.Diagnostics;
using BotFramework.Db;
using BotFramework.Db.Entity;
using BotFramework.Extensions;
using BotFramework.Options;
using BotFramework.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MyYearGoalsBot.Db;
using MyYearGoalsBot.Db.Context;
using MyYearGoalsBot.Helpers;
using MyYearGoalsBot.Resources;
using MyYearGoalsBot.Services;
using Quartz;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;

namespace MyYearGoalsBot.Jobs;

public class NotifyGoalsJob : IJob
{
    public const string Key = "notify_goals_users";
    public const string TriggerKey = "trigger_notify_goals_users";

    private ITelegramBotClient _botClient;
    private readonly BotConfiguration _botConfig;
    private UserGoalsService _goalsService;
    private readonly BotResources R;

    public NotifyGoalsJob(ITelegramBotClient botClient, IOptions<BotConfiguration> botConfig, IOptions<BotResources> r)
    {
        _botClient = botClient;
        _botConfig = botConfig.Value;
        R = r.Value;
    }
    
    public async Task Execute(IJobExecutionContext context)
    {
        await NotifyAllUsers();
    }

    public async Task NotifyAllUsers()
    {
        // Здесь необязательно await ожидать окончания задачи.
        await Task.Factory.StartNew(async () =>
        {
            // Создаем DbContext чтобы задача могла в фоне выполняться.
            var ob = new DbContextOptionsBuilder<BotDbContext>();
            ob.UseNpgsql(_botConfig.DbConnection);
            var appOb = new DbContextOptionsBuilder<AppDbContext>();
            appOb.UseNpgsql(_botConfig.DbConnection);
            
            using(AppDbContext appDb = new(appOb.Options))
            {
                _goalsService = new(appDb);
                
                using (BotDbContext db = new BotDbContext(ob.Options))
                {
                    await BotHelper.ExecuteForAllUsers(db, async tuple =>
                    {
                        try
                        {
                            await NotifyUserAboutGoals(db, tuple.user, tuple.chat);
                            await Task.Delay(100); // Чтобы не выйти за лимиты бота и его не заблокировали.
                        }
                        catch (ApiRequestException e) when (e.ErrorCode == 403)
                        {
                            Debug.WriteLine(e.Message, "ERROR");
                        }
                    });
                }
            }
            
        }, TaskCreationOptions.LongRunning);
    }

    /// <summary>
    /// Уведомляем пользователя о его целях.
    /// </summary>
    private async Task NotifyUserAboutGoals(BotDbContext db, BotUser user, BotChat chat)
    {
        try
        {
            var chatInfo = await _botClient.GetChatAsync(chat.ChatId);
            if (chat == null) return;

            IEnumerable<Goal>? goals = await _goalsService.GetUserGoals(user.TelegramId);

            if (goals is not null && goals.Any())
            {
                await MyYearGoalsBotHelper.SendGoalsToUser(_botClient, user, db, R, chat.ChatId, goals, null);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Not found or banned chat [{chat.ChatId}]");
        }
    }
}