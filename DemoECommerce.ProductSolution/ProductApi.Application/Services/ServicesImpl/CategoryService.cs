using eCommerce.SharedLibrary.Logs;
using eCommerce.SharedLibrary.Response;
using ProductApi.Application.DTOs;
using ProductApi.Domain.Entities;
using ProductApi.Domain.Interfaces;

namespace ProductApi.Application.Services.ServicesImpl
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<CategoryDTO>> GetAllAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();
            return categories.Select(c => new CategoryDTO
            {
                Id = c.Id,
                Name = c.Name,
               
            });
        }
        /*
        public async Task<Response> GetByIdAsync(long id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
                return new Response(false, "Category not found.");

            return new Response(true, "Category retrieved successfully.")
            {
                Data = new CategoryDTO
                {
                    Id = category.Id,
                    Name = category.Name,
                   
                }
            };
        }
        */
        public async Task<Response> AddAsync(CategoryDTO categoryDto)
        {
            try
            {
                var category = new Category
                {
                    Name = categoryDto.Name,
                };
                await _categoryRepository.AddAsync(category);
                return new Response(true, "Category added successfully.");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, "Failed to add category.");
            }
        }

     


    
        /*
        public async Task<Response> UpdateAsync(CategoryDTO categoryDto)
        {
           try
           {
               var category = await _categoryRepository.GetByIdAsync(categoryDto.Id);
               if (category == null)
                   return new Response(false, "Category not found.");

               category.Name = categoryDto.Name;

               await _categoryRepository.UpdateAsync(category);

               return new Response(true, "Category updated successfully.");
           }
           catch (Exception ex)
           {
               LogException.LogExceptions(ex);
               return new Response(false, "Failed to update category.");
           }
        }

        public async Task<Response> DeleteAsync(long id)
        {
           try
           {
               var category = await _categoryRepository.GetByIdAsync(id);
               if (category == null)
                   return new Response(false, "Category not found.");

               await _categoryRepository.DeleteAsync(id);
               return new Response(true, "Category deleted successfully.");
           }
           catch (Exception ex)
           {
               LogException.LogExceptions(ex);
               return new Response(false, "Failed to delete category.");
           }
        }
*/


    }
}
