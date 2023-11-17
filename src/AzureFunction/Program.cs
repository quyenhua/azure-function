using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Application.Common.Interfaces;
using FunctionApp;

namespace FunctionApp;

class Program
{
    static async Task Main(string[] args)
    {
        // #if DEBUG
        //             Debugger.Launch();
        // #endif
        var host = new HostBuilder()
            .ConfigureAppConfiguration(c =>
            {
                c.AddCommandLine(args);
                c.AddJsonFile("appsettings.json", true);
                c.AddJsonFile("local.settings.json", true);
            })
            .ConfigureFunctionsWorkerDefaults()
            .ConfigureServices((c, s) =>
            {
                s.AddSingleton<IHttpRequestProcessor, HttpRequestProcessor>();
                s.AddSingleton<ICurrentUserService, CurrentUserService>();

                var startup = new Startup(c.Configuration);
                startup.ConfigureServices(s);
            })
            .Build();

        await host.RunAsync();
    }
}

internal class CurrentUserService : ICurrentUserService
{
    public string UserId => "";
}