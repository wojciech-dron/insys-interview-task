using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MovieLibrary.Core.Queries;
using MovieLibrary.Core.Queries.Filters;

namespace MovieLibrary.Api.Controllers;

[Route("api/v1/Movie/Filter")]
[ApiController]
public class MovieFilterController : ControllerBase
{
    private readonly IMovieQueries _movieQueries;

    public MovieFilterController(IMovieQueries movieQueries)
    {
        _movieQueries = movieQueries;
    }

    [HttpGet]
    public async Task<IActionResult> GetFiltered([FromQuery] MovieFilter filter)
    {
        var result = await _movieQueries.GetFilteredAsync(filter);
        return Ok(result);
    }
}