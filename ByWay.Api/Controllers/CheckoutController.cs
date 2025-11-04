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
    private readonly IEmailService _emailService;
    public CheckoutController(ICheckoutService checkoutService, IEmailService emailService, ProblemDetailsFactory problemDetailsFactory)
    {
      _checkoutService = checkoutService;
      _emailService = emailService;
      _problemDetailsFactory = problemDetailsFactory;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    public async Task<IActionResult> CompleteCheckout()
    {
      var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
      var userName = User.FindFirstValue(ClaimTypes.Name);
      var email = User.FindFirstValue(ClaimTypes.Email);
      try
      {
        var message = """
          Thank you for your purchase!

          🎉 Your courses are now available in your dashboard. Best of luck on your learning journey.
          """;
        var subject = "Checkout confirmation";
        await _checkoutService.ProcessCheckOutAsync(userId);
        await _emailService.SendEmailAsync(userName, email, subject, message);
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
