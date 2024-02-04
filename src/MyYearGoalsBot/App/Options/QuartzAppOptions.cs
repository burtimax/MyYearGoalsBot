namespace MyYearGoalsBot.App.Options;

/// <summary>
/// Конфигурации для Quartz заданий.
/// </summary>
public class QuartzAppOptions
{
    /// <summary>
    /// Крон строка для задания уведомлений о целях всех пользователей.
    /// </summary>
    public string GoalsNotificationJobCron { get; set; }
}