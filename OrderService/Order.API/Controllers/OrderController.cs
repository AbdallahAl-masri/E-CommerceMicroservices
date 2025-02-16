using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Order.Application.DTOs;
using Order.Application.DTOs.Conversions;
using Order.Application.Interfaces;
using Order.Application.Services;
using SharedLibrary.Responses;

namespace Order.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController(IOrder orderInterface, IOrderService orderService) : ControllerBase
    {

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetOrders()
        {
            var orders = await orderInterface.GetAllAsync();
            if(!orders.Any())
                return NotFound("No orders in the database");

            var (_, list) = OrderConversion.ToDTO(null!, orders);
            return list!.Any() ? Ok(list) : NotFound("No orders found");
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<OrderDTO>> GetOrder(int id)
        {
            var order = await orderInterface.GetByIdAsync(id);
            if (order is null)
                return NotFound("Order not found");
            var (dto, _) = OrderConversion.ToDTO(order, null!);
            return dto != null ? Ok(dto) : NotFound("Order not found");
        }

        [HttpGet("client/{clientId}")]
        public async Task<ActionResult<OrderDTO>> GetClientOrders(Guid clientId)
        {
            if (clientId == Guid.Empty)
                return BadRequest("Invalid client id");
            var orders = await orderInterface.GetOrdersAsync(o => o.UserId == clientId);
            return orders.Any() ? Ok(orders) : NotFound("No orders found for the client");
        }

        [HttpGet("details/{orderId}")]
        public async Task<ActionResult<OrderDetailsDTO>> GetOrderDetails(int orderId)
        {
            if (orderId <= 0)
                return BadRequest("Invalid order id");
            var orderDetails = await orderService.GetOrderDetailsAsync(orderId);
            return orderDetails != null ? Ok(orderDetails) : NotFound("Order not found");
        }

        [HttpPost]
        public async Task<ActionResult<Response>> CreateOrder(OrderDTO orderDTO)
        {
            // check if the model is valid
            if (!ModelState.IsValid)
            {
                return BadRequest("Incomplete data submitted");
            }

            // convert the DTO to entity
            var order = OrderConversion.ToEntity(orderDTO);
            var response = await orderInterface.CreateAsync(order);
            return response.Status ? Ok(response) : BadRequest(response);

        }

        [HttpPut]
        public async Task<ActionResult<Response>> UpdateOrder(OrderDTO orderDTO)
        {
            // check if the model is valid
            if (!ModelState.IsValid)
            {
                return BadRequest("Incomplete data submitted");
            }
            // convert the DTO to entity
            var order = OrderConversion.ToEntity(orderDTO);
            var response = await orderInterface.UpdateAsync(order);
            return response.Status ? Ok(response) : BadRequest(response);
        }

        [HttpDelete]
        public async Task<ActionResult<Response>> DeleteOrder(OrderDTO orderDTO)
        {
            // check if the model is valid
            if (!ModelState.IsValid)
            {
                return BadRequest("Incomplete data submitted");
            }
            // convert the DTO to entity
            var order = OrderConversion.ToEntity(orderDTO);
            var response = await orderInterface.DeleteAsync(order);
            return response.Status ? Ok(response) : BadRequest(response);
        }
    }
}
