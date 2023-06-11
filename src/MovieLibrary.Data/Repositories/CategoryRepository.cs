using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MovieLibrary.Data.Entities;
using MovieLibrary.Data.Repositories.Abstract;

namespace MovieLibrary.Data.Repositories;

public class CategoryRepository : Repository<Category>, ICategoryRepository
{
    public CategoryRepository(MovieLibraryContext context) : base(context)
    { }

    public async Task<bool> IsNameUniqueAsync(string name)
    {
        return await DbSet.AllAsync(c => c.Name != name);
    }
}