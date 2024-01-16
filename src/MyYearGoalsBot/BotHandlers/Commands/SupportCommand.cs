using BotFramework.Attributes;
using BotFramework.Base;
using MyYearGoalsBot.BotHandlers.States.NewGoalState;
using MyYearGoalsBot.Db;
using MyYearGoalsBot.Helpers;
using MyYearGoalsBot.Services;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace MyYearGoalsBot.BotHandlers.Commands;

[BotCommand("/support", version: 1.0f)]
public class SupportCommand : BaseMyYearGoalsBotCommand
{
    private readonly UserGoalsService _service;
    
    public SupportCommand(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        _service = serviceProvider.GetRequiredService<UserGoalsService>();
    }

    public override async Task HandleBotRequest(Update update)
    {
        await Answer(R.SupportMessage, parseMode:ParseMode.Html);
    }
}