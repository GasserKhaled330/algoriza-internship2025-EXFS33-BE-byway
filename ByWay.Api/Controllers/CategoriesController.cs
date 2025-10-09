using ByWay.Api.Mappers;
using ByWay.Domain.DTOs.Category;
using ByWay.Domain.Interfaces.Service;
using Microsoft.AspNetCore.Mvc;

namespace ByWay.Api.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class CategoriesController : ControllerBase
  {
    private readonly ICategoryService _categoryService;
    private readonly CategoryMapper _categoryMapper;

    public CategoriesController(ICategoryService categoryService,
        CategoryMapper categoryMapper)
    {
      _categoryService = categoryService;
      _categoryMapper = categoryMapper;
    }

    // GET: api/<CategoriesController>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<CategoryDto>>> GetAll()
    {
      Response.Headers.CacheControl = "public,max-age=86400";
      var categories = await _categoryService.GetCategoriesAsync();
      var mappedCategories = _categoryMapper.MapCategories(categories);
      return Ok(mappedCategories);
    }

  }
}