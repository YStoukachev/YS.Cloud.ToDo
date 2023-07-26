using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;
using YS.Azure.ToDo.Contracts.Services;

namespace YS.Azure.ToDo.Services
{
    public class FormParser : IFormParser
    {
        public async Task<TForm> ParseForm<TForm>(
            string boundary,
            Stream stream,
            CancellationToken cancellationToken = default)
            where TForm : class
        {
            var multipartReader = new MultipartReader(boundary, stream);
            var section = await multipartReader.ReadNextSectionAsync(cancellationToken);
            var result = Activator.CreateInstance<TForm>();
            var resultProperties = typeof(TForm)
                .GetProperties()
                .ToList();

            while (section != null)
            {
                if (ContentDispositionHeaderValue.TryParse(section.ContentDisposition, out var contentDisposition))
                {
                    var property = resultProperties.First(_ => _.Name == contentDisposition.Name);

                    if (contentDisposition.IsFileDisposition())
                    {
                        // File section
                        var fileContent = new MemoryStream();
                        var extension = contentDisposition
                            .Parameters
                            .Last()
                            .Value
                            .ToString()
                            .Split(".")
                            .Last();
                        extension = extension.Remove(extension.Length - 1);
                        
                        await section.Body.CopyToAsync(fileContent, cancellationToken);
                        fileContent.Position = 0;

                        property.SetValue(result, (fileContent, extension));
                    }
                    else if (contentDisposition.IsFormDisposition())
                    {
                        // Text field section
                        using (var streamReader = new StreamReader(section.Body))
                        {
                            var value = await streamReader.ReadToEndAsync();

                            property.SetValue(result, value);
                        }
                    }
                }

                section = await multipartReader.ReadNextSectionAsync(cancellationToken);
            }

            return result;
        }
    }
}