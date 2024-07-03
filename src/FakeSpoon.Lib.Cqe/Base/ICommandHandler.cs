namespace FakeSpoon.Lib.Cqe.Base;

public interface ICommandHandler<TCommand> where TCommand : ICommand
{
    public Task Execute(TCommand cmd);
}