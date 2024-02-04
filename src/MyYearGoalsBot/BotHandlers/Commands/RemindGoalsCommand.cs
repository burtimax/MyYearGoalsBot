using BotFramework.Attributes;
using BotFramework.Base;
using BotFramework.Options;
using Microsoft.Extensions.Options;
using MyYearGoalsBot.BotHandlers.States.NewGoalState;
using MyYearGoalsBot.Db;
using MyYearGoalsBot.Helpers;
using MyYearGoalsBot.Jobs;
using MyYearGoalsBot.Resources;
using MyYearGoalsBot.Services;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace MyYearGoalsBot.BotHandlers.Commands;

/// <summary>
/// Команда принудительного оповещения о целях пользователей.
/// </summary>
[BotCommand("/remindall", version: 1.0f)]
public class RemindGoalsCommand : BaseMyYearGoalsBotCommand
{
    private readonly UserGoalsService _service;
    private NotifyGoalsJob notifyJob;
    
    public RemindGoalsCommand(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        _service = serviceProvider.GetRequiredService<UserGoalsService>();
    }

    public override async Task HandleBotRequest(Update update)
    {
        var botConfig = ServiceProvider.GetRequiredService<IOptions<BotConfiguration>>();
        var botRes = ServiceProvider.GetRequiredService<IOptions<BotResources>>();
        notifyJob = new(BotClient, botConfig, botRes);
        await notifyJob.NotifyAllUsers();
    }
}