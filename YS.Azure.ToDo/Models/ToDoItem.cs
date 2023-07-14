namespace YS.Azure.ToDo.Models
{
    public class ToDoItem
    {
        public Guid Id { get; set; }

        public string? Title { get; set; }

        public string? Description { get; set; }

        public bool? Done { get; set; } = false;

        public bool? Important { get; set; } = false;

        public DateTime? DueDate { get; set; }
    }
}