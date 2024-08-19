using ETA.API.DbContexts;
using Microsoft.Extensions.Logging.AzureAppServices;

namespace ETA.API
{
    public class Program
    {
        public static IWebHostEnvironment HostingEnvironment { get; private set; }
        public static IConfiguration Configuration { get; private set; }
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            // migrate the database.  Best practice = in Main, using service scope
            using (var scope = host.Services.CreateScope())
            {
                try
                {
                    var context = scope.ServiceProvider.GetService<CourseLibraryContext>();
                }
                catch (Exception ex)
                {
                    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while migrating the database.");
                }
            }

            // run the web app
            host.Run();
        }
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseKestrel();
                    webBuilder.UseContentRoot(Directory.GetCurrentDirectory());
                    webBuilder.UseIISIntegration();
                    webBuilder.ConfigureAppConfiguration((context, configBuilder) =>
                    {
                        HostingEnvironment = context.HostingEnvironment;

                        configBuilder.SetBasePath(HostingEnvironment.ContentRootPath)
                            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                            .AddJsonFile($"appsettings.{HostingEnvironment.EnvironmentName}.json", optional: true)
                            .AddEnvironmentVariables();

                        Configuration = configBuilder.Build();
                    });
                    webBuilder.ConfigureLogging(logging =>
                    {
                        logging.ClearProviders();
                        // We have to be precise on the logging levels
                        logging.AddConsole();
                        logging.AddDebug();
                        logging.AddAzureWebAppDiagnostics();
                    })
                 .ConfigureServices(services =>
                 {
                     services.Configure<AzureFileLoggerOptions>(options =>
                     {
                         options.FileName = "eta-diagnostics-";
                         options.FileSizeLimit = 50 * 1024;
                         options.RetainedFileCountLimit = 5;
                     });
                 });
                    webBuilder.UseStartup<Startup>();
                });
    }
}
