using BotFramework.Db.Entity;

namespace MyYearGoalsBot.Db;

public class Goal : BaseBotEntity<long>
{
    /// <summary>
    /// Пользователь, чья цель.
    /// </summary>
    public long UserId { get; set; }
    
    /// <summary>
    /// Описание цели.
    /// </summary>
    public string Description { get; set; }
    
    /// <summary>
    /// Приоритет цели.
    /// </summary>
    public int Priority { get; set; }

    /// <summary>
    /// Состояние цели (Статус). 
    /// </summary>
    public GoalState State { get; set; }
}