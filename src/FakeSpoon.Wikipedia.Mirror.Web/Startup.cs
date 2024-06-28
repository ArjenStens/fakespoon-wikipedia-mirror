using FakeSpoon.Wikipedia.Mirror.Infrastructure.Cqe.Exensions;

namespace FakeSpoon.Wikipedia.Mirror.Web;

public static class Startup
{
    public static IServiceCollection AddCustomServices(this IServiceCollection services,
        WebApplicationBuilder builder)
    {
        // Automatically add all CQE services
        services.AddCqe();
        
        return services;
    }

    
}