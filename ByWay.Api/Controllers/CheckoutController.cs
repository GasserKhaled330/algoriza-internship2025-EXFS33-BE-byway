using ByWay.Application.Exceptions;
using ByWay.Domain.Enums;
using ByWay.Domain.Interfaces.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Security.Claims;

namespace ByWay.Api.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  [Authorize(Roles = nameof(UserRole.User))]
  public class CheckoutController : ControllerBase
  {
    private readonly ICheckoutService _checkoutService;
    private readonly ProblemDetailsFactory _problemDetailsFactory;
    public CheckoutController(ICheckoutService checkoutService, ProblemDetailsFactory problemDetailsFactory)
    {
      _checkoutService = checkoutService;
      _problemDetailsFactory = problemDetailsFactory;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    public async Task<IActionResult> CompleteCheckout()
    {
      var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
      try
      {
        await _checkoutService.ProcessCheckOutAsync(userId);
        return Created();
      }
      catch (CheckoutValidationException ex)
      {
        var problemDetails = _problemDetailsFactory.CreateProblemDetails(

          HttpContext,
          StatusCodes.Status400BadRequest,
          title: "Checkout Failed",
          detail: ex.Message
        );
        return BadRequest(problemDetails);
      }
    }
  }
}
