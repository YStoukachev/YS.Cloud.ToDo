namespace YS.Azure.ToDo.Models.Requests
{
    public class AddFileRequestModel
    {
        public string TaskId { get; set; } = null!;

        public (MemoryStream FileContent, string FileExtension) File { get; set; } = (null, null)!;
    }
}