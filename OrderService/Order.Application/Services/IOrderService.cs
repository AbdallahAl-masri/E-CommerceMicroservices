using Order.Application.DTOs;

namespace Order.Application.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderDTO>> GetOrdersByUserIdAsync(Guid userId);
        Task<OrderDetailsDTO> GetOrderDetailsAsync(int orderId);
    }
}
