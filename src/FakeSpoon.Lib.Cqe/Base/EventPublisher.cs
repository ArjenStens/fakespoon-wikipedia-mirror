using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FakeSpoon.Lib.Cqe.Base;

public interface IEventPublisher
{
    public Task Publish<TEvent>(TEvent eventToPublish) where TEvent : IEvent;
}

public class EventPublisher(
    ILogger<EventPublisher> logger,
    IServiceProvider serviceProvider
) : IEventPublisher
{
    public async Task Publish<TEvent>(TEvent evnt) where TEvent : IEvent
    {
        await using var scope = serviceProvider.CreateAsyncScope();
        
        var eventHandlers = scope.ServiceProvider.GetServices<IEventHandler<TEvent>>();
            
        foreach (var eventHandler in eventHandlers)
        {
            await eventHandler.Handle(evnt);
        }
    }
}