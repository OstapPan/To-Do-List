using System.ComponentModel.DataAnnotations.Schema;
namespace TO_DO_List.Models
{
    public class TaskVeiwModel
    {
        public int Id { get; set; }
        public string ToDo { get; set; }
        public DateTime DoTillDate { get; set; }
        public string? Description { get; set; }
        public DateTime CereatedDate { get; set; } = DateTime.Now;
        public bool IsRepetitive { get; set; } = false;
        public int? HowOften { get; set; }
        [ForeignKey("IdCategories")]
        public Category? Category { get; set; }
        public int? IdCategories { get; set; }

        public string? UserID { get; set; }

        public TaskVeiwModel()
        {

        }
        public TaskVeiwModel(ITaskToDo task)
        {
            this.ToDo = task.ToDo;
            this.Description = task.Description;
            this.Id= task.Id;
            this.DoTillDate = task.DoTillDate;
            this.CereatedDate = task.CereatedDate;
            this.Description= task.Description;
            this.Category = task.Category;
            this.UserID = task.UserID;
        }
        public TaskVeiwModel(TaskRepetitiveAcions task)
        {
            this.ToDo = task.ToDo;
            this.Description = task.Description;
            this.Id = task.Id;
            this.DoTillDate = task.DoTillDate;
            this.CereatedDate = task.CereatedDate;
            this.Description = task.Description;
            this.Category = task.Category;
            this.IsRepetitive = task.IsRepetitive;
            this.HowOften = task.HowOften;
            this.UserID = task.UserID;
        }


    }
}
