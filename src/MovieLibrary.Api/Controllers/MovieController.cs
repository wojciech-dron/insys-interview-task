using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MovieLibrary.Api.Models;
using MovieLibrary.Core.Commands.Movies;
using MovieLibrary.Core.Dtos;
using MovieLibrary.Core.Queries;

namespace MovieLibrary.Api.Controllers;

[Route("api/v1/MovieManagement")]
[ApiController]
public class MovieController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly IMovieQueries _queries;

    public MovieController(IMediator mediator, 
        IMapper mapper,
        IMovieQueries queries)
    {
        _mediator = mediator;
        _mapper = mapper;
        _queries = queries;
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(MovieDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> Get(int id)
    {
        var result = await _queries.Get(id);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(MovieDto), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> Post([FromBody] CreateMovieCommand createMovieCommand)
    {
        var result = await _mediator.Send(createMovieCommand);
        return CreatedAtRoute("", new {id = result.Id}, _mapper.Map<MovieDto>(result));
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(MovieDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> Put([FromRoute] int id,
        [FromBody] UpdateMovieCommand updateMovieCommand)
    {
        updateMovieCommand.Id = id;
        var result = await _mediator.Send(updateMovieCommand);
        return Ok(_mapper.Map<MovieDto>(result));
    }

    [HttpDelete("{id}")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        await _mediator.Send(new DeleteMovieCommand { Id = id });
        return NoContent();
    }
}