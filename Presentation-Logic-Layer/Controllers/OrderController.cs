
using Business_Logic_Layer.DTO.OrderDTO;
using Business_Logic_Layer.DTO.PaginationDTO;
using Business_Logic_Layer.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation_Logic_Layer.Controllers
{
    /// <summary>
    /// Handles order management operations, including retrieval,
    /// creation, cancellation, and deletion of orders.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController(IOrderService _orderService) : ControllerBase
    {
        /// <summary>
        /// Retrieves all orders with pagination and sorting support.
        /// </summary>
        /// <param name="PageIndex">The page number to retrieve. Defaults to 1.</param>
        /// <param name="PageSize">The number of orders per page. Defaults to 5.</param>
        /// <param name="sortBy">Optional field used to sort the orders.</param>
        /// <returns>A list of orders matching the specified pagination and sorting criteria.</returns>
        /// <remarks>
        /// Requires Admin role authorization.
        /// </remarks>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<PagedResultDTO<ReadOrderDTO>>> GetAllOrders(
            [FromQuery] int PageIndex = 1,
            [FromQuery] int PageSize = 5,
            [FromQuery] string? sortBy = null!)
        {
            var orders = await _orderService.GetAllOrders(PageIndex, PageSize, sortBy!);
            return Ok(orders);
        }

        /// <summary>
        /// Retrieves all orders belonging to the currently authenticated attendee.
        /// </summary>
        /// <returns>A list of orders associated with the current attendee.</returns>
        /// <remarks>
        /// Requires Attendee role authorization.
        /// </remarks>
        [HttpGet("MyOrders")]
        [Authorize(Roles = "Attendee")]
        public async Task<ActionResult<List<ReadOrderDTO>>> GetMyOrders()
        {
            var orders = await _orderService.GetMyOrders();
            return Ok(orders);
        }

        /// <summary>
        /// Retrieves a specific order by its unique identifier.
        /// </summary>
        /// <param name="orderId">The unique identifier of the order.</param>
        /// <returns>The requested order information.</returns>
        /// <remarks>
        /// Requires Attendee role authorization.
        /// </remarks>
        [HttpGet("GetById")]
        [Authorize(Roles = "Attendee")]
        public async Task<ActionResult<ReadOrderDTO>> GetOrderById(int orderId)
        {
            var order = await _orderService.GetOrderById(orderId);
            return Ok(order);
        }

        /// <summary>
        /// Retrieves all orders associated with a specific event,
        /// with pagination and sorting support.
        /// </summary>
        /// <param name="eventId">The unique identifier of the event.</param>
        /// <param name="PageIndex">The page number to retrieve. Defaults to 1.</param>
        /// <param name="PageSize">The number of orders per page. Defaults to 5.</param>
        /// <param name="sortBy">Optional field used to sort the orders.</param>
        /// <returns>A list of orders associated with the specified event.</returns>
        /// <remarks>
        /// Requires Admin role authorization.
        /// </remarks>
        [HttpGet("Event/{eventId:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<PagedResultDTO<ReadOrderDTO>>> GetOrdersByEventId(
            int eventId,
            [FromQuery] int PageIndex = 1,
            [FromQuery] int PageSize = 5,
            [FromQuery] string? sortBy = null!)
        {
            var orders = await _orderService.GetOrdersByEventId(eventId, PageIndex, PageSize, sortBy!);
            return Ok(orders);
        }

        /// <summary>
        /// Creates a new order for a specific event.
        /// </summary>
        /// <param name="EventId">The unique identifier of the event for which the order is created.</param>
        /// <returns>The unique identifier of the newly created order.</returns>
        /// <remarks>
        /// Requires Attendee role authorization.
        /// </remarks>
        [HttpPost]
        [Authorize(Roles = "Attendee")]
        public async Task<ActionResult<int>> CreateOrder(int EventId)
        {
            var orderId = await _orderService.CreateOrder(EventId);
            return Ok(orderId);
        }

        /// <summary>
        /// Cancels an existing order.
        /// </summary>
        /// <param name="OrderId">The unique identifier of the order to cancel.</param>
        /// <returns>The order information after cancellation.</returns>
        /// <remarks>
        /// Requires Attendee role authorization.
        /// </remarks>
        [HttpPatch("CancelOrder")]
        [Authorize(Roles = "Attendee")]
        public async Task<ActionResult<ReadOrderDTO>> CancelOrder(int OrderId)
        {
            var order = await _orderService.CancelOrder(OrderId);
            return Ok(order);
        }

        /// <summary>
        /// Deletes an existing order.
        /// </summary>
        /// <param name="OrderId">The unique identifier of the order to delete.</param>
        /// <returns>True if the order was deleted successfully; otherwise, false.</returns>
        /// <remarks>
        /// Requires Attendee role authorization.
        /// </remarks>
        [HttpDelete]
        [Authorize(Roles = "Attendee")]
        public async Task<ActionResult<bool>> DeleteOrder(int OrderId)
        {
            var deleted = await _orderService.DeleteOrder(OrderId);
            return Ok(deleted);
        }
    }
}
