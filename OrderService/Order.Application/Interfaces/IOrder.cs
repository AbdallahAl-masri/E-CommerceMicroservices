using Order.Domain.Entites;
using SharedLibrary.Intefaces;
using System.Linq.Expressions;

namespace Order.Application.Interfaces
{
    public interface IOrder : IGenericInterface<Orders>
    {
        Task<IEnumerable<Orders>> GetOrdersAsync(Expression<Func<Orders, bool>> predicate);
    }
}
