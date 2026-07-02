using TO_DO_List.Models;
using Azure;

namespace TO_DO_List.Services.CategoryService
{
    public interface ICategoryService
    {
        public Task<PagedResult<Category>> GetCategories(string userID,int page, int pageSize);
        public Task<Category> GetCategoryById(int id, string userID);
        public Task EditCategory(Category task);
        public Task DeleteCategory(int id, string userID);
        public Task CreateCategory(Category task);
    }
}
