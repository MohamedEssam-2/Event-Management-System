
using Business_Logic_Layer.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation_Logic_Layer.Controllers
{
    /// <summary>
    /// Handles payment operations, including checkout session creation
    /// and Stripe webhook processing.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController(
        IPaymentService _paymentService,
        ILogger<PaymentController> _logger) : ControllerBase
    {
        /// <summary>
        /// Creates a Stripe Checkout Session for a specific order.
        /// </summary>
        /// <param name="orderId">The unique identifier of the order to be paid.</param>
        /// <returns>The Stripe Checkout Session information required to proceed with payment.</returns>
        /// <remarks>
        /// Requires Admin or Attendee role authorization.
        /// The order must satisfy the payment requirements before a checkout session can be created.
        /// </remarks>
        [HttpPost("CreateCheckoutSession/{orderId:int}")]
        [Authorize(Roles = "Admin,Attendee")]
        public async Task<IActionResult> CreateCheckoutSession(int orderId)
        {
            var result = await _paymentService.CreateCheckoutSession(orderId);
            return Ok(result);
        }

        /// <summary>
        /// Receives and processes payment events sent by Stripe.
        /// </summary>
        /// <returns>An HTTP 200 response if the webhook is processed successfully.</returns>
        /// <remarks>
        /// This endpoint allows anonymous access because it is called directly by Stripe.
        /// The Stripe-Signature header is used to verify the authenticity of the webhook request.
        /// Returns HTTP 500 if an error occurs during webhook processing.
        /// </remarks>
        [HttpPost("Webhook")]
        [AllowAnonymous]
        public async Task<IActionResult> Webhook()
        {
            try
            {
                using var reader = new StreamReader(Request.Body);
                var json = await reader.ReadToEndAsync();
                var stripeSignature = Request.Headers["Stripe-Signature"].ToString();

                await _paymentService.HandleWebhookAsync(json, stripeSignature);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Stripe webhook processing failed.");
                return StatusCode(500, "An error occurred while processing the webhook.");
            }
        }
    }
}
