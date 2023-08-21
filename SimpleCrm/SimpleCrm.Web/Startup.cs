using Microsoft.AspNetCore.Hosting;

namespace SimpleCrm.Web
{
    public class Startup
    {
        public IConfiguration Configuration { get; } // Injected Property

        public Startup(IConfiguration configuration) // Inject needed service into constructor
        {
            this.Configuration = configuration; // Set the Injected property
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IGreeter, ConfigurationGreeter>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(WebApplication app, IWebHostEnvironment env, IGreeter greeter)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            var greeting = greeter.GetGreeting();
            app.MapGet("/", () => greeting);
        }
    }
}