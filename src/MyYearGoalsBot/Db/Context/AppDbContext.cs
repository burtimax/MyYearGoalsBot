using BotFramework.Db.Entity;
using BotFramework.Extensions;
using Microsoft.EntityFrameworkCore;

namespace MyYearGoalsBot.Db.Context;

public class AppDbContext : DbContext
{
    private const string AppSchema = "app";
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Goal> Goals { get; set; }
    public DbSet<GoalsReview> GoalsReviews { get; set; }
    public DbSet<NotificationSettings> NotificationSettings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Goal>().ToTable("goals", AppSchema);
        modelBuilder.Entity<GoalsReview>().ToTable("goals_reviews", AppSchema);
        modelBuilder.Entity<NotificationSettings>().ToTable("notification_settings", AppSchema);
        
        modelBuilder.SetAllToSnakeCase();
        modelBuilder.SetFilters();
    }
    
    public override Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        foreach (var e in
                 ChangeTracker.Entries<BaseBotEntity<long>>())
        {
            switch (e.State)
            {
                case EntityState.Added:
                    e.Entity.CreatedAt = DateTimeOffset.Now;
                    break;
                case EntityState.Modified:
                    e.Entity.UpdatedAt = DateTimeOffset.Now;
                    break;
                case EntityState.Deleted:
                    e.Entity.DeletedAt = DateTimeOffset.Now;
                    e.State = EntityState.Modified;
                    break;
            }
        }

        return base.SaveChangesAsync(ct);
    }
}