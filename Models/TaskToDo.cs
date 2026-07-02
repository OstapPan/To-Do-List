using System.ComponentModel.DataAnnotations.Schema;
namespace TO_DO_List.Models
{
    public class TaskToDo : ITaskToDo
    {
        public int Id { get ; set; }
        public DateTime DoTillDate { get ; set ; }
        public string? Description { get; set ; }
        public DateTime CereatedDate { get; set; } = DateTime.Now;
        public string ToDo { get; set; }
        public int? IdCategories { get; set ; }
        [ForeignKey("IdCategories")]
        public Category? Category { get; set ; }
        public string UserID { get ; set ; }

        public TaskToDo()
        {

        }
        public TaskToDo(TaskVeiwModel task)
        {
            this.ToDo = task.ToDo;
            this.Description = task.Description;
            this.Id = task.Id;
            this.DoTillDate = task.DoTillDate;
            this.CereatedDate = task.CereatedDate;
            this.Description = task.Description;
            this.IdCategories = task.IdCategories;
            this.Category = task.Category;
            this.UserID = task.UserID;
        }



    }
}
