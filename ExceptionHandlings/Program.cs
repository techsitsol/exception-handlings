using ExceptionHandlings;
using ExceptionHandlings.Shared;
using Serilog;


var builder = CreateHostBuilder(args).Build();
AppSettingsHelper.AppSettingConfigure(config: builder.Services.GetRequiredService<IConfiguration>());
builder.Run();


static IHostBuilder CreateHostBuilder(string[] args) =>
           Host.CreateDefaultBuilder(args)
               .ConfigureLogging(logging =>
               {
                   var logger = new LoggerConfiguration()
                    .ReadFrom.Configuration(new ConfigurationBuilder()
                    .AddJsonFile("seri-log.config.json")
                    .Build())
                    .Enrich.FromLogContext()
                    .CreateLogger();
                   logging.ClearProviders();
                   logging.AddSerilog(logger);
               })
               .ConfigureWebHostDefaults(webBuilder =>
               {
                   webBuilder.UseStartup<Startup>();
               });
