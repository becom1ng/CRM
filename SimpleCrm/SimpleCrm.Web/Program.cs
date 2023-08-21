namespace SimpleCrm.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // REGISTER SERVICES HERE
            builder.Services.AddRazorPages();
            builder.Services.AddSingleton<IGreeter, ConfigurationGreeter>();

            var app = builder.Build();

            // REGISTER MIDDLEWARE HERE

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // Use appsettings.Development.json for greeting. Step 004 of lesson.
            //ConfigurationManager configuration = builder.Configuration;
            //var greeting = configuration["Greeting"];
            //app.MapGet("/", () => greeting);
            
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();
            app.MapRazorPages();

            var greeter = app.Services.GetService<IGreeter>();
            app.MapGet("/", () => greeter.GetGreeting());

            app.Run();
        }

        //private readonly IGreeter _greeter;

        //public Program(IGreeter greeter)
        //{
        //    _greeter = greeter;
        //}
    }
}