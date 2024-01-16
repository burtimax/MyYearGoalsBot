using BotFramework.Attributes;
using Microsoft.AspNetCore.Mvc;
using MyYearGoalsBot.Db;
using MyYearGoalsBot.Helpers;
using MyYearGoalsBot.Services;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace MyYearGoalsBot.BotHandlers.States.TestBot;

[BotState("StartState", version: 2)]
public class StartState : BaseMyYearGoalsBotState
{
    private readonly UserGoalsService _service;
    
    public StartState(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        _service = serviceProvider.GetRequiredService<UserGoalsService>();
    }

    public override async Task HandleBotRequest(Update update)
    {
        try
        {
            await Answer(R.DefaultMessage, parseMode:ParseMode.Html);
        }
        catch (Exception e)
        {
            // ignore
        }
    }
}