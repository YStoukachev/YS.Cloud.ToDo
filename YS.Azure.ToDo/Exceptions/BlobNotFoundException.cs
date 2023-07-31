namespace YS.Azure.ToDo.Exceptions
{
    public class BlobNotFoundException : Exception
    {
        public BlobNotFoundException()
        {
            
        }
        
        public BlobNotFoundException(string message) : base(message)
        {
            
        }

        public BlobNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
            
        }
    }
}