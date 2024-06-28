namespace FakeSpoon.Wikipedia.Mirror.Infrastructure.Cqe.Base;

public class Paged<T>
{
    public PageInfo Paging { get; set; }
    public IEnumerable<T> Items { get; set; }
}

public class PageInfo
{
    public int PageIndex { get; set; }
    public int PageSize { get; set; } = 20;
    public int Total { get; set; }
}

public class PageQuery
{
    public int PageIndex { get; set; }
    public int PageSize { get; set; } = 20;

    public string? OrderBy { get; init; }

    public OrderByDirection OrderByDirection { get; init; } = OrderByDirection.Descending;
}

public class PagedQuery
{
    public PageQuery PageQuery { get; set; }
}

public enum OrderByDirection
{
    Descending,
    Ascending
}