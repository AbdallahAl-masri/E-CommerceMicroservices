using Order.Application.DTOs;

namespace Order.Application.Services
{
    public interface IOrderService
    {
        Task<OrderDetailsDTO> GetOrderDetailsAsync(int orderId, string token);
    }
}
