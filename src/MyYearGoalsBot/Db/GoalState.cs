namespace MyYearGoalsBot.Db;

/// <summary>
/// Список состояний цели.
/// </summary>
public enum GoalState
{
    /// <summary>
    /// Цель поставлена.
    /// </summary>
    Set = 1,
    /// <summary>
    /// Цель выполнена успешно.
    /// </summary>
    Success = 2,
    /// <summary>
    /// Цель не выполнена.
    /// </summary>
    Fail = 3,
}