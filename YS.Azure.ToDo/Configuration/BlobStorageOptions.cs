namespace YS.Azure.ToDo.Configuration
{
    public class BlobStorageOptions
    {
        public string ConnectionString { get; set; } = null!;

        public string BlobContainerName { get; set; } = null!;
    }
}