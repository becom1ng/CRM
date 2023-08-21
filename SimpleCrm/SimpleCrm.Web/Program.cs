namespace SimpleCrm.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddSingleton<IGreeter, ConfigurationGreeter>();
            var app = builder.Build();

            ConfigurationManager configuration = builder.Configuration;

            //var greeting = configuration["Greeting"];
            var greeting = greeter.GetGreeting();
            app.MapGet("/", () => greeting);

            app.Run();
        }

        private readonly IGreeter _greeter;

        public Program(IGreeter greeter) 
        {
            _greeter = greeter;
        }
    }
}