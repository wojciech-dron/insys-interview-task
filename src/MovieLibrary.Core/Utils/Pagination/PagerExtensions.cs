using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace MovieLibrary.Core.Utils.Pagination;

public static class PagerExtensions
{
    public static PagedList<T> ToPagedList<T>(this IQueryable<T> query, IPager pager)
    {
        return PagedList<T>.Create(query, pager);
    }

    public static async Task<PagedList<T>> ToPagedListAsync<T>(this IQueryable<T> query, IPager pager)
    {
        return await PagedList<T>.CreateAsync(query, pager);
    }

    public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, IPager pager)
    {
        return source.OrderBy($"{pager.OrderBy} {pager.Direction}");
    }
}