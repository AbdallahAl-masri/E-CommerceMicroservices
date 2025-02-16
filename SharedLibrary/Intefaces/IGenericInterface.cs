using SharedLibrary.Responses;
using System.Linq.Expressions;

namespace SharedLibrary.Intefaces
{
    public interface IGenericInterface<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task<T> FindByAsync(Expression<Func<T, bool>> predicate);
        Task<Response> CreateAsync(T entity);
        Task<Response> UpdateAsync(T entity);
        Task<Response> DeleteAsync(T entity);
        
    }
}
