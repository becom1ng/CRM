namespace SimpleCrm.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var startup = new Startup(); // My custom startup class.
            startup.ConfigureServices(builder.Services); // Add services to the container.

            // REGISTER SERVICES HERE if using the new .net 6 method
            // ex: builder.Services.AddSingleton<IGreeter, ConfigurationGreeter>();

            var app = builder.Build();
            
            // REGISTER MIDDLEWARE HERE if using the new .net 6 method

            IGreeter greeter = new ConfigurationGreeter(builder.Configuration);
            startup.Configure(app, app.Environment, greeter); // Configure the HTTP request pipeline.
        }
    }    
}