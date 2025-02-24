using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Order.Application.DTOs;
using Order.Application.DTOs.Conversions;
using Order.Application.Interfaces;
using Order.Application.Services;
using SharedLibrary.Responses;
using System.Security.Claims;

namespace Order.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController(IOrder orderInterface, IOrderService orderService) : ControllerBase
    {

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetOrders()
        {
            var orders = await orderInterface.GetAllAsync();
            if(!orders.Any())
                return NotFound("No orders in the database");

            var (_, list) = OrderConversion.ToDTO(null!, orders);
            return list!.Any() ? Ok(list) : NotFound("No orders found");
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OrderDTO>> GetOrder(int id)
        {
            var order = await orderInterface.GetByIdAsync(id);
            if (order is null)
                return NotFound("Order not found");
            var (dto, _) = OrderConversion.ToDTO(order, null!);
            return dto != null ? Ok(dto) : NotFound("Order not found");
        }

        [HttpGet("details/{orderId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<OrderDetailsDTO>> GetOrderDetails(int orderId)
        {
            if (orderId <= 0)
                return BadRequest("Invalid order id");

            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            if (string.IsNullOrEmpty(token))
                return Unauthorized("Token is required");

            var orderDetails = await orderService.GetOrderDetailsAsync(orderId, token);
            return orderDetails != null ? Ok(orderDetails) : NotFound("Order not found");
        }

        [HttpGet("user-order")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetUserOrders()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Invalid Token");

            var orders = await orderInterface.FindByAsync(o => o.UserId == Guid.Parse(userId));

            var (_, list) = OrderConversion.ToDTO(null!, orders);

            return list!.Any() ? Ok(list) : NotFound("No orders found for the user");
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<Response>> CreateOrder(OrderDTO orderDTO)
        {
            // check if the model is valid
            if (!ModelState.IsValid)
            {
                return BadRequest("Incomplete data submitted");
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Invalid Token");

            // convert the DTO to entity
            var order = OrderConversion.ToEntity(orderDTO, Guid.Parse(userId));
            var response = await orderInterface.CreateAsync(order);
            return response.Status ? Ok(response.Message) : BadRequest(response.Message);

        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<Response>> UpdateOrder(OrderDTO orderDTO)
        {
            // check if the model is valid
            if (!ModelState.IsValid)
            {
                return BadRequest("Incomplete data submitted");
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Invalid Token");

            // convert the DTO to entity
            var order = OrderConversion.ToEntity(orderDTO, Guid.Parse(userId));
            var response = await orderInterface.UpdateAsync(order);
            return response.Status ? Ok(response.Message) : BadRequest(response.Message);
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<Response>> DeleteOrder(OrderDTO orderDTO)
        {
            // check if the model is valid
            if (!ModelState.IsValid)
            {
                return BadRequest("Incomplete data submitted");
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Invalid Token");

            // convert the DTO to entity
            var order = OrderConversion.ToEntity(orderDTO, Guid.Parse(userId));
            var response = await orderInterface.DeleteAsync(order);
            return response.Status ? Ok(response.Message) : BadRequest(response.Message);
        }
    }
}
