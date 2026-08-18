using Business_Logic_Layer.DTO.ReviewDTO;
using Business_Logic_Layer.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation_Logic_Layer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewController(IReviewService _reviewService):ControllerBase    
    {
        [HttpPost]
        [Authorize(Roles = "Attendee")]
        public async Task<ActionResult> CreateReview([FromBody] CreateReviewDTO dto)
        {
            var review = await _reviewService.CreateReview(dto);
            return Ok(review);
        }
        [HttpGet("GetById")]
        [Authorize(Roles = "Attendee,Organizer,Admin")]
        public async Task<ActionResult> GetReviewById(int id)
        {
            var review = await _reviewService.GetReviewById(id);
            return Ok(review);
        }
        [HttpGet("MyReviews")]
        [Authorize(Roles = "Attendee")]
        public async Task<ActionResult> MyReviews()
        {
            var reviews = await _reviewService.MyReviews();
            return Ok(reviews);
        }
        [HttpGet("Event/{eventId:int}")]
        [AllowAnonymous]
        public async Task<ActionResult> GetAllReviewsByEventId(int eventId)
        {
            var reviews = await _reviewService.GetAllReviewsByEventId(eventId);
            return Ok(reviews);
        }
        [HttpPatch("UpdateReview")]
        [Authorize(Roles = "Attendee")]
        public async Task<ActionResult> UpdateReview(int id, [FromBody] UpdateReviewDTO dto)
        {
            var review = await _reviewService.UpdateReview(id, dto);
            return Ok(review);
        }
        [HttpDelete("DeleteReview")]
        [Authorize(Roles = "Attendee,Admin")]
        public async Task<ActionResult> DeleteReview(int id)
        {
            var result = await _reviewService.DeleteReview(id);
            return Ok(result);
        }

    }
}
