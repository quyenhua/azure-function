using Microsoft.EntityFrameworkCore;

namespace Application.Common.Models;

public class PaginatedList<T>(List<T> items, int count, int pageIndex, int pageSize)
{
    public List<T> Items { get; } = items;
    public int PageIndex { get; } = pageIndex;
    public int TotalPages { get; } = (int)Math.Ceiling(count / (double)pageSize);
    public int TotalItems { get; } = count;

    public bool HasPreviousPage => PageIndex > 1;

    public bool HasNextPage => PageIndex < TotalPages;

    public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
    {
        var count = await source.CountAsync();
        var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

        return new PaginatedList<T>(items, count, pageIndex, pageSize);
    }
}
