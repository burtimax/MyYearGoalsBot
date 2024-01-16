using BotFramework.Attributes;
using BotFramework.Base;
using MyYearGoalsBot.BotHandlers.States.NewGoalState;
using MyYearGoalsBot.Db;
using MyYearGoalsBot.Helpers;
using MyYearGoalsBot.Services;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace MyYearGoalsBot.BotHandlers.Commands;

[BotCommand("/goals", version: 1.0f)]
public class GoalsCommand : BaseMyYearGoalsBotCommand
{
    private readonly UserGoalsService _service;
    
    public GoalsCommand(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        _service = serviceProvider.GetRequiredService<UserGoalsService>();
    }

    public override async Task HandleBotRequest(Update update)
    {
        await SendGoals(new ReplyKeyboardRemove());
    }
}