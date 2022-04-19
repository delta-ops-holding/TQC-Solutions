using Discord;
using Discord.WebSocket;
using LeadershipMinion.Core;
using LeadershipMinion.Core.Abstractions;
using LeadershipMinion.Core.Configurations;
using LeadershipMinion.Core.Helpers;
using LeadershipMinion.Logical.Data.Abstractions;
using LeadershipMinion.Logical.Data.Handlers;
using LeadershipMinion.Logical.Data.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Discord.Rest;

namespace LeadershipMinion
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            using IHost host = CreateHostBuilder(args).Build();

            var startupService = ActivatorUtilities.GetServiceOrCreateInstance<IStartupService>(host.Services);

            await startupService.InitializeBotAsync();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration((hostingContext, configuration) =>
                {
                    BuildConfiguration(hostingContext.HostingEnvironment.EnvironmentName, configuration);
                })
                .ConfigureServices((context, service) =>
                {
                    ConfigureServices(service, context.Configuration);
                })
                .ConfigureLogging(logging => {
                    logging
                    .AddConsole()
                    // .AddJsonConsole()
                    .SetMinimumLevel(LogLevel.Trace);
                });
        private static void BuildConfiguration(string envName, IConfigurationBuilder configuration) =>
            configuration
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{envName ?? "Production"}.json", optional: true)
                .AddJsonFile("clandata.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

        private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            // Create a configuration for the Discord Bot.
            var discordSocketConfiguration = new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Info,
                MessageCacheSize = 100,
                AlwaysDownloadUsers = true,
                GatewayIntents = 
                    GatewayIntents.GuildMessages |
                    GatewayIntents.DirectMessages |
                    GatewayIntents.DirectMessageReactions | 
                    GatewayIntents.GuildMessageReactions |
                    GatewayIntents.Guilds | 
                    GatewayIntents.GuildPresences | 
                    GatewayIntents.GuildMembers
            };

            // Get Bot Configuration.
            var botConfiguration = configuration.GetSection(ConstantHelper.BOT_CONFIGURATION_SECTION).Get<BotConfiguration>();
            var basicConfiguration = configuration.GetSection(ConstantHelper.BASIC_CONFIGURATION_SECTION).Get<BasicConfiguration>();
            var clanConfiguration = configuration.GetSection(ConstantHelper.CLAN_CONFIGURATION_SECTION).Get<ClanConfiguration>();

            services.AddSingleton(new DiscordSocketClient(discordSocketConfiguration));
            services.AddSingleton<DiscordRestClient>();

            services.AddSingleton<IBotConfiguration>(botConfiguration);
            services.AddSingleton<IBasicConfiguration>(basicConfiguration);
            services.AddSingleton<IClanConfiguration>(clanConfiguration);

            services.AddSingleton<RuntimeHelper>();
            services.AddScoped<IEmbedService, EmbedService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IApplicationHandler, ApplicationHandler>();

            services.AddSingleton<IStartupService, StartupService>();
        }
    }
}