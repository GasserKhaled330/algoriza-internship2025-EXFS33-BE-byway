using ByWay.Api.Mappers;
using ByWay.Domain.DTOs;
using ByWay.Domain.Enums;
using ByWay.Domain.Interfaces.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace ByWay.Api.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class InstructorsController : ControllerBase
  {
    private readonly IInstructorService _instructorService;
    private readonly InstructorMapper _instructorMapper;
    private readonly ILogger<InstructorsController> _logger;
    private readonly ProblemDetailsFactory _problemDetailsFactory;
    // LoggerMessage delegates
    private static readonly Action<ILogger, int, Exception?> LogInstructorNotFound =
        LoggerMessage.Define<int>(LogLevel.Warning, new EventId(100, "InstructorNotFound"),
            "Instructor with id {Id} not found.");

    private static readonly Action<ILogger, int, Exception?> LogInstructorFetched =
        LoggerMessage.Define<int>(LogLevel.Information, new EventId(101, "InstructorFetched"),
            "Instructor with id {Id} fetched successfully.");

    private static readonly Action<ILogger, int, Exception?> LogInstructorCreated =
        LoggerMessage.Define<int>(LogLevel.Information, new EventId(102, "InstructorCreated"),
            "Instructor with id {Id} created successfully.");

    private static readonly Action<ILogger, int, Exception?> LogInstructorUpdated =
        LoggerMessage.Define<int>(LogLevel.Information, new EventId(103, "InstructorUpdated"),
            "Instructor with id {Id} updated successfully.");

    private static readonly Action<ILogger, int, Exception?> LogInstructorDeleted =
        LoggerMessage.Define<int>(LogLevel.Information, new EventId(104, "InstructorDeleted"),
            "Instructor with id {Id} deleted successfully.");

    private static readonly Action<ILogger, string, Exception?> LogError =
        LoggerMessage.Define<string>(LogLevel.Error, new EventId(200, "Error"), "Error: {Message}");

    private static readonly Action<ILogger, string, Exception?> LogDebug =
        LoggerMessage.Define<string>(LogLevel.Debug, new EventId(300, "Debug"), "Debug: {Message}");

    public InstructorsController(
        IInstructorService instructorService,
        InstructorMapper instructorMapper,
        ILogger<InstructorsController> logger, ProblemDetailsFactory problemDetailsFactory)
    {
      _instructorService = instructorService;
      _logger = logger;
      _problemDetailsFactory = problemDetailsFactory;
      _instructorMapper = instructorMapper;
    }

    // GET: api/<InstructorController>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<PagingResponseDto<InstructorDto>>> GetAll(
        [FromQuery] int pageIndex = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? name = null,
        [FromQuery] JobTitle? jobTitle = null
    )
    {
      var (instructors, totalCount) = await _instructorService.GetPagedInstructorsAsync(
          pageIndex, pageSize, name, jobTitle);

      var resultDtos = _instructorMapper.MapInstructors(instructors);
      var pagingResponse = new PagingResponseDto<InstructorDto>()
      {
        PageSize = pageSize,
        PageIndex = pageIndex,
        TotalCount = totalCount,
        Data = resultDtos
      };
      return Ok(pagingResponse);
    }

    // GET api/<InstructorController>/5
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(InstructorDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<ActionResult<InstructorDto>> Get(int id)
    {
      var instructor = await _instructorService.GetInstructorByIdAsync(id);
      if (instructor is null)
      {
        LogInstructorNotFound(_logger, id, null);
        var problemDetails = _problemDetailsFactory.CreateProblemDetails(
            HttpContext,
            StatusCodes.Status404NotFound,
            title: "Not Found",
            detail: $"Instructor with id {id} not found."
        );
        return NotFound(problemDetails);
      }

      LogInstructorFetched(_logger, id, null);
      var resultDto = _instructorMapper.MapInstructor(instructor);
      return Ok(resultDto);
    }

    [HttpGet("[action]")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult JobTitles()
    {
      Response.Headers.CacheControl = "public,max-age=86400";
      return Ok(_instructorService.GetInstructorJobTitles());
    }

    [HttpGet("count")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<int>> GetCoursesCount()
    {
      int count = await _instructorService.GetInstructorsCountAsync();
      return Ok(count);
    }

    // POST api/<InstructorController>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(InstructorDto))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
    public async Task<ActionResult<InstructorDto>> Post([FromForm] AddInstructorDto model)
    {
      if (!ModelState.IsValid)
      {
        LogDebug(_logger, "Validation error on instructor creation.", null);
        return ValidationProblem(ModelState);
      }

      try
      {
        var addedInstructor = await _instructorService.AddInstructorAsync(model);
        LogInstructorCreated(_logger, addedInstructor.Id, null);
        var resultDto = _instructorMapper.MapInstructor(addedInstructor);
        return CreatedAtAction(nameof(Get), new { id = addedInstructor.Id }, resultDto);
      }
      catch (Exception e) when (e is InvalidOperationException or IOException)
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


    // PUT api/<InstructorController>/5
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> Put(int id, [FromForm] UpdateInstructorDto model)
    {
      if (!ModelState.IsValid)
      {
        LogDebug(_logger, "Validation error on instructor update.", null);
        return ValidationProblem(ModelState);
      }

      try
      {
        await _instructorService.UpdateInstructorAsync(id, model);
        LogInstructorUpdated(_logger, id, null);
        return NoContent();
      }
      catch (KeyNotFoundException ex)
      {
        LogInstructorNotFound(_logger, id, ex);
        var problemDetails = _problemDetailsFactory.CreateProblemDetails(
            HttpContext,
            StatusCodes.Status404NotFound,
            title: "Not Found",
            detail: ex.Message
        );
        return NotFound(problemDetails);
      }
      catch (Exception e) when (e is InvalidOperationException or IOException) // Catch specific image/IO errors
      {
        LogError(_logger, e.Message, e);
        // Use ProblemDetailsFactory for standard 400 response
        var problemDetails = _problemDetailsFactory.CreateProblemDetails(
            HttpContext,
            StatusCodes.Status400BadRequest,
            title: "Processing Error",
            detail: e.Message
        );
        return BadRequest(problemDetails);
      }
    }

    // DELETE api/<InstructorController>/5
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> Delete(int id)
    {
      try
      {
        await _instructorService.RemoveInstructorAsync(id);
        LogInstructorDeleted(_logger, id, null);
        return NoContent();
      }
      catch (KeyNotFoundException ex)
      {
        LogInstructorNotFound(_logger, id, null);
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
        LogDebug(_logger, $"Attempted to delete instructor {id} with assigned courses.", null);
        var problemDetails = _problemDetailsFactory.CreateProblemDetails(
            HttpContext,
            StatusCodes.Status400BadRequest,
            title: "Delete Error",
            detail: ex.Message
        );
        return BadRequest(problemDetails);
      }
    }

  }
}
