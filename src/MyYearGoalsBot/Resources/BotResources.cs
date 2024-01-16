using BotFramework.Other;
using MyYearGoalsBot.BotHandlers.States;
using MyYearGoalsBot.BotHandlers.States.TestBot;
using Telegram.Bot.Types.ReplyMarkups;

namespace MyYearGoalsBot.Resources;

public partial class BotResources
{
    public string BotIntroduction { get; set; }
    public string NotExpectedUpdate { get; set; }
    public string Cancel { get; set; }
    public string ActionCancelled { get; set; }
    public string SupportMessage { get; set; }
    public string HelpMeMessage { get; set; }
    public string DefaultMessage { get; set; }
    public AddGoalRes AddGoal { get; set; }
    public GoalsRes Goals { get; set; }
    public DeleteGoalRes DeleteGoal { get; set; }


    public ReplyKeyboardMarkup GetOneTimeCancelKeyboard()
    {
        var kb = new MarkupBuilder<ReplyKeyboardMarkup>();
        ReplyKeyboardMarkup keyboard = kb
            .NewRow()
            .Add(Cancel)
            .Build() as ReplyKeyboardMarkup;
        keyboard.OneTimeKeyboard = true;
        return keyboard;
    } 
}