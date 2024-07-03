namespace FakeSpoon.Lib.Cqe.Base;

public interface IEventHandler<TEvent> where TEvent : IEvent
{
    public Task Handle(TEvent evnt);
}