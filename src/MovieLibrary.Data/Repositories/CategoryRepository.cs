using MovieLibrary.Data.Entities;
using MovieLibrary.Data.Repositories.Abstract;

namespace MovieLibrary.Data.Repositories;

public class CategoryRepository : Repository<Category>, ICategoryRepository
{
    public CategoryRepository(MovieLibraryContext context) : base(context)
    { }
}