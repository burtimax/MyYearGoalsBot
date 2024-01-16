using BotFramework;
using BotFramework.Attributes;
using MyYearGoalsBot.Helpers;
using MyYearGoalsBot.Services;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace MyYearGoalsBot.BotHandlers.States.DeleteGoalState;

[BotState(STATE, version:1.0f)]
public class DeleteGoalState : BaseMyYearGoalsBotState
{
    public const string STATE = "DeleteGoalState";
    
    public DeleteGoalState(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        Expected(Telegram.Bot.Types.Enums.UpdateType.Message);
        ExpectedMessage(MessageType.Text);
        NotExpectedMessage = R.DeleteGoal.InputNumberOrCancel;
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
        
        int? number = ParseIntFromMessage(message.Text!);

        if (number == null)
        {
            await Answer(R.DeleteGoal.InputNumberOrCancel);
            return;
        }
        
        await GoalsService.DeleteGoalFromUser(User.TelegramId, number.Value);
        Chat.States.Set(BotConstants.StartState);
        await BotDbContext.SaveChangesAsync();
        await SendGoals(new ReplyKeyboardRemove());
    }

    private int? ParseIntFromMessage(string text)
    {
        text = text.Trim(' ', ',', '.');
        if (int.TryParse(text, out int number))
        {
            return number;
        }

        return null;
    }
}