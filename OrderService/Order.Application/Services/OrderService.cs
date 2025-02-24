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
        private async Task<List<ProductDTO>?> GetProductAsync(List<int> productIds)
        {
            var response = await httpClient.PostAsJsonAsync("api/product/batch", productIds);
            return response.IsSuccessStatusCode
                ? await response.Content.ReadFromJsonAsync<List<ProductDTO>>()
                : new List<ProductDTO>();
        }

        // Get User
        private async Task<UserDTO> GetUserAsync(Guid userId, string token)
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
            // Prepare order
            var order = await orderInterface.GetByIdAsync(orderId);
            if (order is null || order.OrderId <= 0)
                return null!;

            // Get retry pipeline
            var retryPipeline = resiliencePipeline.GetPipeline("my-retry-pipeline");

            // Prepare user details
            var userDTO = await retryPipeline.ExecuteAsync(async token => await GetUserAsync(order.UserId, JWTToken));

            // Get product IDs from order items
            var productIds = order.OrderItems.Select(item => item.ProductId).ToList();

            // Fetch product details in one request
            var products = await retryPipeline.ExecuteAsync(async token => await GetProductAsync(productIds));

            // Map products to order details
            var productDetails = order.OrderItems.Select(item =>
            {
                var productDTO = products!.FirstOrDefault(p => p.ProductId == item.ProductId);
                return new ProductDTO
                {
                    ProductId = item.ProductId,
                    Name = productDTO?.Name ?? "Unknown",
                    Price = productDTO?.Price ?? 0,
                    StockQuantity = item.Quantity
                };
            }).ToList();

            var totalAmount = productDetails.Sum(p => p.TotalPrice);

            // Populate order details
            var orderDetails = new OrderDetailsDTO
            {
                OrderId = order.OrderId,
                User = userDTO,
                CreatedDate = order.CreatedDate,
                Products = productDetails,
                TotalAmount = totalAmount
            };

            return orderDetails;
        }
    }
}
