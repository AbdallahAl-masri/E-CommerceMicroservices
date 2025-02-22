using Product.Application.DTOs;

namespace Product.Application.Services
{
    public interface IProductService
    {
        Task<List<ProductDTO>> GetProductsByIdsAsync(List<int> productIds);
    }
}
