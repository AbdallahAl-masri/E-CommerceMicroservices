using Order.Domain.Entites;

namespace Order.Application.DTOs.Conversions
{
    public static class OrderConversion
    {
        public static (OrderDTO?, IEnumerable<OrderDTO>?) ToDTO(Orders order, IEnumerable<Orders> orders)
        {
            // Return single order DTO
            if (order is not null || orders is null)
            {
                return (
                    new OrderDTO
                    {
                        OrderId = order!.OrderId,
                        UserId = order!.UserId,
                        CreatedDate = order!.CreatedDate,
                        Products = order!.OrderItems.Select(oi => new OrderItemDTO
                        {
                            ProductId = oi.ProductId,
                            Quantity = oi.Quantity
                        }).ToList()
                    },
                    null
                );
            }

            // Return list of order DTOs
            if (order is null || orders is not null)
            {
                return (
                    null,
                    orders.Select(o => new OrderDTO
                    {
                        OrderId = o.OrderId,
                        UserId = o.UserId,
                        CreatedDate = o.CreatedDate,
                        Products = o.OrderItems.Select(oi => new OrderItemDTO
                        {
                            ProductId = oi.ProductId,
                            Quantity = oi.Quantity
                        }).ToList()
                    }).ToList()
                );
            }

            return (null, null);
        }


        public static Orders ToEntity(this OrderDTO orderDTO)
        {
            return new Orders
            {
                UserId = orderDTO.UserId,
                CreatedDate = orderDTO.CreatedDate,
                OrderItems = orderDTO.Products.Select(p => new OrderItem
                {
                    ProductId = p.ProductId,
                    Quantity = p.Quantity,
                }).ToList()
            };
        }

    }
}
