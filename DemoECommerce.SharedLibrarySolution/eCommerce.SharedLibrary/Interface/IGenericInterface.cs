

using System.Linq.Expressions;



namespace eCommerce.SharedLibrary.Interface
{
    public interface IGenericInterface<T> where T : class
    {
        Task<eCommerce.SharedLibrary.Response.Response> CreateAsync(T entity);
        Task<eCommerce.SharedLibrary.Response.Response> UpdateAsync(T entity);
        Task<eCommerce.SharedLibrary.Response.Response> DeleteAsync(T entity);

        Task<IEnumerable<T>> GetAllAsync();
        Task<T> FindByIdAsync(int id);
        Task<T> GetByAsync(Expression<Func<T, bool>> predicate);

    }
}
