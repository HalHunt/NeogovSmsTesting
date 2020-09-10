using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Neogov.Sms.Tester.Models;
using Neogov.Sms.Tester.Services;

namespace Neogov.Sms.Tester
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<MessageDbContext>(options => options.UseSqlite("Data Source=messages.db"));

            services.AddControllersWithViews();

            services.AddSignalR();

            services.AddScoped<ValidateTwilioRequestAttribute>();
            services.AddScoped<IMessageRepository, MessageRepository>();

            var appConfig = new AppConfiguration
            {
                TwilioAccountSid = Configuration[AppConfiguration.ConfigKeyTwilioAccountSid],
                TwilioAuthToken = Configuration[AppConfiguration.ConfigKeyTwilioAuthToken],
                MessageToNumber = Configuration[AppConfiguration.ConfigKeyMessageToNumber]
            };
            if (!string.IsNullOrWhiteSpace(Configuration[AppConfiguration.ConfigKeyMessagesPageSize]) && int.TryParse(Configuration[AppConfiguration.ConfigKeyMessagesPageSize], out _))
                appConfig.MessagesPageSize = int.Parse(Configuration[AppConfiguration.ConfigKeyMessagesPageSize]);
            
            services.AddSingleton(typeof(AppConfiguration), appConfig);

            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();
            app.UseSpaStaticFiles();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<MessageHub>("/hub/messages");
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });
        }
    }
}
