using BotFramework.Db.Entity;

namespace MyYearGoalsBot.Db;

/// <summary>
/// Просмотр целей.
/// </summary>
public class GoalsReview : BaseBotEntity<long>
{
    /// <summary>
    /// TelegramId пользователя.
    /// </summary>
    public long UserId { get; set; }
}