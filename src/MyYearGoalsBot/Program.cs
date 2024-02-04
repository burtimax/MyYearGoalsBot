using System.Reflection;
using BotFramework.Db;
using BotFramework.Extensions;
using BotFramework.Options;
using BotFramework.Other;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyYearGoalsBot.App.Options;
using MyYearGoalsBot.Db.Context;
using MyYearGoalsBot.Extensions;
using MyYearGoalsBot.Resources;
using MyYearGoalsBot.Services;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var builder = WebApplication.CreateBuilder(args);
IServiceCollection services = builder.Services;

// Initialize

// Register options
services.AddLogging();
services.Configure<ApplicationConfiguration>(builder.Configuration);
services.Configure<BotConfiguration>(builder.Configuration.GetSection("Bot"));
services.Configure<QuartzAppOptions>(builder.Configuration.GetSection("Quartz"));
services.Configure<BotOptions>(builder.Configuration.GetSection("BotOptions"));
var botConfig = builder.Configuration.GetSection("Bot").Get<BotConfiguration>();
QuartzAppOptions quartzConfig = builder.Configuration.GetSection("Quartz").Get<QuartzAppOptions>()!;
BotResources botResources = services.ConfigureBotResources(botConfig.ResourcesFilePath);
services.AddBot(botConfig);
services.AddQuartzJobs(quartzConfig);
services.AddDbContext<AppDbContext>(
    options => options.UseNpgsql(botConfig.DbConnection));

// Add services to the container.
services.AddControllers().AddNewtonsoftJson();
services.AddHttpContextAccessor();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();

services.AddTransient<UserGoalsService>();

var app = builder.Build();

// Чтобы сервер разрешал запросы от Telegram сервера.
app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

// Миграция контекста приложения.
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<MyYearGoalsBot.Db.Context.AppDbContext>();
    await db.Database.MigrateAsync();
}

//app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.MapControllers();
app.Run();