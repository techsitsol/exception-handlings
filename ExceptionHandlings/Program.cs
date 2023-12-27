using ExceptionHandlings;
using ExceptionHandlings.Shared;


var builder = CreateHostBuilder(args).Build();
AppSettingsHelper.AppSettingConfigure(config: builder.Services.GetRequiredService<IConfiguration>());
builder.Run();


static IHostBuilder CreateHostBuilder(string[] args) =>
           Host.CreateDefaultBuilder(args)
               .ConfigureWebHostDefaults(webBuilder =>
               {
                   webBuilder.UseStartup<Startup>();
               });
