using HttpDemo.Shared.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace HttpDemo.Server.Controllers;


public sealed class CategoryController : Controller
{
    private static readonly IDictionary<int, CategoryDto> CategoryData = new Dictionary<int, CategoryDto>
    {
        { 1, new CategoryDto(1, "test") },
        { 2, new CategoryDto(2, "test2") },
        { 3, new CategoryDto(3, "test3") },
        { 4, new CategoryDto(4, "test4") },
        { 5, new CategoryDto(5, "test5") },
    };

    private static int lastId = 6;

   
    public CategoryController(ILogger<CategoryController> logger) : base(logger)
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
        return Ok(CategoryData.Values);
    }

    [HttpGet("{id:int}")]
    public IActionResult GetById([FromRoute] int id)
    {
        if (!CategoryData.TryGetValue(id, out var category))
            return NotFound();

        return Ok(category);
    }
 
    [HttpPost]
    public IActionResult Create([FromBody] CreateCategoryDto category)
    {
        lastId++;
        var newCategory = new CategoryDto(lastId, category.Name);
        CategoryData.Add(newCategory.Id, newCategory);
        return Created($"/api/cinema/CategoryController/{lastId}", newCategory);
    }

    [HttpPut("{id:int}")]
    public IActionResult Update([FromRoute] int id, [FromBody] CategoryDto category)
    {
        if (!CategoryData.ContainsKey(id))
            return NotFound();

        CategoryData[id] = category;
        return Ok();
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete([FromRoute] int id)
    {
        if (!CategoryData.ContainsKey(id))
            return NotFound();

        CategoryData.Remove(id);
        return Ok();
    }
}