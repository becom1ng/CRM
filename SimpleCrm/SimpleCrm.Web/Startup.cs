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

            app.UseWelcomePage("/welcome");

            app.UseRouting();

            var greeting = greeter.GetGreeting();
            app.MapGet("/", () => greeting);
            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapGet("/", async context =>
            //    {
            //        var message = greeter.GetGreeting();
            //        await context.Response.WriteAsync(message);
            //    });
            //});
        }
    }
}