using BotFramework.Base;
using Microsoft.Extensions.Options;
using MyYearGoalsBot.Resources;
using Telegram.Bot.Types;

namespace MyYearGoalsBot.BotHandlers.States;

public class BotState : BaseBotState
{
    protected BotResources R;
    
    public BotState(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        R = serviceProvider.GetRequiredService<IOptions<BotResources>>().Value;
    }

    public override Task HandleBotRequest(Update update)
    {
        return null;
    }
}