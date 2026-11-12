
using Business_Logic_Layer.DTO.WishlistDTO;
using Business_Logic_Layer.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation_Logic_Layer.Controllers
{
    /// <summary>
    /// Handles wishlist operations, including retrieving,
    /// creating, and deleting wishlist entries.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class WishlistController(IWishlistService _wishlistService) : ControllerBase
    {
        /// <summary>
        /// Retrieves the wishlist of the currently authenticated attendee.
        /// </summary>
        /// <returns>A list of wishlist entries associated with the current attendee.</returns>
        /// <remarks>
        /// Requires Attendee role authorization.
        /// </remarks>
        [HttpGet("MyWishlist")]
        [Authorize(Roles = "Attendee")]
        public async Task<ActionResult<List<ReadWishlistDTO>>> GetWishlist()
        {
            var wishlist = await _wishlistService.GetWishlist();
            return Ok(wishlist);
        }

        /// <summary>
        /// Adds an event to the currently authenticated attendee's wishlist.
        /// </summary>
        /// <param name="eventId">The unique identifier of the event to add to the wishlist.</param>
        /// <returns>The unique identifier of the newly created wishlist entry.</returns>
        /// <remarks>
        /// Requires Attendee role authorization.
        /// </remarks>
        [HttpPost]
        [Authorize(Roles = "Attendee")]
        public async Task<ActionResult<int>> CreateWishlist(int eventId)
        {
            var wishlistId = await _wishlistService.CreateWishlist(eventId);
            return Ok(wishlistId);
        }

        /// <summary>
        /// Removes an event from the wishlist.
        /// </summary>
        /// <param name="id">The unique identifier of the wishlist entry to delete.</param>
        /// <returns>True if the wishlist entry was deleted successfully; otherwise, false.</returns>
        /// <remarks>
        /// Requires Attendee role authorization.
        /// </remarks>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Attendee")]
        public async Task<ActionResult<bool>> DeleteWishlist(int id)
        {
            var result = await _wishlistService.DeleteWishlist(id);
            return Ok(result);
        }
    }
}
