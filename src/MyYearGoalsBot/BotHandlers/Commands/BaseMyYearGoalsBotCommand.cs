using BotFramework.Base;
using BotFramework.Enums;
using Microsoft.Extensions.Options;
using MyYearGoalsBot.Db;
using MyYearGoalsBot.Db.Context;
using MyYearGoalsBot.Helpers;
using MyYearGoalsBot.Resources;
using MyYearGoalsBot.Services;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace MyYearGoalsBot.BotHandlers.Commands;

public class BaseMyYearGoalsBotCommand : BaseBotCommand
{
    protected IServiceProvider ServiceProvider;
    protected readonly BotResources R;
    protected readonly AppDbContext Db;
    protected readonly UserGoalsService GoalsService;

    public BaseMyYearGoalsBotCommand(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        this.ServiceProvider = serviceProvider;
        R = serviceProvider.GetRequiredService<IOptions<BotResources>>().Value;
        Db = serviceProvider.GetRequiredService<AppDbContext>();
        GoalsService = serviceProvider.GetRequiredService<UserGoalsService>();
    }
    

    protected Task Answer(string text, ParseMode parseMode = ParseMode.Html, IReplyMarkup replyMarkup = default)
    {
        return BotClient.SendTextMessageAsync(Chat.ChatId, text, parseMode, replyMarkup: replyMarkup);
    }
    
    public async Task ChangeState(string stateName, ChatStateSetterType setterType = ChatStateSetterType.ChangeCurrent)
    {
        Chat.States.Set(stateName, setterType);
        await BotDbContext.SaveChangesAsync();
    }

    public override Task HandleBotRequest(Update update)
    {
        throw new NotImplementedException();
    }
    
    /// <summary>
    /// Отправить список целей пользователю.
    /// </summary>
    /// <returns></returns>
    protected async Task SendGoals(IReplyMarkup replyMarkup = null)
    {
        IEnumerable<Goal>? goals = await GoalsService.GetUserGoals(User.TelegramId);
        await MyYearGoalsBotHelper.SendGoalsToUser(BotClient, User, BotDbContext, R, Chat.ChatId, goals, replyMarkup);
    }
}