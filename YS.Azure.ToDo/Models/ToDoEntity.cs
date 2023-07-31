namespace YS.Azure.ToDo.Models
{
    public class ToDoEntity : IdEntity
    {
        public string? Title { get; set; }

        public string? Description { get; set; }

        public ToDoStatus Status { get; set; }

        public bool Important { get; set; }

        public bool Archived { get; set; }

        public DateTime? DueDate { get; set; }

        public ICollection<TaskFilesEntity> Files { get; set; } = null!;
    }
}