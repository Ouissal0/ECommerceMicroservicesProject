using eCommerce.SharedLibrary.Response;
using ProductApi.Application.DTOs;

namespace ProductApi.Application.Services
{
    public interface ICategoryService
    {
       public Task<IEnumerable<CategoryDTO>> GetAllAsync();
       // Task<Response> GetByIdAsync(long id);
       Task<Response> AddAsync(CategoryDTO categoryDto);
      //  Task<Response> UpdateAsync(CategoryDTO categoryDto);
      //  Task<Response> DeleteAsync(long id);
    }
}
