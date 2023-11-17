using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Application;
using Infrastructure;

namespace FunctionApp;

public class Startup(IConfiguration configuration)
{
    private readonly IConfiguration configuration = configuration;

    public void ConfigureServices(IServiceCollection collection)
    {
        collection.AddApplication();

        // TODO The Azure Function should not have any dependency on Infrastructure other than DI
        // Determine a better technique
        collection.AddInfrastructure(configuration);
    }
}