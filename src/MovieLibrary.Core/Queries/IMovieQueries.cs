using System.Threading.Tasks;
using MovieLibrary.Core.Dtos;
using MovieLibrary.Core.Queries.Filters;
using MovieLibrary.Core.Utils.Pagination;

namespace MovieLibrary.Core.Queries;

public interface IMovieQueries
{
    Task<MovieDto> Get(int id);
    Task<PagedList<MovieDto>> GetFilteredAsync(MovieFilter filter);
}