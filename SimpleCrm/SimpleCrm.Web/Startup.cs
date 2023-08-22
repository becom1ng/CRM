namespace SimpleCrm.Web
{
    public class Startup
    {
        // SERVICES - This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddSingleton<IGreeter, ConfigurationGreeter>();
        }

        // MIDDLEWARE - This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(WebApplication app, IWebHostEnvironment env, IGreeter greeter)
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

            app.UseRouting();

            app.MapControllerRoute(
                "default",
                "{controller=Home}/{action=Index}/{id?}"
                );
            app.MapControllerRoute(
                name: "contact",
                pattern: "Contact/{phone}",
                constraints: new { phone = "^\\d{3}-\\d{3}\\d{4}$" },
                defaults: new { controller = "Contact", action = "List" }
                );

            app.Run();
            app.Run(ctx => ctx.Response.WriteAsync("Not Found"));
        }
    }
}