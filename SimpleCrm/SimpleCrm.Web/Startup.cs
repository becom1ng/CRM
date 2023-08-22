namespace SimpleCrm.Web
{
    public class Startup
    {
        // SERVICES - This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
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

            app.UseFileServer();
            //app.UseDefaultFiles();
            //app.UseStaticFiles();
            app.UseWelcomePage("/welcome");

            app.UseRouting();

            app.MapGet("/hello", async context =>
            {
                var message = greeter.GetGreeting();
                await context.Response.WriteAsync(message);
            });

            //var greeting = greeter.GetGreeting();
            //app.MapGet("/", () => greeting);
        }
    }
}