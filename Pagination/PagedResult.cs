namespace FitnesTracker;

public class PagedResult<T> where T : class
{
    public PagedResult(List<T> items, int totalCount, int pageNumber, int pageSize)
    {
        Items = items; // data of currnet page
        TotalCount = totalCount; // item in DB
        PageNumber = pageNumber; // number of current page
        PageSize = pageSize; // items on page
    }

    public List<T> Items { get; set; }
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
}
