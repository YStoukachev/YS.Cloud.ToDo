namespace YS.Azure.ToDo.Models.Requests
{
    public class ArchiveTaskRequestModel
    {
        public string TaskId { get; set; } = null!;

        public bool Archive { get; set; }
    }
}