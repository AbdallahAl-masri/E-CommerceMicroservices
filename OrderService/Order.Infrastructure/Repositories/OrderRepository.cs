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
                var order = context.Orders.Add(entity).Entity;
                await context.SaveChangesAsync();
                return order.OrderId > 0 ? new Response { Status = true, Message = "Order placed successfully" } :
                    new Response { Status = false, Message = "Error occurred while placing order" };
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

        public async Task<Orders> FindByAsync(Expression<Func<Orders, bool>> predicate)
        {
            try
            {
                var order = await context.Orders.Where(predicate).FirstOrDefaultAsync();
                return order is not null ? order : null!;
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
                var orders = await context.Orders.AsNoTracking().ToListAsync();
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
                var order = await context.Orders.FindAsync(id);
                return order is not null ? order : null!;
            }
            catch (Exception ex)
            {
                // log Exception
                LogException.LogExceptions(ex);

                throw new Exception("Error occurred while fetching order");
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

                context.Entry(order).State = EntityState.Detached;
                context.Orders.Update(entity);
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
