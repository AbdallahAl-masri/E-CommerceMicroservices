using Order.Application.DTOs;
using Order.Application.DTOs.Conversions;
using Order.Application.Interfaces;
using Polly.Registry;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Order.Application.Services
{
    public class OrderService(IOrder orderInterface,HttpClient httpClient,
        ResiliencePipelineProvider<string> resiliencePipeline) : IOrderService
    {

        // Get Product
        public async Task<ProductDTO> GetProductAsync(int productId)
        {
            // Call product API using HttpClient
            var getProduct = await httpClient.GetAsync($"api/product/{productId}");
            if (!getProduct.IsSuccessStatusCode)
                return null!;

            var product = await getProduct.Content.ReadFromJsonAsync<ProductDTO>();
            return product!;
        }

        // Get User
        public async Task<UserDTO> GetUserAsync(Guid userId, string token)
        {
            // Set the Authorization header with Bearer Token
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Call user API using HttpClient
            var getUser = await httpClient.GetAsync($"api/authentication");
            if (!getUser.IsSuccessStatusCode)
                return null!;
            var user = await getUser.Content.ReadFromJsonAsync<UserDTO>();
            return user!;
        }

        // Get Order Details by Id
        public async Task<OrderDetailsDTO> GetOrderDetailsAsync(int orderId, string JWTToken)
        {
            // prepare order
            var order = await orderInterface.GetByIdAsync(orderId);
            if(order is null || order.OrderId <= 0)
                return null!;

            // Get retry pipline
            var retryPipeline = resiliencePipeline.GetPipeline("my-retry-pipeline");

            // prepare product
            var productDTO = await retryPipeline.ExecuteAsync(async token => await GetProductAsync(order.ProductId));

            // prepare user
            var userDTO = await retryPipeline.ExecuteAsync(async token => await GetUserAsync(order.UserId, JWTToken));

            // populate order details
            var orderDetails = new OrderDetailsDTO
            {
                OrderId = order.OrderId,
                ProductId = order.ProductId,
                UserId = order.UserId,
                Name = userDTO.Name,
                Email = userDTO.Email,
                Address = userDTO.Address,
                MobileNumber = userDTO.MobileNumber,
                ProductName = productDTO.Name,
                UnitPrice = productDTO.Price,
                TotalPrice = productDTO.Price * order.Quantity,
                Quantity = order.Quantity,
                CreatedDate = order.CreatedDate
            };

            return orderDetails;
        }

        // Get Orders by User Id
        public async Task<IEnumerable<OrderDTO>> GetOrdersByUserIdAsync(Guid userId)
        {
            // Get orders by user id
            var orders = await orderInterface.GetOrdersAsync(o => o.UserId == userId);
            if(!orders.Any())
                return null!;

            // convert from entity to DTO
            var (_, _ordersDTO) = OrderConversion.ToDTO(null!, orders);

            return _ordersDTO!;
        }
    }
}
