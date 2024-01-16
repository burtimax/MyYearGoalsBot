using BotFramework.Attributes;
using BotFramework.Base;
using MyYearGoalsBot.BotHandlers.States.NewGoalState;
using MyYearGoalsBot.Db;
using MyYearGoalsBot.Helpers;
using MyYearGoalsBot.Services;
using Telegram.Bot.Types;

namespace MyYearGoalsBot.BotHandlers.Commands;

[BotCommand("/helpme", version: 1.0f)]
public class HelpMeCommand : BaseMyYearGoalsBotCommand
{
    private readonly UserGoalsService _service;
    
    public HelpMeCommand(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        _service = serviceProvider.GetRequiredService<UserGoalsService>();
    }

    public override async Task HandleBotRequest(Update update)
    {
        await Answer(R.HelpMeMessage);
    }
}