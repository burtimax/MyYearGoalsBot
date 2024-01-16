using BotFramework;
using BotFramework.Attributes;
using BotFramework.Db;
using MyYearGoalsBot.BotHandlers.Commands;
using MyYearGoalsBot.Db;
using MyYearGoalsBot.Helpers;
using MyYearGoalsBot.Services;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace MyYearGoalsBot.BotHandlers.States.NewGoalState;

[BotState(STATE, version:1.0f)]
public class NewGoalState : BaseMyYearGoalsBotState
{
    public const string STATE = "NewGoalState";
    
    public NewGoalState(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        Expected(Telegram.Bot.Types.Enums.UpdateType.Message);
        ExpectedMessage(MessageType.Text);
        NotExpectedMessage = R.AddGoal.WriteGoalToBot;
    }

    public override async Task HandleMessage(Message message)
    {
        if (string.Equals(message.Text, R.Cancel))
        {
            Chat.States.Set(BotConstants.StartState);
            await BotDbContext.SaveChangesAsync();
            await SendGoals(new ReplyKeyboardRemove());
            return;
        }
        
        await GoalsService.AddGoalToUser(User.TelegramId, message.Text!);
        Chat.States.Set(BotConstants.StartState);
        await BotDbContext.SaveChangesAsync();
        await SendGoals(new ReplyKeyboardRemove());
    }
}