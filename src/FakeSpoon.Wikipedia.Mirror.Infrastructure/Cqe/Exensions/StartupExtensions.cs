using System.Reflection;
using FakeSpoon.Wikipedia.Mirror.Infrastructure.Cqe.Base;
using Microsoft.Extensions.DependencyInjection;

namespace FakeSpoon.Wikipedia.Mirror.Infrastructure.Cqe.Exensions;

public static class StartupExtensions
{
    public static IServiceCollection AddCqe(this IServiceCollection services, Assembly[] assemblies)
    {
        services.AddHandlers(typeof(ICommandHandler<>), assemblies);
        services.AddHandlers(typeof(IQueryHandler<,>), assemblies);
        services.AddHandlers(typeof(IEventHandler<>), assemblies);

        services.AddScoped<IEventPublisher, EventPublisher>();

        return services;
    }
    
    private static void AddHandlers(this IServiceCollection services, Type handlerInterface, Assembly[] assemblies, Type[]? ignoreTypes = null)
    {
        ignoreTypes ??= new Type[] { };
        var handlers = GetClassesImplementingInterface(handlerInterface, assemblies);

        foreach (var handler in handlers)
        {
            var serviceType = handler.GetInterfaces()
                .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == handlerInterface);

            if (ignoreTypes.Contains(serviceType))
            {
                continue;
            }

            services.AddScoped(
                serviceType,
                handler);
        }
    }

    private static IEnumerable<Type> GetClassesImplementingInterface(Type handlerInterface, Assembly[] assemblies)
    {
        var types = assemblies
            .SelectMany(x => x.GetTypes());
        
        return types
            .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == handlerInterface)
            );
    }
}