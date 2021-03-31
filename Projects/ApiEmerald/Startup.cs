using DatabaseAccess.Database;
using DatabaseAccess.Database.Interfaces;
using DatabaseAccess.Managers.Interfaces;
using DatabaseAccess.Managers.V3;
using DatabaseAccess.Repositories;
using DatabaseAccess.Repositories.V3;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace ApiEmerald
{
    public class Startup
    {
        private readonly string _tqcPolicySpecificOrigins = "TQC-Policy";
        private readonly string _allowAnyPolicy = "Annonymous-Policy";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var database = new SqlDatabase(Configuration);

            services.AddSingleton<IDatabase>(database);
            services.AddSingleton<IClanService>(
                new ClanV3Service(
                    new ClanV3Repository(database),
                    new ClanPlatformRepository(database),
                    new ClanMemberRepository(database)));

            services.AddCors(options =>
            {
                options.AddPolicy(name: _allowAnyPolicy,
                    builder =>
                    {
                        builder.AllowAnyHeader()
                               .AllowAnyMethod();
                    });

                options.AddPolicy(name: _tqcPolicySpecificOrigins,
                    builder =>
                    {
                        builder.WithOrigins("http://www.the-queenscourt.com/");
                    });
            });

            services.AddControllers().AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TQC Community API", Version = "v3.2.3" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TQC Community API"));
            }

            //app.UseHttpsRedirection();
            app.UseRouting();

            app.UseCors();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
