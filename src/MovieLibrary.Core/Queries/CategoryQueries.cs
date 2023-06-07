using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MovieLibrary.Core.Models;
using MovieLibrary.Data.Repositories;

namespace MovieLibrary.Core.Queries;

public class CategoryQueries
{
    private readonly ICategoryRepository _repository;

    public CategoryQueries(ICategoryRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<CategoryDto>> GetListAsync()
    {
        return await _repository.GetQuery()
            .Select(p => new CategoryDto
            {
                Id = p.Id,
                Name = p.Name
            })
            .ToListAsync();
    }
}