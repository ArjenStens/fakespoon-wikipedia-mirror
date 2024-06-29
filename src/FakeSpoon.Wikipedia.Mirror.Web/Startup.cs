using Carter;
using FakeSpoon.Wikipedia.Mirror.Domain.Commands;
using FakeSpoon.Wikipedia.Mirror.Infrastructure.Cqe.Exensions;

namespace FakeSpoon.Wikipedia.Mirror.Web;

public static class Startup
{
    public static IServiceCollection AddCustomServices(this IServiceCollection services,
        WebApplicationBuilder builder)
    {

        services
            .AddCqe(new []{typeof(PostWikipediaDumpCommand).Assembly}) // Commands, Queries, Events
            .AddCarter(); // Endpoint mapping
        
        return services;
    }
    
    
    public static void AddCustomWebApplicationResources(this WebApplication app)
    {
        app.MapCarter();
    }

    
}