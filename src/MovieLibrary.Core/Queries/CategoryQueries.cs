using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MovieLibrary.Core.Models;
using MovieLibrary.Data.Repositories;

namespace MovieLibrary.Core.Queries;

public class CategoryQueries
{
    private readonly ICategoryRepository _repository;
    private readonly IMapper _mapper;

    public CategoryQueries(ICategoryRepository repository, 
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<List<CategoryDto>> GetListAsync()
    {
        return await _repository.GetQuery()
            .ProjectTo<CategoryDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public async Task<CategoryDto> Get(int id)
    {
        return await _repository.GetQuery()
            .ProjectTo<CategoryDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(p => p.Id == id);
    }
}