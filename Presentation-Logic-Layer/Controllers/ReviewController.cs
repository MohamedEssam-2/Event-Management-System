
using Business_Logic_Layer.DTO.ReviewDTO;
using Business_Logic_Layer.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation_Logic_Layer.Controllers
{
    /// <summary>
    /// Handles event review operations, including creation,
    /// retrieval, updating, and deletion of reviews.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewController(IReviewService _reviewService) : ControllerBase
    {
        /// <summary>
        /// Creates a new review for an event.
        /// </summary>
        /// <param name="dto">The review details, including the event and review content.</param>
        /// <returns>The newly created review information.</returns>
        /// <remarks>
        /// Requires Attendee role authorization.
        /// </remarks>
        [HttpPost]
        [Authorize(Roles = "Attendee")]
        public async Task<ActionResult> CreateReview([FromBody] CreateReviewDTO dto)
        {
            var review = await _reviewService.CreateReview(dto);
            return Ok(review);
        }

        /// <summary>
        /// Retrieves a specific review by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the review.</param>
        /// <returns>The requested review information.</returns>
        /// <remarks>
        /// Requires Attendee, Organizer, or Admin role authorization.
        /// </remarks>
        [HttpGet("GetById")]
        [Authorize(Roles = "Attendee,Organizer,Admin")]
        public async Task<ActionResult> GetReviewById(int id)
        {
            var review = await _reviewService.GetReviewById(id);
            return Ok(review);
        }

        /// <summary>
        /// Retrieves all reviews submitted by the currently authenticated attendee.
        /// </summary>
        /// <returns>A list of reviews associated with the current attendee.</returns>
        /// <remarks>
        /// Requires Attendee role authorization.
        /// </remarks>
        [HttpGet("MyReviews")]
        [Authorize(Roles = "Attendee")]
        public async Task<ActionResult> MyReviews()
        {
            var reviews = await _reviewService.MyReviews();
            return Ok(reviews);
        }

        /// <summary>
        /// Retrieves all reviews associated with a specific event.
        /// </summary>
        /// <param name="eventId">The unique identifier of the event.</param>
        /// <returns>A list of reviews submitted for the specified event.</returns>
        /// <remarks>
        /// This endpoint is publicly accessible.
        /// </remarks>
        [HttpGet("Event/{eventId:int}")]
        [AllowAnonymous]
        public async Task<ActionResult> GetAllReviewsByEventId(int eventId)
        {
            var reviews = await _reviewService.GetAllReviewsByEventId(eventId);
            return Ok(reviews);
        }

        /// <summary>
        /// Updates an existing review.
        /// </summary>
        /// <param name="id">The unique identifier of the review to update.</param>
        /// <param name="dto">The updated review information.</param>
        /// <returns>The updated review information.</returns>
        /// <remarks>
        /// Requires Attendee role authorization.
        /// </remarks>
        [HttpPatch("UpdateReview")]
        [Authorize(Roles = "Attendee")]
        public async Task<ActionResult> UpdateReview(int id, [FromBody] UpdateReviewDTO dto)
        {
            var review = await _reviewService.UpdateReview(id, dto);
            return Ok(review);
        }

        /// <summary>
        /// Deletes an existing review.
        /// </summary>
        /// <param name="id">The unique identifier of the review to delete.</param>
        /// <returns>The result of the review deletion operation.</returns>
        /// <remarks>
        /// Requires Attendee or Admin role authorization.
        /// </remarks>
        [HttpDelete("DeleteReview")]
        [Authorize(Roles = "Attendee,Admin")]
        public async Task<ActionResult> DeleteReview(int id)
        {
            var result = await _reviewService.DeleteReview(id);
            return Ok(result);
        }
    }
}
