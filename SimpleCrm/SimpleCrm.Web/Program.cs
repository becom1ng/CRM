namespace SimpleCrm.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var app = builder.Build();

            app.MapGet("/", () => "Hello and welcome to my first ASP.NET application!");

            app.Run();
        }
    }
}