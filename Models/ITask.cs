namespace TO_DO_List.Models
{
    public interface ITaskToDo
    {
        public int Id { get; set; } 

        public string ToDo { get; set; }

        public DateTime DoTillDate { get; set; }
        public string? Description { get; set; }

        public DateTime CereatedDate { get;  set; }

        public Category? Category { get; set; }
        public int? IdCategories { get; set; }

        public string UserID { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    }

}
