using Microsoft.EntityFrameworkCore;
using Order.Application.Interfaces;
using Order.Domain.Entites;
using Order.Infrastructure.Data;
using SharedLibrary.Logs;
using SharedLibrary.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Order.Infrastructure.Repositories
{
    class OrderRepository(OrderDbContext context) : IOrder
    {
        public async Task<Response> CreateAsync(Orders entity)
        {
            try
            {
                // Prevent saving orders with no items
                if (entity.OrderItems == null || !entity.OrderItems.Any())
                {
                    return new Response { Status = false, Message = "Order must contain at least one product." };
                }
                var order = new Orders
                {
                    UserId = entity.UserId,
                    CreatedDate = DateTime.Now,
                    OrderItems = entity.OrderItems
                };

                await context.SaveChangesAsync();

                return new Response { Status = order.OrderId > 0, 
                    Message = order.OrderId > 0 ? 
                    "Order placed successfully" : "Error occured while placing order" };
            }
            catch (Exception ex)
            {
                // log Exception
                LogException.LogExceptions(ex);

                return new Response { Status = false, Message = "Error occurred while placing order" };

            }
        }

        public async Task<Response> DeleteAsync(Orders entity)
        {
            try
            {
                var order = await GetByIdAsync(entity.OrderId);
                if (order is null)
                    return new Response { Status = false, Message = "Order not found" };

                context.Orders.Remove(order);
                await context.SaveChangesAsync();
                return new Response { Status = true, Message = "Order deleted successfully" };
            }
            catch (Exception ex)
            {
                // log Exception
                LogException.LogExceptions(ex);

                return new Response { Status = false, Message = "Error occurred while deleting order" };

            }
        }

        public async Task<IEnumerable<Orders>> FindByAsync(Expression<Func<Orders, bool>> predicate)
        {
            try
            {
                var order = await context.Orders.Where(predicate).Include(o => o.OrderItems).ToListAsync();
                return order.Any() ? order : null!;
            }
            catch (Exception ex)
            {
                // log Exception
                LogException.LogExceptions(ex);

                throw new Exception("Error occurred while fetching order");
            }
        }

        public async Task<IEnumerable<Orders>> GetAllAsync()
        {
            try
            {
                var orders = await context.Orders.AsNoTracking().Include(o => o.OrderItems).ToListAsync();
                return orders is not null ? orders : null!;
            }
            catch (Exception ex)
            {
                // log Exception
                LogException.LogExceptions(ex);

                throw new Exception("Error occurred while fetching orders");
            }
        }

        public async Task<Orders> GetByIdAsync(int id)
        {
            try
            {
                var order = await context.Orders
                    .Include(o => o.OrderItems)
                    .FirstOrDefaultAsync(o => o.OrderId == id);

                return order ?? null!;
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error occurred while fetching order", ex);
            }
        }


        public async Task<IEnumerable<Orders>> GetOrdersAsync(Expression<Func<Orders, bool>> predicate)
        {
            try
            {
                var orders = await context.Orders.Where(predicate).ToListAsync();
                return orders is not null ? orders : null!;
            }
            catch (Exception ex)
            {
                // log Exception
                LogException.LogExceptions(ex);

                throw new Exception("");
            }
        }

        public async Task<Response> UpdateAsync(Orders entity)
        {
            try
            {
                var order = await GetByIdAsync(entity.OrderId);

                if (order is null)
                    return new Response { Status = false, Message = "Order not found" };

                context.Entry(order).CurrentValues.SetValues(entity);

                // Ensure CreatedDate is not updated unless necessary
                order.CreatedDate = entity.CreatedDate;

                // Remove OrderItems that are no longer in the updated order
                var itemsToRemove = order.OrderItems
                    .Where(existingItem => !entity.OrderItems.Any(updatedItem => updatedItem.OrderItemId == existingItem.OrderItemId))
                    .ToList();

                context.OrderItems.RemoveRange(itemsToRemove);

                // Add or Update OrderItems
                foreach (var updatedItem in entity.OrderItems)
                {
                    var existingItem = order.OrderItems.FirstOrDefault(item => item.OrderItemId == updatedItem.OrderItemId);

                    if (existingItem is null)
                    {
                        // New OrderItem
                        updatedItem.OrderId = order.OrderId; // Ensure FK is set
                        context.OrderItems.Add(updatedItem);
                    }
                    else
                    {
                        // Existing OrderItem - Update if values changed
                        if (existingItem.ProductId != updatedItem.ProductId || existingItem.Quantity != updatedItem.Quantity)
                        {
                            context.Entry(existingItem).CurrentValues.SetValues(updatedItem);
                        }
                    }
                }

                await context.SaveChangesAsync();
                return new Response { Status = true, Message = "Order updated successfully" };
            }
            catch (Exception ex)
            {
                // log Exception
                LogException.LogExceptions(ex);

                return new Response { Status = false, Message = "Error occurred while updating order" };

            }
        }
    }
}
