namespace FakeSpoon.Lib.Cqe.Base;

public interface IQueryHandler<TQuery, TResult>
    where TQuery : IQuery<TResult>
{
    public Task<TResult> Run(TQuery query);
}