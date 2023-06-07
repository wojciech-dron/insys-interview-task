using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MovieLibrary.Core.Commands.Categories;
using MovieLibrary.Core.Models;
using MovieLibrary.Core.Queries;

namespace MovieLibrary.Api.Controllers;

[Route("api/v1/CategoryManagement")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly CategoryQueries _queries;

    public CategoryController(IMediator mediator, CategoryQueries queries)
    {
        _mediator = mediator;
        _queries = queries;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetList()
    {
        var result = await _queries.GetListAsync();
        return Ok(result);
    }

    [HttpGet("{id}")]
    public string Get(int id)
    {
        return "value";
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateCategory.Command command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtRoute("", new {id = result.Id}, result.MapToDto());
    }

    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {
    }

    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
}