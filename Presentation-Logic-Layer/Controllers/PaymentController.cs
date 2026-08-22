using Business_Logic_Layer.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation_Logic_Layer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController(IPaymentService _paymentService): ControllerBase
    {
        [HttpPost("CreateCheckoutSession/{orderId:int}")]
        [Authorize(Roles = "Admin,Attendee")]
        public async Task<IActionResult> CreateCheckoutSession(int orderId)
        {
            var result =await _paymentService.CreateCheckoutSession(orderId);
            return Ok(result);
        }

        [HttpPost("Webhook")]
        [AllowAnonymous]
        public async Task<IActionResult> Webhook()
        {
            try
            {
                using var reader = new StreamReader(Request.Body);
                var json = await reader.ReadToEndAsync();

                var stripeSignature = Request.Headers["Stripe-Signature"].ToString();

                await _paymentService.HandleWebhookAsync(
                    json,
                    stripeSignature);

                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine("========== STRIPE WEBHOOK ERROR ==========");
                Console.WriteLine(ex.ToString());
                Console.WriteLine("==========================================");
                return StatusCode(500, ex.ToString());
            }
        }
    }
}