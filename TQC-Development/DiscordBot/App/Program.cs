using DatabaseAccess.Database;
using DatabaseAccess.Database.Interfaces;
using DatabaseAccess.Repositories;
using DatabaseAccess.Repositories.Interfaces;
using Discord;
using Discord.Commands;
using Discord.Net;
using Discord.WebSocket;
using DiscordBot.Commands;
using DiscordBot.Commands.Modules;
using DiscordBot.Interfaces;
using DiscordBot.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static DiscordBot.Commands.Modules.InfoModule;

namespace DiscordBot
{
    public class Program
    {
        private static async Task Main(string[] args)
        {
            var builder = new ConfigurationBuilder();
            BuildConfig(builder);

            var host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    var config = new DiscordSocketConfig
                    {
                        LogLevel = LogSeverity.Info,
                        MessageCacheSize = 100,
                        ExclusiveBulkDelete = false,
                        AlwaysDownloadUsers = true
                    };

                    services.AddSingleton(config);
                    services.AddSingleton(new DiscordSocketClient(config));

                    // Database Services.
                    services.AddScoped<IDatabase, SqlDatabase>();
                    services.AddScoped<ILogRepository, DatabaseLogRepository>();                    

                    // Utility Services.
                    services.AddScoped<IDataService, DataService>();
                    services.AddScoped<INotifier, NotificationService>();
                    services.AddScoped<IClanApplication, ClanApplicationService>();

                    // Log Services.
                    services.AddSingleton<ILogger, LogService>();

                    // Startup Services.
                    services.AddSingleton<IStartup, MinionStartup>();                    

                    // Commands.                    
                    services.AddScoped<CommandService>();
                    services.AddScoped<ICommandHandler, CommandHandler>();

                    // Command Modules.
                    services.AddSingleton<InfoModule>();
                    services.AddSingleton<LogModule>();
                })
                .Build();

            var startupService = ActivatorUtilities.GetServiceOrCreateInstance<IStartup>(host.Services);

            await startupService.InitBotAsync();
        }

        private static void BuildConfig(IConfigurationBuilder builder)
        {
            builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
                .AddEnvironmentVariables();
        }
    }
}
