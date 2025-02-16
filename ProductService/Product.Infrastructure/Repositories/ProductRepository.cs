using Microsoft.EntityFrameworkCore;
using Product.Application.Interfaces;
using Product.Domain.Entities;
using Product.Infrastructure.Data;
using SharedLibrary.Logs;
using SharedLibrary.Responses;
using System.Linq.Expressions;

namespace Product.Infrastructure.Repositories
{
    public class ProductRepository(ProductDbContext context) : IProduct
    {
        public async Task<Response> CreateAsync(Products entity)
        {
            try
            {
                // Check if product already exists
                var getProduct = await FindByAsync(x => x.Name == entity.Name);
                if (getProduct is not null)
                {
                    return new Response { Status = false, Message = "Product already exists" };
                }

                var createProduct = context.Products.Add(entity).Entity;
                await context.SaveChangesAsync();
                if (createProduct is not null && createProduct.ProductId > 0)
                {
                    return new Response { Status = true, Message = "Product added successfully" };
                }
                return new Response { Status = false, Message = $"Error occurred while adding new {entity.Name}" };
            }
            catch (Exception ex)
            {
                // Log exception
                LogException.LogExceptions(ex);
                return new Response { Status = false, Message = "Error occurred adding new product" };
            }
        }

        public async Task<Response> DeleteAsync(Products entity)
        {
            try
            {
                var product = await GetByIdAsync(entity.ProductId);
                if (product is null)
                {
                    return new Response { Status = false, Message = $"{entity.Name} not found" };
                }

                context.Products.Remove(product);
                await context.SaveChangesAsync();
                return new Response { Status = true, Message = $"{entity.Name} deleted successfully" };
            }
            catch (Exception ex)
            {
                // Log exception
                LogException.LogExceptions(ex);
                return new Response { Status = false, Message = "Error occurred adding new product" };
            }
        }

        public async Task<IEnumerable<Products>> GetAllAsync()
        {
            try
            {
                var products = await context.Products.AsNoTracking().ToListAsync();
                return products is not null ? products : null!;
            }
            catch (Exception ex)
            {
                // Log exception
                LogException.LogExceptions(ex);
                throw new Exception("Error occurred while fetching products");
            }
        }

        public async Task<Products> GetByIdAsync(int id)
        {
            try
            {
                var product = await context.Products.FindAsync(id);
                return product is not null? product : null!;
            }
            catch (Exception ex)
            {
                // Log exception
                LogException.LogExceptions(ex);

                throw new Exception("Error occurred while fetching product");
            }
        }

        public async Task<Products> FindByAsync(Expression<Func<Products, bool>> predicate)
        {
            var product = await context.Products.Where(predicate).FirstOrDefaultAsync();
            return product is not null ? product : null!;
        }

        public async Task<Response> UpdateAsync(Products entity)
        {
            try
            {
                var product = await GetByIdAsync(entity.ProductId);
                if (product is null)
                {
                    return new Response { Status = false, Message = $"{entity.Name} not found" };
                }
                context.Entry(product).State = EntityState.Detached;
                context.Update(entity);
                await context.SaveChangesAsync();
                return new Response { Status = true, Message = $"{entity.Name} updated successfully" };
            }
            catch (Exception ex)
            {
                // Log exception
                LogException.LogExceptions(ex);
                return new Response { Status = false, Message = "Error occurred adding new product" };
            }
        }
    }
}
