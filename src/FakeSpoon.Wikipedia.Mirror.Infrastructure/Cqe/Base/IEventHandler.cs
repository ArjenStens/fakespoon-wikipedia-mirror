namespace FakeSpoon.Wikipedia.Mirror.Infrastructure.Cqe.Base;

public interface IEventHandler<TEvent> where TEvent : IEvent
{
    public Task Handle(TEvent evnt);
}