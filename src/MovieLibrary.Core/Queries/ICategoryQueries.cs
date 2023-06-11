using System.Collections.Generic;
using System.Threading.Tasks;
using MovieLibrary.Core.Dtos;

namespace MovieLibrary.Core.Queries;

public interface ICategoryQueries
{
    Task<List<CategoryDto>> GetAllAsync();
    Task<CategoryDto> Get(int id);
}