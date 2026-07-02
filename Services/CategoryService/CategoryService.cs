using Microsoft.EntityFrameworkCore;
using TO_DO_List.Data;
using TO_DO_List.Models;

namespace TO_DO_List.Services.CategoryService
{
    public class CategoryService:ICategoryService
    {
        private readonly ApplicationDbContext _context;

        public CategoryService(ApplicationDbContext contex)
        {
            _context = contex;
        }

        public async Task CreateCategory(Category task)
        {
            

            _context.Categories.Add(task);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCategory(int id, string userID)
        {
            var todelete = await _context.Categories
             .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userID);

            _context.Categories.Remove(todelete);

            await  _context.SaveChangesAsync();
        }

        public async Task EditCategory(Category task)
        {
            _context.Categories.Update(task);
            await _context.SaveChangesAsync();
        }

        public async Task<PagedResult<Category>> GetCategories(string userID, int page, int pageSize)
        {
            var query = _context.Categories
                .Where(c => c.UserId == userID);

            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Category>
            {
                Items = items,
                TotalCount = totalCount
            };
        }
       

        public async Task<Category> GetCategoryById(int id, string userID)
        {
            return await _context.Categories.Where(x => x.UserId == userID).FirstOrDefaultAsync(x => x.Id == id);

        }
    }
}
