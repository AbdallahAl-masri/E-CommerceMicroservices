using Product.Application.DTOs;
using Product.Domain.Entities;
using SharedLibrary.Intefaces;


namespace Product.Application.Interfaces
{
    public interface IProduct : IGenericInterface<Products>
    {
        Task<List<ProductDTO>> GetProductsByIdsAsync(List<int> productIds);
    }
}
