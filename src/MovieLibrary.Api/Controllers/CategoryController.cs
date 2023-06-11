using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MovieLibrary.Core.Commands.Categories;
using MovieLibrary.Core.Dtos;
using MovieLibrary.Core.Queries;

namespace MovieLibrary.Api.Controllers;

[Route("api/v1/CategoryManagement")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly ICategoryQueries _queries;

    public CategoryController(IMediator mediator, 
        IMapper mapper,
        ICategoryQueries queries)
    {
        _mediator = mediator;
        _mapper = mapper;
        _queries = queries;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetList()
    {
        var result = await _queries.GetAllAsync();
        return Ok(result);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(CategoryDto), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Get(int id)
    {
        var result = await _queries.Get(id);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(CategoryDto), (int)HttpStatusCode.Created)]
    public async Task<IActionResult> Post([FromBody] CreateCategoryCommand createCategoryCommand)
    {
        var result = await _mediator.Send(createCategoryCommand);
        return CreatedAtRoute("", new {id = result.Id}, _mapper.Map<CategoryDto>(result));
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(CategoryDto), (int)HttpStatusCode.Accepted)]
    public async Task<IActionResult> Put([FromRoute] int id,
        [FromBody] UpdateCategoryCommand updateCategoryCommand)
    {
        updateCategoryCommand.Id = id;
        var result = await _mediator.Send(updateCategoryCommand);
        return Ok(_mapper.Map<CategoryDto>(result));
    }

    [HttpDelete("{id}")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    public async Task Delete(int id)
    {
        await _mediator.Send(new DeleteCategoryCommand { Id = id });
    }
}