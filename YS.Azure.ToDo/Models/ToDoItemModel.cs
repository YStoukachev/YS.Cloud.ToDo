namespace YS.Azure.ToDo.Models
{
    public class ToDoItemModel : IdEntity
    {
        public string? Title { get; set; }

        public string? Description { get; set; }

        public ToDoStatus Status { get; set; }

        public bool Important { get; set; }

        public bool Archived { get; set; }

        public DateTime? DueDate { get; set; }

        public IList<string>? FileNames { get; set; }
    }
}