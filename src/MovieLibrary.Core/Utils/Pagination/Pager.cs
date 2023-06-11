namespace MovieLibrary.Core.Utils.Pagination;

public class Pager : IPager
{
    private int _index = 1;
    private int _pageSize = 10;
    private string _order = "DESC";

    public int Index
    {
        get => _index;
        set => _index = value > 0 ? value : 1;
    }

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value is > 0 ? value : 10;
    }

    public string Direction
    {
        get => _order;
        set => _order = value.ToUpper() == "ASC" ? "ASC" : "DESC";
    }

    public string OrderBy { get; set; } = "Id";
}
