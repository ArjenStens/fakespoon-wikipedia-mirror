using Carter;
using FakeSpoon.Lib.Cqe.Exensions;
using FakeSpoon.Wikipedia.Mirror.Domain.Commands;
using FakeSpoon.Wikipedia.Mirror.Domain.Options;
using Microsoft.Extensions.Options;

namespace FakeSpoon.Wikipedia.Mirror.Web;

public static class Startup
{
    public static IServiceCollection AddCustomServices(this IServiceCollection services,
        WebApplicationBuilder builder)
    {
        services
            .Configure<NostrOptions>(builder.Configuration.GetSection(nameof(NostrOptions)))
            .AddCqe(new []{typeof(PostWikipediaDumpCommand).Assembly}) // Commands, Queries, Events
            .AddCarter(); // Endpoint mapping
        
        return services;
    }
    
    
    public static void AddCustomWebApplicationResources(this WebApplication app)
    {
        app.MapCarter();
    }
}