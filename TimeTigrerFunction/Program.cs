using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using TimeTigrerFunction.Model;
using TimeTigrerFunction;

internal class Program
{
    private static void Main(string[] args)
    {
        var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureAppConfiguration((builderContext, config) =>
    {
        //config.AddJsonFile("local.settings.json", optional: false, reloadOnChange: true);
        config.AddJsonFile("local.settings.json", optional: false, reloadOnChange: true);
    }).ConfigureServices((builderContext, services) => { IConfigurationSection SMTPSettings = builderContext.Configuration.GetSection("SMTPSettings");
        SMTPSetting mySetting = SMTPSettings.Get<SMTPSetting>();
        services.AddSingleton<SMTPSetting>(mySetting);

        EmailService.SMTPSetting = mySetting;
    })
    .Build();

        host.Run();
    }
}