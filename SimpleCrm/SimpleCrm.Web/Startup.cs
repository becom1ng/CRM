using Microsoft.EntityFrameworkCore;
using SimpleCrm.SqlDbServices;

namespace SimpleCrm.Web
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddSingleton<IGreeter, ConfigurationGreeter>();
            services.AddScoped<ICustomerData, SqlCustomerData>();
            services.AddDbContext<SimpleCrmDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("SimpleCrmConnection"));
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IGreeter greeter)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(new ExceptionHandlerOptions
                {
                    ExceptionHandler = context => context.Response.WriteAsync("Oops! Something went wrong.")
                });
            }

            app.UseStaticFiles();
            app.UseWelcomePage(new WelcomePageOptions
            {
                Path = "/welcome"
            });

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    "default",
                    "{controller=Home}/{action=Index}/{id?}"
                    );
                endpoints.MapControllerRoute(
                    "Create",
                    "{controller=Home}/{action=Create}"
                    );
            });

            app.Run(ctx => ctx.Response.WriteAsync("Not Found."));
        }
    }
}