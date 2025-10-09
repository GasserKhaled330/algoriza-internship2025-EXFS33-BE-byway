using ByWay.Api.Mappers;
using ByWay.Application.Specifications.CourseSpecification;
using ByWay.Domain.Dtos.Course;
using ByWay.Domain.DTOs;
using ByWay.Domain.DTOs.Course;
using ByWay.Domain.Enums;
using ByWay.Domain.Interfaces.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace ByWay.Api.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class CoursesController : ControllerBase
  {
    private readonly ICourseService _courseService;
    private readonly CourseMapper _courseMapper;
    private readonly ILogger<CoursesController> _logger;
    private readonly ProblemDetailsFactory _problemDetailsFactory;

    // LoggerMessage delegates
    private static readonly Action<ILogger, int, Exception?> LogCourseNotFound =
        LoggerMessage.Define<int>(LogLevel.Warning, new EventId(100, "CourseNotFound"),
            "Course with id {Id} not found.");

    private static readonly Action<ILogger, int, Exception?> LogCourseFetched =
        LoggerMessage.Define<int>(LogLevel.Information, new EventId(101, "CourseFetched"),
            "Course with id {Id} fetched successfully.");

    private static readonly Action<ILogger, int, Exception?> LogCourseCreated =
        LoggerMessage.Define<int>(LogLevel.Information, new EventId(102, "CourseCreated"),
            "Course with id {Id} created successfully.");

    private static readonly Action<ILogger, int, Exception?> LogCourseUpdated =
        LoggerMessage.Define<int>(LogLevel.Information, new EventId(103, "CourseUpdated"),
            "Course with id {Id} updated successfully.");

    private static readonly Action<ILogger, int, Exception?> LogCourseDeleted =
        LoggerMessage.Define<int>(LogLevel.Information, new EventId(104, "CourseDeleted"),
            "Course with id {Id} deleted successfully.");

    private static readonly Action<ILogger, string, Exception?> LogError =
        LoggerMessage.Define<string>(LogLevel.Error, new EventId(200, "Error"), "Error: {Message}");

    private static readonly Action<ILogger, string, Exception?> LogDebug =
        LoggerMessage.Define<string>(LogLevel.Debug, new EventId(300, "Debug"), "Debug: {Message}");

    public CoursesController(
        ICourseService courseService,
        CourseMapper courseMapper,
        ILogger<CoursesController> logger, ProblemDetailsFactory problemDetailsFactory)
    {
      _courseService = courseService;
      _logger = logger;
      _problemDetailsFactory = problemDetailsFactory;
      _courseMapper = courseMapper;
    }

    // GET: api/<CourseController>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<PagingResponseDto<CourseDto>>> GetAll(
        [FromQuery] int pageIndex = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? sortBy = null,
        [FromQuery] string? name = null,
        [FromQuery] string? category = null,
        [FromQuery] double? rating = null,
        [FromQuery] decimal? cost = null,
        [FromQuery] int? minLecturesCount = null,
        [FromQuery] int? maxLecturesCount = null
    )
    {
      var (instructors, totalCount) = await _courseService.GetPagedCoursesAsync(
          pageIndex, pageSize, sortBy, name, category, rating, cost, minLecturesCount, maxLecturesCount);

      var resultDtos = _courseMapper.MapCourses(instructors);
      var pagingResponse = new PagingResponseDto<CourseDto>()
      {
        PageSize = pageSize,
        PageIndex = pageIndex,
        TotalCount = totalCount,
        Data = resultDtos
      };
      return Ok(pagingResponse);
    }

    // GET api/<CourseController>/5
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CourseDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<ActionResult<CourseDto>> Get(int id)
    {
      var course = await _courseService.GetCourseByIdAsync(id);
      if (course is null)
      {
        LogCourseNotFound(_logger, id, null);
        var problemDetails = _problemDetailsFactory.CreateProblemDetails(
            HttpContext,
            StatusCodes.Status404NotFound,
            title: "Not Found",
            detail: $"Course with id {id} not found."
        );
        return NotFound(problemDetails);
      }

      LogCourseFetched(_logger, id, null);
      var resultDto = _courseMapper.MapCourse(course);
      return Ok(resultDto);
    }

    [HttpGet("top-rated")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<CourseDto>>> GetTopRatedCourses([FromQuery] int count = 3)
    {
      var topRatedSpec = new TopRatedCoursesSpecification(count);
      var topRatedCourses = await _courseService.GetTopRatedCoursesAsync(topRatedSpec);
      var resultDto = _courseMapper.MapCourses(topRatedCourses);
      return Ok(resultDto);
    }

    [HttpGet("{courseId:int}/related/top-rated")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<CourseDto>>> GetTopRatedCourses(int courseId, [FromQuery] int categoryId)
    {
      var topRatedInSameCategorySpec = new TopRatedCoursesInSameCategorySpecification(categoryId, courseId);
      var topRatedInSameCategoryCourses = await _courseService.GetTopRatedCoursesInSameCategoryAsync(topRatedInSameCategorySpec);
      var resultDto = _courseMapper.MapCourses(topRatedInSameCategoryCourses);
      return Ok(resultDto);
    }

    [HttpGet("count")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<int>> GetCoursesCount()
    {
      int count = await _courseService.GetCoursesCountAsync();
      return Ok(count);
    }

    // POST api/<CourseController>
    [HttpPost]
    [Authorize(Roles = nameof(UserRole.Admin))]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CourseDto))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
    public async Task<ActionResult<CourseDto>> Post([FromForm] AddCourseDto model)
    {
      if (!ModelState.IsValid)
      {
        LogDebug(_logger, "Validation error on course creation.", null);
        return ValidationProblem(ModelState);
      }

      try
      {
        var addedCourse = await _courseService.AddCourseAsync(model);
        LogCourseCreated(_logger, addedCourse.Id, null);

        return CreatedAtAction(nameof(Get), new { id = addedCourse.Id }, null);
      }
      catch (InvalidOperationException e)
      {
        LogError(_logger, e.Message, e);
        var problemDetails = _problemDetailsFactory.CreateProblemDetails(
            HttpContext,
            StatusCodes.Status400BadRequest,
            title: "Processing Error",
            detail: e.Message
        );
        return BadRequest(problemDetails);
      }
    }

    // PUT api/<CourseController>/5
    [HttpPut("{id}")]
    [Authorize(Roles = nameof(UserRole.Admin))]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> Put(int id, [FromForm] UpdateCourseDto model)
    {
      if (!ModelState.IsValid)
      {
        LogDebug(_logger, "Validation error on course update.", null);
        return ValidationProblem(ModelState);
      }

      try
      {
        await _courseService.UpdateCourseAsync(id, model);
        LogCourseUpdated(_logger, id, null);
        return NoContent();
      }
      catch (KeyNotFoundException ex)
      {
        LogCourseNotFound(_logger, id, ex);
        var problemDetails = _problemDetailsFactory.CreateProblemDetails(
            HttpContext,
            StatusCodes.Status404NotFound,
            title: "Not Found",
            detail: ex.Message
        );
        return NotFound(problemDetails);
      }
      catch (ArgumentException e)
      {
        LogError(_logger, e.Message, e);
        var problemDetails = _problemDetailsFactory.CreateProblemDetails(
            HttpContext,
            StatusCodes.Status400BadRequest,
            title: "Processing Error",
            detail: e.Message
        );
        return BadRequest(problemDetails);
      }
    }

    // DELETE api/<CourseController>/5
    [HttpDelete("{id}")]
    [Authorize(Roles = nameof(UserRole.Admin))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> Delete(int id)
    {
      try
      {
        await _courseService.RemoveCourseAsync(id);
        LogCourseDeleted(_logger, id, null);
        return NoContent();
      }
      catch (KeyNotFoundException ex)
      {
        LogCourseNotFound(_logger, id, null);
        var problemDetails = _problemDetailsFactory.CreateProblemDetails(
            HttpContext,
            StatusCodes.Status404NotFound,
            title: "Not Found",
            detail: ex.Message
        );
        return NotFound(problemDetails);
      }
      catch (InvalidOperationException ex)
      {
        LogError(_logger, ex.Message, ex);
        var problemDetails = _problemDetailsFactory.CreateProblemDetails(
            HttpContext,
            StatusCodes.Status400BadRequest,
            title: "Delete Constraint Error",
            detail: ex.Message
        );
        return BadRequest(problemDetails);
      }
    }
  }
}