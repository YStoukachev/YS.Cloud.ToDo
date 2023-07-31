using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using YS.Azure.ToDo.Contracts.Services;
using YS.Azure.ToDo.Exceptions;
using YS.Azure.ToDo.Helpers;
using YS.Azure.ToDo.Models.Requests;
using YS.Azure.ToDo.Models.Responses;

namespace YS.Azure.ToDo.Functions
{
    public class UploadFileToToDoItemFunction
    {
        private readonly ILogger<UploadFileToToDoItemFunction> _logger;
        private readonly IFormParser _formParser;
        private readonly IToDoService _toDoService;

        public UploadFileToToDoItemFunction(
            ILogger<UploadFileToToDoItemFunction> logger, 
            IFormParser formParser, 
            IToDoService toDoService)
        {
            _logger = logger;
            _formParser = formParser;
            _toDoService = toDoService;
        }

        [Function(nameof(UploadFileToToDoItemFunction))]
        public async Task<HttpResponseData> RunAsync(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "task/content/upload")] HttpRequestData req)
        {
            _logger.LogInformation("Got request for adding file to TODO item.");

            var boundary = GetBoundaryFromContentType(
                GetHeaderValue("Content-Type", req.Headers));
            var model = await _formParser.ParseForm<AddFileRequestModel>(boundary, req.Body);
            
            _logger.LogInformation("Successfully parsed form.");
            _logger.LogInformation("Uploading file to blob storage.");

            try
            {
                var createdFileName = await _toDoService.UploadFileToTaskAsync(model);

                _logger.LogInformation("File successfully uploaded.");

                return await req.CreateApiResponseAsync(HttpStatusCode.OK, new ApiResponseMessage
                {
                    Message = "File successfully uploaded",
                    OperationName = nameof(UploadFileToToDoItemFunction),
                    Response = new
                    {
                        FileName = createdFileName
                    }
                });
            }
            catch (TaskNotFoundException e)
            {
                _logger.LogWarning(e.Message);

                return await req.CreateApiResponseAsync(HttpStatusCode.BadRequest, new ApiResponseMessage
                {
                    Error = e.Message,
                    OperationName = nameof(UploadFileToToDoItemFunction)
                });
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                
                return await req.CreateApiResponseAsync(HttpStatusCode.BadRequest, new ApiResponseMessage
                {
                    Error = e.Message,
                    OperationName = nameof(UploadFileToToDoItemFunction)
                });
            }
        }
        
        private static string GetBoundaryFromContentType(string contentType)
        {
            var boundaryStart = contentType.IndexOf("boundary=", StringComparison.OrdinalIgnoreCase);
            
            if (boundaryStart == -1)
                return null!;

            return contentType.Substring(boundaryStart + "boundary=".Length).Trim('"');
        }

        private static string GetHeaderValue(string headerName, HttpHeadersCollection headers)
        {
            if (headers.TryGetValues(headerName, out var values))
            {
                return values.First();
            }

            return string.Empty;
        }
    }
}