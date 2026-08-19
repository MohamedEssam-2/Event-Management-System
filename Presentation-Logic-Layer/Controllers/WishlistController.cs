using Business_Logic_Layer.DTO.WishlistDTO;
using Business_Logic_Layer.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation_Logic_Layer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WishlistController(IWishlistService _wishlistService ) :ControllerBase
    {
        [HttpGet]
        [Authorize(Roles = "Attendee")]
        public async Task<ActionResult<List<ReadWishlistDTO>>> GetWishlist()
        {
            var wishlist = await _wishlistService.GetWishlist();
            return Ok(wishlist);
        }
        [HttpPost]
        [Authorize(Roles = "Attendee")]
        public async Task<ActionResult<int>> CreateWishlist(int eventId)
        {
            var wishlistId = await _wishlistService.CreateWishlist(eventId);
            return Ok(wishlistId);
        }
        //[HttpGet("Event/{eventId}")]
        //[Authorize(Roles = "Admin,Organizer")]
        //public async Task<ActionResult<List<ReadWishlistDTO>>> GetWishlistByEventId(int eventId)
        //{
        //    var wishlist = await _wishlistService.GetWishlistByEventId(eventId);
        //    return Ok(wishlist);
        //}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Attendee")]
        public async Task<ActionResult<bool>> DeleteWishlist(int id)
        {
            var result = await _wishlistService.DeleteWishlist(id);
            return Ok(result);
        }
    }
}
