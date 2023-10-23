using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using SimpleCrm.SqlDbServices;
using SimpleCrm.WebApi.Auth;

namespace SimpleCrm.WebApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<SimpleCrmDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("SimpleCrmConnection")));
            services.AddDbContext<CrmIdentityDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("SimpleCrmConnection")));
            services.AddDefaultIdentity<CrmUser>()
              .AddDefaultUI()
              .AddEntityFrameworkStores<CrmIdentityDbContext>();

            services.AddControllersWithViews();
            services.AddRazorPages();


            var googleOptions = Configuration.GetSection(nameof(GoogleAuthSettings));
            services.Configure<GoogleAuthSettings>(options =>
            {
                options.ClientId = googleOptions[nameof(GoogleAuthSettings.ClientId)];
                options.ClientSecret = googleOptions[nameof(GoogleAuthSettings.ClientSecret)];
            });
            var microsoftOptions = Configuration.GetSection(nameof(MicrosoftAuthSettings));
            services.Configure<MicrosoftAuthSettings>(options =>
            {
                options.ClientId = microsoftOptions[nameof(MicrosoftAuthSettings.ClientId)];
                options.ClientSecret = microsoftOptions[nameof(MicrosoftAuthSettings.ClientSecret)];
            });

            services.AddAuthentication()
                .AddCookie(cfg => cfg.SlidingExpiration = true)
                .AddGoogle(options =>
                {
                    options.ClientId = googleOptions[nameof(GoogleAuthSettings.ClientId)];
                    options.ClientSecret = googleOptions[nameof(GoogleAuthSettings.ClientSecret)];
                })
               .AddMicrosoftAccount(options =>
                {
                    options.ClientId = microsoftOptions[nameof(MicrosoftAuthSettings.ClientId)];
                    options.ClientSecret = microsoftOptions[nameof(MicrosoftAuthSettings.ClientSecret)];
                });
            ;

            services.AddSpaStaticFiles(config =>
            {
                config.RootPath = Configuration["SpaRoot"];
            });

            services.AddScoped<ICustomerData, SqlCustomerData>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });

            app.UseWhen(
                context => !context.Request.Path.StartsWithSegments("/api"),
                appBuilder => appBuilder.UseSpa(spa =>
                {
                    if (env.IsDevelopment())
                    {
                        spa.Options.SourcePath = "../simple-crm-cli";
                        spa.Options.StartupTimeout = new TimeSpan(0, 0, 300); //300 seconds
                        spa.UseAngularCliServer(npmScript: "start");
                    }
                }));
        }
    }
}