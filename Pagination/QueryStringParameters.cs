namespace APICatalogo.Pagination;

public abstract class QueryStringParameters
{
    private const int maxPageSize = 50;
    public int PageNumber { get; set; } = 1;
    public int _pageSize = maxPageSize;

    public int PageSize
    {
        get
        {
            return _pageSize;
        }
        set
        {
            _pageSize = (value > maxPageSize) ? maxPageSize : value;
        }
    }
}