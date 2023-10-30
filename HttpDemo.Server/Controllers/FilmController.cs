using HttpDemo.Shared.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace HttpDemo.Server.Controllers;


public sealed class FilmController : Controller
{
    private static readonly IDictionary<int, FilmDto> FilmData = new Dictionary<int, FilmDto>
    {
        {1, new FilmDto(1, "test", new string[]{"test1", "test2"}) },
        {2, new FilmDto(2, "test2", new string[]{"test1", "test2"}) },
    };

    private static int lastId = 6;

    public FilmController(ILogger<FilmController> logger) : base(logger)
    {
    }

    [HttpGet("Index")]
    public IActionResult Index()
    {
        var template = ControllerContext.ActionDescriptor.AttributeRouteInfo?.Template;
        return Content($"Index- template:{template}");
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(FilmData.Values);
    }

    [HttpGet("{id:int}")]
    public IActionResult GetById([FromRoute] int id)
    {
        if (!FilmData.TryGetValue(id, out var film))
            return NotFound();

        return Ok(film);
    }

    [HttpPost]
    public IActionResult Create([FromBody] CreateFilmDto film)
    {
        lastId++;
        var newFilm = new FilmDto(lastId, film.Name, film.Categories);
        FilmData.Add(newFilm.Id, newFilm);
        return Created($"/api/cinema/FilmController/{lastId}", newFilm);
    }

    [HttpPut("{id:int}")]
    public IActionResult Update([FromRoute] int id, [FromBody] FilmDto film)
    {
        if (!FilmData.ContainsKey(id))
            return NotFound();

        FilmData[id] = film;
        return Ok();
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete([FromRoute] int id)
    {
        if (!FilmData.ContainsKey(id))
            return NotFound();

        FilmData.Remove(id);
        return Ok();
    }
    [HttpGet("/api/cinema/Film/{categoryName}")]
    public IActionResult GetByCategory(string categoryName)
    {

        var films = FilmData.Values.Where(film => film.Categories.Contains(categoryName)).ToList();

        if (films == null || !films.Any())
        {
            return NotFound();
        }


        return Ok(films);
    }
}