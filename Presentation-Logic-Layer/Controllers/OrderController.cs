using Business_Logic_Layer.DTO.OrderDTO;
using Business_Logic_Layer.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation_Logic_Layer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController(IOrderService _orderService):ControllerBase
    {
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<ReadOrderDTO>>> GetAllOrders([FromQuery] int PageIndex = 1, [FromQuery] int PageSize = 5, [FromQuery] string? sortBy=null!)
        {
            var orders = await _orderService.GetAllOrders(PageIndex , PageSize,sortBy!);
            return Ok(orders);
        }
        [HttpGet("MyOrders")]
        [Authorize(Roles = "Attendee")]
        public async Task<ActionResult<List<ReadOrderDTO>>> GetMyOrders()
        {
            var orders = await _orderService.GetMyOrders();
            return Ok(orders);
        }
        [HttpGet("GetById")]
        [Authorize(Roles = "Attendee")]
        public async Task<ActionResult<ReadOrderDTO>> GetOrderById(int orderId)
        {
            var order = await _orderService.GetOrderById(orderId);
            return Ok(order);
        }

        [HttpPost]
        [Authorize(Roles = "Attendee")]
        public async Task<ActionResult<int>> CreateOrder(int EventId)
        {
            var orderId = await _orderService.CreateOrder(EventId);
            return Ok(orderId);
        }
        [HttpDelete]
        [Authorize(Roles = "Attendee")]
        public async Task<ActionResult<bool>> DeleteOrder(int OrderId)
        {
            var deleted = await _orderService.DeleteOrder(OrderId);
            return Ok(deleted);
        }
    }
}
