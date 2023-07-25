namespace YS.Azure.ToDo.Models.Responses
{
    public class ApiResponseMessage
    {
        public string? OperationName { get; set; }

        public string? Message { get; set; }

        public DateTime? ExecutionTime { get; set; }

        public object? Response { get; set; }

        public string? Error { get; set; }
    }
}