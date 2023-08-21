namespace SimpleCrm.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var app = builder.Build();

            ConfigurationManager configuration = builder.Configuration;

            var greeting = configuration["Greeting"];
            app.MapGet("/", () => greeting);

            app.Run();
        }
    }
}