namespace YS.Azure.ToDo.Configuration
{
    public class ToDoOptions
    {
        public string CosmosDbName { get; set; } = null!;

        public string CosmosContainerName { get; set; } = null!;

        public string BlobNameTemplate { get; set; } = null!; // {TodoId}/{FileId}{extension}
    }
}