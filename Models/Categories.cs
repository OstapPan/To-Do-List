using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
namespace TO_DO_List.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public string? UserId { get; set; }

        public ICollection<TaskToDo> Tasks { get; set; } = new List<TaskToDo>();
        public ICollection<TaskRepetitiveAcions> TasksRepetetive { get; set; } = new List<TaskRepetitiveAcions>();

        public Category() { }
        public Category(Category category)
        {
            Id = category.Id;
            Name = category.Name;
            UserId = category.UserId;
            Tasks = category.Tasks.ToList();
            TasksRepetetive = category.TasksRepetetive.ToList();



        }

    }

}