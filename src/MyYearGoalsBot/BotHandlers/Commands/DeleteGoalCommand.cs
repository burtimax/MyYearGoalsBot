using BotFramework.Attributes;
using BotFramework.Base;
using MyYearGoalsBot.BotHandlers.States.DeleteGoalState;
using MyYearGoalsBot.BotHandlers.States.NewGoalState;
using MyYearGoalsBot.Db;
using MyYearGoalsBot.Helpers;
using MyYearGoalsBot.Services;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace MyYearGoalsBot.BotHandlers.Commands;

[BotCommand("/delete_goal", version: 1.0f)]
public class DeleteGoalCommand : BaseMyYearGoalsBotCommand
{
    private readonly UserGoalsService _service;
    
    public DeleteGoalCommand(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        _service = serviceProvider.GetRequiredService<UserGoalsService>();
    }

    public override async Task HandleBotRequest(Update update)
    {
        Chat.States.Set(DeleteGoalState.STATE);
        await BotDbContext.SaveChangesAsync();
        await SendGoals(new ReplyKeyboardRemove());
        
        int goalsCount = await GoalsService.GetUserGoalsCount(User.TelegramId);
        if (goalsCount > 0)
        {
            await BotClient.SendTextMessageAsync(Chat.ChatId, R.DeleteGoal.InputGoalOrderNumber, replyMarkup:R.GetOneTimeCancelKeyboard());
        }
    }
}