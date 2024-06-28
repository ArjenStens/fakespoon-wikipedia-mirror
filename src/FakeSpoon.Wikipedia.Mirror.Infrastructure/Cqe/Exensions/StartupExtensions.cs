using FakeSpoon.Wikipedia.Mirror.Infrastructure.Cqe.Base;
using Microsoft.Extensions.DependencyInjection;

namespace FakeSpoon.Wikipedia.Mirror.Infrastructure.Cqe.Exensions;

public static class StartupExtensions
{
    public static IServiceCollection AddCqe(this IServiceCollection services)
    {
        services.AddHandlers(typeof(ICommandHandler<>));
        services.AddHandlers(typeof(IQueryHandler<,>));
        services.AddHandlers(typeof(IEventHandler<>));

        services.AddScoped<IEventPublisher, EventPublisher>();

        return services;
    }
    
    private static void AddHandlers(this IServiceCollection services, Type handlerInterface, Type[]? ignoreTypes = null)
    {
        ignoreTypes ??= new Type[] { };
        var handlers = GetClassesImplementingInterface(handlerInterface);

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

    private static IEnumerable<Type> GetClassesImplementingInterface(Type handlerInterface)
    {
        return typeof(StartupExtensions).Assembly.GetTypes()
            .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == handlerInterface)
            );
    }
}