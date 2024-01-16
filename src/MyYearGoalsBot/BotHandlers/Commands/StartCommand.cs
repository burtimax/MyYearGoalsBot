using BotFramework.Attributes;
using BotFramework.Base;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace MyYearGoalsBot.BotHandlers.Commands;

[BotCommand("/start", version: 2.0f)]
public class StartCommand : BaseMyYearGoalsBotCommand
{
    public StartCommand(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }

    public override async Task HandleBotRequest(Update update)
    {
        await Answer(R.BotIntroduction);
        await Answer(R.DefaultMessage, parseMode:ParseMode.Html);
    }
}