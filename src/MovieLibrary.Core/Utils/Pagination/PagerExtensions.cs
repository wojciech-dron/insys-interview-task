using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace MovieLibrary.Core.Utils.Pagination;

public static class PagerExtensions
{
    public static PagedList<T> ToPagedList<T>(this IQueryable<T> query, IPager? pager = null)
    {
        return PagedList<T>.Create(query, pager ?? new Pager());
    }

    public static async Task<PagedList<T>> ToPagedListAsync<T>(this IQueryable<T> query, IPager? pager = null)
    {
        return await PagedList<T>.CreateAsync(query, pager ?? new Pager());
    }

    public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, IPager pager)
    {
        return source.OrderBy($"{pager.Direction} {pager.OrderBy}");
    }
}