using BotFramework.Attributes;
using BotFramework.Base;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace MyYearGoalsBot.BotHandlers.UpdateType;

[BotPriorityHandler(Telegram.Bot.Types.Enums.UpdateType.MyChatMember)]
public class MyChatMemberTypeHandler : BaseBotPriorityHandler
{
    public MyChatMemberTypeHandler(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }


    public override async Task HandleBotRequest(Update update)
    {
        ChatMemberUpdated input = update.MyChatMember!;

        if (input.NewChatMember.Status == ChatMemberStatus.Kicked ||
            input.NewChatMember.Status == ChatMemberStatus.Left)
        {
           // ToDo сделать обработку, когда пользователь блокирует бота.
        }
        
        if (input.NewChatMember.Status == ChatMemberStatus.Member)
        {
            // ToDo сделать обработку, когда пользователь приходит в бота.
        }
    }
}