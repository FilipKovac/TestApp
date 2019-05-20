using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TestApp
{
    public class Startup
    {
        public const string CookieScheme = "AppUser";

        public Startup(IConfiguration configuration)
        { 
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddAuthentication(CookieScheme)
                .AddCookie(CookieScheme, options =>
                {
                    options.AccessDeniedPath = "/account/denied";
                    options.LoginPath = "/account/login";
                });

            // Add access to settings from appsettings to controllers
            services.AddSingleton<IConfiguration>(Configuration);
            
            // Create context according to appsettings config file
            switch (Configuration.GetValue<string>("AppSettings:UseSource", "default"))
            {
                case "XML":
                    services.AddSingleton<Models.IClient>(c => new Models.Xml.LibraryClient(Configuration.GetValue<string>("AppSettings:Source", "default")));
                    break;
                default:
                    Static.Logger.Fatal("No database source was picked");
                    System.Environment.Exit(0);
                    break;
            }

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
