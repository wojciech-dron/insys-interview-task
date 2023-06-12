using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MovieLibrary.Data.Entities;
using MovieLibrary.Data.Repositories.Abstract;

namespace MovieLibrary.Data.Repositories;

public class CategoryRepository : Repository<Category>, ICategoryRepository
{
    public CategoryRepository(MovieLibraryContext context) : base(context)
    { }

    public async Task<List<Category>> GetAsync(List<int> categoriesIds, CancellationToken cancellationToken = default)
    {
        return await DbSet.Where(c => categoriesIds.Contains(c.Id))
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> IsNameUniqueAsync(string name, int exceptId = 0, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Where(c => c.Id != exceptId)
            .AllAsync(c => c.Name != name, cancellationToken);
    }

    public async Task<bool> ExistsAsync(IEnumerable<int> ids, CancellationToken cancellationToken = default)
    {
        var fixedIds = ids.Distinct().ToArray();
        
        var dbIds = await DbSet
            .Where(c => fixedIds.Contains(c.Id))
            .Select(x => x.Id)
            .ToListAsync(cancellationToken);

        return dbIds.Count == fixedIds.Length;
    }
}