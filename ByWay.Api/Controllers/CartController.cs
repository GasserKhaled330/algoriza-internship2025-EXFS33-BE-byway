using ByWay.Domain.Dtos.CartDtos;
using ByWay.Domain.Enums;
using ByWay.Domain.Interfaces.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ByWay.Api.Controllers
{
  [Route("api/[controller]")]
  [ApiController]

  public class CartController : ControllerBase
  {
    private readonly ICartService _cartService;

    public CartController(ICartService cartService)
    {
      _cartService = cartService;
    }

    // GET: api/<CartController>
    [HttpGet]
    [Authorize(Roles = nameof(UserRole.User))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<List<CartItemDto>>> GetCart()
    {
      var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

      var cart = await _cartService.GetCartAsync(userId);
      return Ok(cart);
    }

    [HttpGet("count")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<int>> GetCartItemsCount()
    {
      var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

      var cartItemsCount = await _cartService.GetCartItemsCount(userId);
      return Ok(cartItemsCount);
    }

    [HttpGet("IsInCart/{courseId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<bool>> IsItemInCart(int courseId)
    {
      var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

      var exists = await _cartService.IsInCart(userId, courseId);
      return Ok(exists);
    }


    // POST api/<CartController>/5
    [HttpPost("{courseId:int}")]
    [Authorize(Roles = nameof(UserRole.User))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Post(int courseId)
    {
      var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

      await _cartService.AddItemAsync(userId, courseId);
      return Created();
    }

    // DELETE api/<CartController>/5
    [HttpDelete("{courseId:int}")]
    [Authorize(Roles = nameof(UserRole.User))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(int courseId)
    {
      var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

      await _cartService.RemoveItemAsync(userId, courseId);
      return NoContent();
    }
  }
}
