using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using passman_back.Domain.Interfaces.DbContexts;
using passman_back.Infrastructure.Business.MailService;
using passman_back.Infrastructure.Business.MappingProfiles;
using passman_back.Infrastructure.Business.Settigns;
using passman_back.Infrastructure.Data.DbContexts;
using passman_back.Infrastructure.Domain.Settings;
using passman_back.IoC;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace passman_back {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {

            // Di of appsettings
            var mailSettingSection = Configuration.GetSection("MailSettings");
            var mainDbSettingSection = Configuration.GetSection("MainDbSettings");
            var passmanSettingsSection = Configuration.GetSection("PassmanSettings");
            services.Configure<PassmanSettings>(passmanSettingsSection);
            services.Configure<MailSettings>(mailSettingSection);
            services.Configure<MainDbSettings>(mainDbSettingSection);
            //

            #region  Allowed_Origins
            var allowedOriginsSection = Configuration.GetSection("AllowedOrigins");
            var allowedOrigins = allowedOriginsSection.Value
                ?.Split(';')
                ?.Where(x => !string.IsNullOrEmpty(x))
                ?.ToList();
            var allowedOriginsEnv = Environment
                .GetEnvironmentVariable("ALLOWED_ORIGINS")
                ?.Split(';')
                ?.Where(x => !string.IsNullOrEmpty(x))
                ?.ToList();

            if (allowedOriginsEnv != null && allowedOriginsEnv.Count > 0) {
                allowedOrigins.AddRange(allowedOriginsEnv);
            }

            var frontendUrl = Environment.GetEnvironmentVariable("FRONTEND_URL");

            if (!string.IsNullOrEmpty(frontendUrl)) {
                allowedOrigins.Add(frontendUrl);
            }
            #endregion

            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "passman_back", Version = "v1" });
            });

            var jsonOptions = new JsonSerializerOptions() {
                MaxDepth = 99
            };

            services.AddHttpContextAccessor();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddControllers().AddJsonOptions(j => {
                j.JsonSerializerOptions.MaxDepth = 999;
            });
            services.AddDbContext<IMainDbContext, MainDbContext>();
            services.AddAutoMapper(typeof(DefaultMapperProfiles));
            services.ConfigureRepositoty();
            services.ConfigureValidators();
            services.ConfigureServices();
            services.ConfigureExportImport();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie("Cookies", "Cookies",
                    options => {
                        options.Events.OnRedirectToAccessDenied = context => {
                            context.Response.StatusCode = 403;
                            return Task.CompletedTask;
                        };
                    });

            services.AddCors(options =>
                options.AddPolicy("OurPolicy", cors =>
                    cors
                    .AllowAnyHeader()
                    .WithMethods("CREATE", "PUT", "POST", "DELETE")
                    .WithOrigins(allowedOrigins.ToArray())
                    .AllowCredentials()
                )
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (!env.IsProduction()) {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "passman_back v1"));
            }
            app.UseRouting();
            app.UseCors("OurPolicy");
            app.UseAuthentication();
            app.UseAuthorization();
            
            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
        }
    }
}
