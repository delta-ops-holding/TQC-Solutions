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

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration((hostingContext, configuration) =>
                {
                    BuildConfiguration(hostingContext.HostingEnvironment.EnvironmentName, configuration);
                })
                .ConfigureServices((context, service) =>
                {
                    ConfigureServices(service, context.Configuration);
                });

        public static void BuildConfiguration(string envName, IConfigurationBuilder configuration) =>
            configuration
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{envName ?? "Production"}.json", optional: true)
                .AddEnvironmentVariables();

        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
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
            var botConfiguration = configuration.GetSection(ConstantHelper.BOT_CONFIGURATION_SECTION_NAME).Get<BotConfiguration>();

            services.AddSingleton(new DiscordSocketClient(discordSocketConfiguration));
            services.AddSingleton<DiscordRestClient>();

            services.AddSingleton<IBotConfiguration>(botConfiguration);

            services.AddSingleton<RuntimeHelper>();

            services.AddScoped<IClanService, ClanService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IApplicationHandler, ApplicationHandler>();

            services.AddSingleton<IStartupService, StartupService>();
        }
    }
}
