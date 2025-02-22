using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Product.Application.DTOs;
using Product.Application.DTOs.Conversions;
using Product.Application.Interfaces;
using Product.Application.Services;

namespace Product.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class ProductController(IProduct _product, IProductService productService) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetAll()
        {
            // get all products
            var products = await _product.GetAllAsync();

            if (!products.Any())
            {
                return NotFound("No products are found");
            }

            // convert the products entity to DTO
            var (_, list) = ProductConversion.ToDTO(null!, products);

            return list!.Any() ? Ok(list) : NotFound("No products are found");
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductDTO>> GetById(int id)
        {
            // get the product by id
            var product = await _product.GetByIdAsync(id);
            if (product is null)
            {
                return NotFound("Product not found");
            }

            // convert the product entity to DTO
            var (dto, _) = ProductConversion.ToDTO(product, null!);
            return dto != null ? Ok(dto) : NotFound("Product not found");
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ProductDTO>> Create(ProductDTO productDTO)
        {
            // check if the model is valid
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // convert the DTO to entity
            var product = productDTO.ToEntity();
            var response = await _product.CreateAsync(product);
            return response.Status ? Ok(product) : BadRequest(response.Message);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ProductDTO>> Update(ProductDTO productDTO)
        {
            // check if the model is valid
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // convert the DTO to entity
            var product = productDTO.ToEntity();
            var response = await _product.UpdateAsync(product);
            return response.Status ? Ok(productDTO) : BadRequest(response.Message);
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ProductDTO>> Delete(ProductDTO product)
        {
            var getEntity = product.ToEntity();
            var response = await _product.DeleteAsync(getEntity);
            return response.Status ? NoContent() : BadRequest(response.Message);
        }

        [HttpPost("batch")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<ProductDTO>>> GetProductsByIds([FromBody] List<int> productIds)
        {
            if (productIds == null || !productIds.Any())
                return BadRequest("Product ID list cannot be empty.");

            var products = await productService.GetProductsByIdsAsync(productIds);
            return products.Any() ? Ok(products) : NotFound("No products found for the given IDs.");
        }
    }
}
