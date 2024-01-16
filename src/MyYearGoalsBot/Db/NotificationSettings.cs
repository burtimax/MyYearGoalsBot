using BotFramework.Db.Entity;

namespace MyYearGoalsBot.Db;

/// <summary>
/// Настройки уведомлений для пользователя.
/// </summary>
public class NotificationSettings : BaseBotEntity<long>
{
    /// <summary>
    /// Показывать уведомления для просмотра целей.
    /// </summary>
    public bool ShowGoalsNotification { get; set; }
}