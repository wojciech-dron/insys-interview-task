using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MovieLibrary.Core.Utils.Pagination;

public class PagedList<T> : IPager
{
    public int Index { get; private set; }
    public int PageSize { get; }
    public string Direction { get; }
    public string OrderBy { get; }
    
    public int TotalCount { get; private set; }
    public int TotalPages { get; private set; }
    public int Count { get; }
    public List<T> Results { get; }

    
    private PagedList() : base() // for mapper
    { }

    private PagedList(List<T> items, IPager pager)
    {
        Results = items;
        Index = pager.Index;
        PageSize = pager.PageSize;
        Direction = pager.Direction;
        OrderBy = pager.OrderBy;
        Count = items.Count;
    }

    public static PagedList<T> Create(IQueryable<T> query, IPager pager)
    {
        var totalCount = query.Count();
        var totalPages = (int)Math.Ceiling(totalCount / (double)pager.PageSize);
        var fixedIndex = pager.Index <= totalPages ? pager.Index : totalPages;
        
        var items = query
            .Skip((fixedIndex - 1) * pager.PageSize)
            .Take(pager.PageSize)
            .ToList();

        return new PagedList<T>(items, pager)
        {
            TotalCount = totalCount,
            TotalPages = totalPages,
            Index = fixedIndex
        };
    }
    
    public static async Task<PagedList<T>> CreateAsync(IQueryable<T> query, IPager pager)
    {
        var totalCount = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalCount / (double)pager.PageSize);
        var fixedIndex = pager.Index <= totalPages ? pager.Index : totalPages;

        var items = await query
            .Skip((fixedIndex - 1) * pager.PageSize)
            .Take(pager.PageSize)
            .ToListAsync();

        return new PagedList<T>(items, pager)
        {
            TotalCount = totalCount,
            TotalPages = totalPages,
            Index = fixedIndex
        };
    }
}
