using Microsoft.EntityFrameworkCore;
using MyYearGoalsBot.Db;
using MyYearGoalsBot.Db.Context;
using MyYearGoalsBot.Extensions;

namespace MyYearGoalsBot.Services;

/// <summary>
/// Сервис управления целями пользователя.
/// </summary>
public class UserGoalsService
{
    private readonly AppDbContext _db;

    public UserGoalsService(AppDbContext db)
    {
        _db = db;
    }

    /// <summary>
    /// Добавить новую цель пользователю.
    /// </summary>
    /// <param name="userId">TelegramId пользователя.</param>
    /// <param name="goalDescription">Описание цели.</param>
    /// <returns>Созданая сущность цели.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public async Task<Goal> AddGoalToUser(long userId, string goalDescription)
    {
        if (goalDescription == null) throw new ArgumentNullException(nameof(goalDescription));

        Goal goal = new Goal()
        {
            UserId = userId,
            State = GoalState.Set,
            Description = goalDescription,
        };

        _db.Goals.Add(goal);
        await _db.SaveChangesAsync();
        return goal;
    }
    
    /// <summary>
    /// Удалить цель из списка целей пользователя (на текущий год).
    /// </summary>
    /// <param name="userId">TelegramId пользователя.</param>
    /// <param name="goalOrderNum">Порядок цели в списке.</param>
    public async Task DeleteGoalFromUser(long userId, int goalOrderNum)
    {
        Goal? goal = await GetUserGoalByOrderNum(userId, goalOrderNum);

        if (goal is not null)
        {
            _db.Goals.Remove(goal);
            await _db.SaveChangesAsync();
        }
    }

    /// <summary>
    /// Редактировать цель пользователя в списке.
    /// </summary>
    /// <param name="userId">TelegramId пользователя.</param>
    /// <param name="goalOrderNum">Порядок цели в списке.</param>
    /// <param name="newDescription">Новое описание цели.</param>
    /// <returns>Сущность отредактированной цели.</returns>
    public async Task<Goal> EditGoalForUser(long userId, int goalOrderNum, string newDescription)
    {
        Goal? goal = await GetUserGoalByOrderNum(userId, goalOrderNum);

        if (goal == null) throw new Exception($"Не найдена цель в списке у пользователя [{userId}].");

        goal.Description = newDescription;

        _db.Goals.Update(goal);
        await _db.SaveChangesAsync();
        return goal;
    }

    /// <summary>
    /// Получить список целей пользователя.
    /// </summary>
    /// <param name="userId"></param>
    /// <returns>Список целей пользователя на текущий год.</returns>
    public async Task<IEnumerable<Goal>?> GetUserGoals(long userId)
    {
        return await _db.Goals.GetForUserInThisYear(userId).ToListAsync();
    }
    
    /// <summary>
    /// Получить кол-во целей пользователя на текущий год.
    /// </summary>
    /// <param name="userId">TelegramId пользователя.</param>
    /// <returns>Кол-во целей пользователя на текущий год.</returns>
    public async Task<int> GetUserGoalsCount(long userId)
    {
        return await _db.Goals.GetForUserInThisYear(userId).CountAsync();
    }
    
    /// <summary>
    /// Получить цель пользователя по её порядку.
    /// </summary>
    /// <param name="userId">TelegramId пользователя.</param>
    /// <param name="goalOrderNum">Порядок цели с списке. (1 -> N).</param>
    /// <returns></returns>
    private async Task<Goal?> GetUserGoalByOrderNum(long userId, int goalOrderNum)
    {
        int goalsCount = await GetUserGoalsCount(userId);
        if (goalsCount == 0) return null;

        if (goalOrderNum < 1 || goalOrderNum > goalsCount) return null;
        
        return await _db.Goals.GetForUserInThisYear(userId).Skip(goalOrderNum - 1).Take(1).FirstOrDefaultAsync();
    }
    
    
}