using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MovieLibrary.Data.Entities;
using MovieLibrary.Data.Repositories.Abstract;

namespace MovieLibrary.Data.Repositories;

public class MovieRepository : Repository<Movie>, IMovieRepository
{
    public MovieRepository(MovieLibraryContext context) : base(context)
    { }

    public override async Task<Movie> GetAsync(int id, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Include(m => m.MovieCategories)
            .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);

    }
}