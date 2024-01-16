using MyYearGoalsBot.Db;

namespace MyYearGoalsBot.Extensions;

public static class IQueryableGoalsExtension
{
    /// <summary>
    /// Получить список целей для пользователя с учетом правильной сортировки и только для этого года.
    /// </summary>
    /// <param name="userId">TelegramId пользователя.</param>
    /// <returns></returns>
    public static IQueryable<Goal> GetForUserInThisYear(this IQueryable<Goal> query, long userId)
    {
        return query
            .ForUser(userId)
            .Order()
            .Where(g => g.CreatedAt.Year == DateTime.Now.Year);
    }
    
    /// <summary>
    /// Правильно отсортировать цели пользователя.
    /// </summary>
    /// <param name="query">Запрос целей.</param>
    /// <returns></returns>
    private static IQueryable<Goal> Order(this IQueryable<Goal> query)
    {
        return query.OrderByDescending(g => g.Priority).ThenBy(g => g.CreatedAt);
    }
    
    /// <summary>
    /// Получить цели для пользователя.
    /// </summary>
    /// <param name="query"></param>
    /// <param name="userId"></param>
    /// <returns>Список целей в правильной сортировке.</returns>
    private static IQueryable<Goal> ForUser(this IQueryable<Goal> query, long userId)
    {
        return query.Where(g => g.UserId == userId);
    }
}