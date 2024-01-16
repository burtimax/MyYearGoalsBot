using System.Diagnostics;
using System.Text;
using BotFramework.Db;
using BotFramework.Db.Entity;
using MyYearGoalsBot.Db;
using MyYearGoalsBot.Resources;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace MyYearGoalsBot.Helpers;

public class MyYearGoalsBotHelper
{
    private const string Key = "LastGoalsMessage"; 
    
    public static async Task SendGoalsToUser(ITelegramBotClient botClient, BotUser user, BotDbContext db, BotResources r, ChatId chatId, IEnumerable<Goal>? goals, IReplyMarkup replyMarkup = null)
    {
        if (goals == null || goals.Any() == false)
        {
            await botClient.SendTextMessageAsync(chatId, r.Goals.NoGoals);
            return;
        }

        StringBuilder sb = new();

        int c = 1;
        
        foreach (Goal goal in goals)
        {
            sb.AppendLine($"{c}) {goal.Description}");
            c++;
        }

        if (user.AdditionalProperties.Contains(Key))
        {
            int messageId = user.AdditionalProperties.Get<int>(key: Key);
            try
            {
                await botClient.DeleteMessageAsync(chatId, messageId);
            }
            catch (Exception e)
            {
                // ignored
            }
            finally
            {
                user.AdditionalProperties.Remove(Key);
            }
        }
        
        var message = await botClient.SendTextMessageAsync(chatId, text:string.Format(r.Goals.GoalsTemplate, sb.ToString()), parseMode:ParseMode.Html, replyMarkup: replyMarkup);
        
        user.AdditionalProperties.Set(Key, message.MessageId);
        await db.SaveChangesAsync();
    }
}