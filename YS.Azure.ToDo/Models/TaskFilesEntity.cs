namespace YS.Azure.ToDo.Models
{
    public class TaskFilesEntity : IdEntity
    {
        public string TaskId { get; set; } = null!;

        public string FileName { get; set; } = null!;

        public ToDoEntity Task { get; set; } = null!;
    }
}