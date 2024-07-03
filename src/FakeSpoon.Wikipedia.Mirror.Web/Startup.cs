using Carter;
using FakeSpoon.Lib.Cqe.Exensions;
using FakeSpoon.Wikipedia.Mirror.Domain.Commands;

namespace FakeSpoon.Wikipedia.Mirror.Web;

public static class Startup
{
    public static IServiceCollection AddCustomServices(this IServiceCollection services,
        WebApplicationBuilder builder)
    {

        services
            .AddCqe(new []{typeof(PostWikipediaDumpCommand).Assembly}) // Commands, Queries, Events
            .AddCarter(); // Endpoint mapping

        // services.AddTransient<IRelayConnection, RelayConnection>();
        
        return services;
    }
    
    
    public static void AddCustomWebApplicationResources(this WebApplication app)
    {
        app.MapCarter();
    }
}