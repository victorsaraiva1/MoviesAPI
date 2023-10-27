using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using MoviesAPI.Data.Dtos;
using System.Xml.XPath;

[ApiController]
[Route("[controller]")]
public class MovieController : ControllerBase
{
    private readonly MovieService _movieService;

    public MovieController(MovieService movieService)
    {
        _movieService = movieService;
    }

    [HttpPost]
    public IActionResult CreateMovies([FromBody] CreateMovieDto movieDto)
    {
        var movie = _movieService.CreateMovie(movieDto);
        return CreatedAtAction(nameof(GetMovieById), new { id = movie.Id }, movie);
    }

    [HttpGet]
    public IActionResult GetMovies([FromQuery] int skip = 0, [FromQuery] int take = 50)
    {
        var movies = _movieService.GetMovies(skip, take);
        return Ok(movies);
    }

    [HttpGet("{id}")]
    public IActionResult GetMovieById(int id)
    {
        var movieSelected = _movieService.GetMovieById(id);
        if (movieSelected == null) return NotFound();
        return Ok(movieSelected);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateMovie(int id, [FromBody] UpdateMovieDto movieDto)
    {
        if (_movieService.UpdateMovie(id, movieDto))
        {
            return NoContent();
        }
        return NotFound();
    }

    [HttpPatch("{id}")]
    public IActionResult UpdatePartialMovie(int id, JsonPatchDocument<UpdateMovieDto> moviePatch)
    {
        var product = _movieService.UpdatePartialMovie(id, new UpdateMovieDto()); 

        if (product == null)
        {
            return NotFound(); // Produto não encontrado
        }

        moviePatch.ApplyTo(product, ModelState);

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        return Ok(product);
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteMovie(int id)
    {
        if (_movieService.DeleteMovie(id))
        {
            return NoContent();
        }
        return NotFound();
    }
}
