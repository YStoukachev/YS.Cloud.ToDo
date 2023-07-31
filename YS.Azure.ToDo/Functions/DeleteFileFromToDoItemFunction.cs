using System.Net;
using System.Web;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using YS.Azure.ToDo.Contracts.Services;
using YS.Azure.ToDo.Exceptions;
using YS.Azure.ToDo.Helpers;
using YS.Azure.ToDo.Models.Requests;
using YS.Azure.ToDo.Models.Responses;

namespace YS.Azure.ToDo.Functions
{
    public class DeleteFileFromToDoItemFunction
    {
        private readonly ILogger<DeleteFileFromToDoItemFunction> _logger;
        private readonly IToDoService _toDoService;

        public DeleteFileFromToDoItemFunction(
            ILogger<DeleteFileFromToDoItemFunction> logger, 
            IToDoService toDoService)
        {
            _logger = logger;
            _toDoService = toDoService;
        }

        [Function(nameof(DeleteFileFromToDoItemFunction))]
        public async Task<HttpResponseData> RunAsync(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "task/content/delete")] HttpRequestData req)
        {
            _logger.LogInformation("Got request for deleting file from task.");

            DeleteFileRequestModel request;

            try
            {
                var query = HttpUtility.ParseQueryString(req.Url.Query);
                var fileName = query["fileName"];

                if (string.IsNullOrWhiteSpace(fileName))
                {
                    _logger.LogWarning("File name is null or empty string.");

                    return await req.CreateApiResponseAsync(HttpStatusCode.BadRequest, new ApiResponseMessage
                    {
                        OperationName = nameof(DeleteFileFromToDoItemFunction),
                        Error = "File name is null or empty string."
                    });
                }

                _logger.LogInformation("Deleting file from task.");

                await _toDoService.DeleteFileFromTaskAsync(fileName);

                _logger.LogInformation("File from task successfully deleted.");

                return await req.CreateApiResponseAsync(HttpStatusCode.OK, new ApiResponseMessage
                {
                    OperationName = nameof(DeleteFileFromToDoItemFunction),
                    Message = "File successfully deleted."
                });
            }
            catch (BlobNotFoundException e)
            {
                _logger.LogWarning(e.Message);

                return await req.CreateApiResponseAsync(HttpStatusCode.BadRequest, new ApiResponseMessage
                {
                    OperationName = nameof(DeleteFileFromToDoItemFunction),
                    Error = e.Message
                });
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                
                return await req.CreateApiResponseAsync(HttpStatusCode.BadRequest, new ApiResponseMessage
                {
                    OperationName = nameof(DeleteFileFromToDoItemFunction),
                    Error = e.Message
                });
            }
        }
    }
}