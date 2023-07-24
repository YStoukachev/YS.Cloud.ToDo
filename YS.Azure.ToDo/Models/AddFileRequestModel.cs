namespace YS.Azure.ToDo.Models
{
    public class AddFileRequestModel
    {
        public Guid TaskId { get; set; }

        public Stream FileContent { get; set; } = null!;
    }
}