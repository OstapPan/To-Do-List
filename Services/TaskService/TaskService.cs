using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TO_DO_List.Data;
using TO_DO_List.Models;

using TO_DO_List.Services.TaskServise;
namespace TO_DO_List.Services.TaskService
{
    public class TaskService : ITaskService
    {
        private readonly ApplicationDbContext _contex;

        public TaskService(ApplicationDbContext contex)
        {
            _contex = contex;
        }

        public async Task CreateTask(ITaskToDo task)
        {
            if (task is TaskRepetitiveAcions repTask && repTask.IsRepetitive)
            { 
                _contex.TaskRepetitives.Add(repTask);
                await _contex.SaveChangesAsync();
                return;
            }
            
            if (task is TaskToDo normaltask)
            {
                _contex.TaskToDo.Add(normaltask);
                await _contex.SaveChangesAsync();
            }
        }

        // ОПТИМІЗОВАНО: Видалення прямо в БД без викачування сутності в оперативну пам'ять
        public async Task DeleteTask(int id, string userID)
        {
            var deletedCount = await _contex.TaskToDo
                .Where(t => t.Id == id && t.UserID == userID)
                .ExecuteDeleteAsync();

            if (deletedCount == 0)
            {
                await _contex.TaskRepetitives
                    .Where(t => t.Id == id && t.UserID == userID)
                    .ExecuteDeleteAsync();
            }
        }

        public async Task EditTask(ITaskToDo task)
        {
            if (task is TaskRepetitiveAcions repTask && repTask.IsRepetitive)
            {
                _contex.TaskRepetitives.Update(repTask);
                await _contex.SaveChangesAsync();
                return;
            }
            
            if (task is TaskToDo normalTask)
            {
                _contex.TaskToDo.Update(normalTask);
                await _contex.SaveChangesAsync();
            }
        }

        // ОПТИМІЗОВАНО: Додано AsNoTracking для швидкодії читання
        public async Task<PagedResult<ITaskToDo>> GetTasks(string userID, int page, int pageSize)
        {
            var normalTasksQuery = _contex.TaskToDo
                .AsNoTracking() // <--- Вимикаємо трекінг для Read-Only запиту
                .Include(t => t.Category) 
                .Where(x => x.UserID == userID);

            var repetitiveTasksQuery = _contex.TaskRepetitives
                .AsNoTracking() // <--- Вимикаємо трекінг для Read-Only запиту
                .Include(t => t.Category) 
                .Where(x => x.UserID == userID);

            var totalCount = await normalTasksQuery.CountAsync() + await repetitiveTasksQuery.CountAsync();

            // УВАГА: Зараз ви берете по pageSize з КОЖНОЇ таблиці (разом може бути pageSize * 2).
            // Якщо це ок, то AsNoTracking зробить цей процес максимально швидким.
            var normalTasks = await normalTasksQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var repetitiveTasks = await repetitiveTasksQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var allTasks = normalTasks.Cast<ITaskToDo>()
                .Concat(repetitiveTasks.Cast<ITaskToDo>())
                .ToList();

            return new PagedResult<ITaskToDo>
            {
                Items = allTasks,
                TotalCount = totalCount
            };
        }

        // ОПТИМІЗОВАНО: Додано .Include() для категорій та .AsNoTracking()
        public async Task<ITaskToDo?> GetTasksById(int id, string userID)
        {
            var normalTask = await _contex.TaskToDo
                .AsNoTracking()
                .Include(t => t.Category) // <--- ПОВЕРТАЄМО КАТЕГОРІЮ (виправлено пропуск)
                .Where(x => x.UserID == userID)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (normalTask != null) return normalTask;

            return await _contex.TaskRepetitives
                .AsNoTracking()
                .Include(t => t.Category) // <--- ПОВЕРТАЄМО КАТЕГОРІЮ (виправлено пропуск)
                .Where(x => x.UserID == userID)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
       public async Task<IEnumerable<TaskVeiwModel>> GetTasksByCategoryIdAsync(int categoryId,string userID)
{
    // 1. Отримуємо звичайні таски
    var normaltasks = await _contex.TaskToDo
        .Where(t => t.IdCategories == categoryId && t.UserID == userID)
        .Select(t => new TaskVeiwModel
        {
            Id = t.Id,
            ToDo = t.ToDo,
            Description = t.Description,
            DoTillDate = t.DoTillDate,
            CereatedDate = t.CereatedDate,
            IdCategories = t.IdCategories,
            // Мапимо категорію безпечно, щоб уникнути циклічного JSON
            Category = t.Category != null ? new Category 
            { 
                Id = t.Category.Id, 
                Name = t.Category.Name 
            } : null,
            UserID = t.UserID,
            IsRepetitive = false, // Для звичайних тасок завжди false
            HowOften = 0
        })
        .ToListAsync(); // Асинхронний виклик бази

    // 2. Отримуємо таски, що повторюються
    var repetitivetasks = await _contex.TaskRepetitives
        .Where(t => t.IdCategories == categoryId && t.UserID == userID)
        .Select(t => new TaskVeiwModel
        {
            Id = t.Id,
            ToDo = t.ToDo,
            Description = t.Description,
            DoTillDate = t.DoTillDate,
            CereatedDate = t.CereatedDate,
            IdCategories = t.IdCategories,
            Category = t.Category != null ? new Category 
            { 
                Id = t.Category.Id, 
                Name = t.Category.Name 
            } : null,
            UserID = t.UserID,
            IsRepetitive = true, 
            HowOften = t.HowOften // Перевірте назву цього поля у вашій моделі TaskRepetitive
        })
        .ToListAsync(); // Асинхронний виклик бази

    var allTasks = normaltasks.Concat(repetitivetasks).ToList();   
    
    return allTasks;
}
        // Додайте цей метод у клас TaskService
public async Task<IEnumerable<TaskVeiwModel>> SearchTasksByTextAsync(string searchText, string userID)
{
    if (string.IsNullOrWhiteSpace(searchText))
    {
        return new List<TaskVeiwModel>();
    }

    var lowerSearchText = searchText.ToLower();

    // 1. Шукаємо у звичайних тасках
    var normalTasks = await _contex.TaskToDo
        .Where(t => t.UserID == userID && 
                   (t.ToDo.ToLower().Contains(lowerSearchText) || 
                   (t.Description != null && t.Description.ToLower().Contains(lowerSearchText))))
        .Select(t => new TaskVeiwModel
        {
            Id = t.Id,
            ToDo = t.ToDo,
            Description = t.Description,
            DoTillDate = t.DoTillDate,
            CereatedDate = t.CereatedDate,
            IdCategories = t.IdCategories,
            Category = t.Category != null ? new Category 
            { 
                Id = t.Category.Id, 
                Name = t.Category.Name 
            } : null,
            UserID = t.UserID,
            IsRepetitive = false,
            HowOften = 0
        })
        .ToListAsync();

    // 2. Шукаємо у тасках, що повторюються
    var repetitiveTasks = await _contex.TaskRepetitives
        .Where(t => t.UserID == userID && 
                   (t.ToDo.ToLower().Contains(lowerSearchText) || 
                   (t.Description != null && t.Description.ToLower().Contains(lowerSearchText))))
        .Select(t => new TaskVeiwModel
        {
            Id = t.Id,
            ToDo = t.ToDo,
            Description = t.Description,
            DoTillDate = t.DoTillDate,
            CereatedDate = t.CereatedDate,
            IdCategories = t.IdCategories,
            Category = t.Category != null ? new Category 
            { 
                Id = t.Category.Id, 
                Name = t.Category.Name 
            } : null,
            UserID = t.UserID,
            IsRepetitive = true,
            HowOften = t.HowOften
        })
        .ToListAsync();

    return normalTasks.Concat(repetitiveTasks).ToList();
}
        public async Task EnsureDefaultCategoriesAsync(string userID)
        {
            if (string.IsNullOrEmpty(userID)) return;

            var hasCategories = await _contex.Categories.AnyAsync(c => c.UserId == userID);

            if (!hasCategories)
            {
                var defaultCategories = new List<Category>
                {
                    new Category { Name = "Work", UserId = userID },
                    new Category { Name = "Home", UserId = userID }
                };

                _contex.Categories.AddRange(defaultCategories);
                await _contex.SaveChangesAsync();
            }
        }
    }
}