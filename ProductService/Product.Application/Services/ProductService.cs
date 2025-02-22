using Product.Application.DTOs;
using Product.Application.Interfaces;

namespace Product.Application.Services
{
    public class ProductService(IProduct productInterface) : IProductService
    {
        public async Task<List<ProductDTO>> GetProductsByIdsAsync(List<int> productIds)
        {
            var products = await productInterface.GetProductsByIdsAsync(productIds);

            return products.Any() ? products : null!;
        }
    }
}
