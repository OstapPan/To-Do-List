using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TO_DO_List.Data;
using TO_DO_List.Models;
using TO_DO_List.Services.CategoryService;

namespace TO_DO_List.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICategoryService _categoryService;
    

        public CategoriesController(ApplicationDbContext context , ICategoryService categoryService)
        {
            _context = context;
            _categoryService = categoryService;
           
           
        }

        // GET: Categories
        public async Task<IActionResult> Index([FromQuery] int page = 1, [FromQuery] int pageSize = 16)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return View(await _categoryService.GetCategories(userId, page, pageSize));
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int id)
        {

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var category = await _categoryService.GetCategoryById(id, userId);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Categories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,UserId")] Category category)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (ModelState.IsValid)
            {
                category.UserId= userId;
                await _categoryService.CreateCategory(category);
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int id)
        {

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var category = await _categoryService.GetCategoryById(id , userId);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,UserId")] Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (ModelState.IsValid)
            {
                category.UserId = userId;
                await _categoryService.EditCategory(category);
               
                
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var category = await _categoryService.GetCategoryById(id, userId);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Categories == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Categories'  is null.");
            }
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            _categoryService.DeleteCategory(id, userId);
            
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return (_context.Categories?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
