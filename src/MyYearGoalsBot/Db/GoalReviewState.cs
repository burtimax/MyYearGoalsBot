namespace MyYearGoalsBot.Db;

/// <summary>
/// Статус просмотра целей.
/// </summary>
public enum GoalReviewState
{
    /// <summary>
    /// Не просмотрено.
    /// </summary>
    Unread = 1,
    
    /// <summary>
    /// Просмотрено.
    /// </summary>
    Read = 2,
}