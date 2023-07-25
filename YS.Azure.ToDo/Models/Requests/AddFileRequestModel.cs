namespace YS.Azure.ToDo.Models.Requests
{
    public class AddFileRequestModel
    {
        public Guid TaskId { get; set; }

        public Stream FileContent { get; set; } = null!;
    }
}