namespace YS.Azure.ToDo.Models.Requests
{
    public class ArchiveTaskRequestModel
    {
        public Guid TaskId { get; set; }

        public bool Archive { get; set; }
    }
}