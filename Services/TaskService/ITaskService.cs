using Microsoft.AspNetCore.Mvc;
using TO_DO_List.Data;
using TO_DO_List.Models;
using Azure;

namespace TO_DO_List.Services.TaskServise
{
    public interface ITaskService
    {
        public  Task<PagedResult<ITaskToDo>> GetTasks(string userID, int page, int pageSize);
        public  Task<ITaskToDo> GetTasksById(int id,string userID);
        public  Task EditTask(ITaskToDo task);
        public Task DeleteTask(int id, string userID);
        public Task CreateTask(ITaskToDo task);
        public Task EnsureDefaultCategoriesAsync(string userID);
        public Task<IEnumerable<TaskVeiwModel>> GetTasksByCategoryIdAsync(int categoryId, string userID);
        // Додайте до ITaskService
        public Task<IEnumerable<TaskVeiwModel>> SearchTasksByTextAsync(string searchText, string userID);
    }
}
