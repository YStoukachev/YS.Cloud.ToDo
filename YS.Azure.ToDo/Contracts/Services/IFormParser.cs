namespace YS.Azure.ToDo.Contracts.Services
{
    public interface IFormParser
    {
        Task<TForm> ParseForm<TForm>(string boundary, Stream stream, CancellationToken cancellationToken = default) 
            where TForm : class;
    }
}