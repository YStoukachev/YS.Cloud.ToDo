namespace YS.Azure.ToDo.Models
{
    public class ToDoItemModel
    {
        public Guid Id { get; set; }

        public string? Title { get; set; }

        public string? Description { get; set; }

        public ToDoStatus Status { get; set; }

        public bool Important { get; set; } = false;

        public DateTime? DueDate { get; set; }

        public IList<string>? FileNames { get; set; }
    }
}