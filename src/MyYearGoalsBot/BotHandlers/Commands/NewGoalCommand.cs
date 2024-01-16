using BotFramework.Attributes;
using BotFramework.Base;
using MyYearGoalsBot.BotHandlers.States.NewGoalState;
using Telegram.Bot.Types;

namespace MyYearGoalsBot.BotHandlers.Commands;

[BotCommand("/new_goal", version: 1.0f)]
public class NewGoalCommand : BaseMyYearGoalsBotCommand
{
    public NewGoalCommand(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        
    }

    public override async Task HandleBotRequest(Update update)
    {
        Chat.States.Set(NewGoalState.STATE);
        await BotDbContext.SaveChangesAsync();
        await Answer(R.AddGoal.WriteGoalToBot, replyMarkup:R.GetOneTimeCancelKeyboard());
    }
}