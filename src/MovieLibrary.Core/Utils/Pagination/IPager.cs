namespace MovieLibrary.Core.Utils.Pagination;

public interface IPager
{
    public int Index { get; }
    public int PageSize { get; }
    public string Direction { get; }
    public string OrderBy { get; }
}