using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MovieLibrary.Core.Dtos;
using MovieLibrary.Core.Queries.Filters;
using MovieLibrary.Core.Utils.Pagination;
using MovieLibrary.Data.Repositories;

namespace MovieLibrary.Core.Queries;

public class MovieQueries : IMovieQueries
{
    private readonly IMovieRepository _repository;
    private readonly IMapper _mapper;

    public MovieQueries(IMovieRepository repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    
    public async Task<MovieDto> Get(int id)
    {
        return await _repository.GetQuery()
            .ProjectTo<MovieDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<PagedList<MovieDto>> GetFilteredAsync(MovieFilter filter)
    {
        return await _repository.GetQuery()
            .Filter(filter)
            .OrderBy(filter)
            .ProjectTo<MovieDto>(_mapper.ConfigurationProvider)
            .ToPagedListAsync(filter);
    }
}