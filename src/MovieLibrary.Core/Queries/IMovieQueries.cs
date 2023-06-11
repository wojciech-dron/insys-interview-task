using System.Collections.Generic;
using System.Threading.Tasks;
using MovieLibrary.Core.Dtos;

namespace MovieLibrary.Core.Queries;

public interface IMovieQueries
{
    Task<MovieDto> Get(int id);
    Task<List<MovieDto>> GetAllAsync();
}