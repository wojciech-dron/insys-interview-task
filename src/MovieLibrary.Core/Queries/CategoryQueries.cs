using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MovieLibrary.Core.Dtos;
using MovieLibrary.Core.Exceptions;
using MovieLibrary.Data.Entities;
using MovieLibrary.Data.Repositories;

namespace MovieLibrary.Core.Queries;

public class CategoryQueries : ICategoryQueries
{
    private readonly ICategoryRepository _repository;
    private readonly IMapper _mapper;

    public CategoryQueries(ICategoryRepository repository, 
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<CategoryDto> Get(int id)
    {
        return (await _repository.GetQuery()
            .ProjectTo<CategoryDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(p => p.Id == id))
            ?? throw new NotFoundException(typeof(Category), id);
    }
    
    public async Task<List<CategoryDto>> GetAllAsync()
    {
        return await _repository.GetQuery()
            .ProjectTo<CategoryDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
    }
}