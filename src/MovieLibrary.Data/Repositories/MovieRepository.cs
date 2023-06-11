using MovieLibrary.Data.Entities;
using MovieLibrary.Data.Repositories.Abstract;

namespace MovieLibrary.Data.Repositories;

public class MovieRepository : Repository<Movie>, IMovieRepository
{
    public MovieRepository(MovieLibraryContext context) : base(context)
    { }
}