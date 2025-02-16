using Order.Domain.Entites;

namespace Order.Application.DTOs.Conversions
{
    public static class OrderConversion
    {
        public static (OrderDTO?, IEnumerable<OrderDTO>?) ToDTO(Orders order, IEnumerable<Orders> orders)
        {

            // return single
            if (order is not null || orders is null)
            {
                return (
                    new OrderDTO
                    {
                        OrderId = order!.OrderId,
                        ProductId = order!.ProductId,
                        UserId = order!.UserId,
                        Quantity = order!.Quantity,
                        CreatedDate = order!.CreatedDate
                    }, null);
            }

            // return list
            if(order is null || orders is not null)
            {
                return (null, orders.Select(o => new OrderDTO
                {
                    OrderId = o.OrderId,
                    ProductId = o.ProductId,
                    UserId = o.UserId,
                    Quantity = o.Quantity,
                    CreatedDate = o.CreatedDate
                }).ToList());
            }

            return (null, null);
        }

        public static Orders ToEntity(this OrderDTO orderDTO)
        {
            return new Orders
            {
                OrderId = orderDTO.OrderId,
                ProductId = orderDTO.ProductId,
                UserId = orderDTO.UserId,
                Quantity = orderDTO.Quantity,
                CreatedDate = orderDTO.CreatedDate
            };
        }
    }
}
