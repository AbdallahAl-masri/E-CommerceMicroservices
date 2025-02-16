using Product.Domain.Entities;

namespace Product.Application.DTOs.Conversions
{
    public static class ProductConversion
    {
        public static (ProductDTO?, IEnumerable<ProductDTO>?) ToDTO(Products product, IEnumerable<Products> products)
        {
            // return single
            if(product is not null || products is null)
            {
                return (
                    new ProductDTO
                    {
                        ProductId = product!.ProductId,
                        Name = product!.Name,
                        Price = product!.Price,
                        StockQuantity = product!.StockQuantity
                    }, null);
            }

            // return list
            if(product is null || products is not null)
            {
                return (null, products.Select(p => new ProductDTO
                {
                    ProductId = p.ProductId,
                    Name = p.Name,
                    Price = p.Price,
                    StockQuantity = p.StockQuantity
                }).ToList());
            }

            return (null, null);
        }

        public static Products ToEntity(this ProductDTO productDTO)
        {
            return new Products
            {
                ProductId = productDTO.ProductId,
                Name = productDTO.Name,
                Price = productDTO.Price,
                StockQuantity = productDTO.StockQuantity
            };
        }
    }
}
