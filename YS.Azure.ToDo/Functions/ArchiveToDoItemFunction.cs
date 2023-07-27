using System.Net;
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
    public class ArchiveToDoItemFunction
    {
        private readonly ILogger<ArchiveToDoItemFunction> _logger;
        private readonly IToDoService _toDoService;

        public ArchiveToDoItemFunction(
            ILogger<ArchiveToDoItemFunction> logger, 
            IToDoService toDoService)
        {
            _logger = logger;
            _toDoService = toDoService;
        }

        [Function(nameof(ArchiveToDoItemFunction))]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "task/archive")] HttpRequestData req,
            FunctionContext executionContext)
        {
            _logger.LogInformation("Got request for archiving task.");

            ArchiveTaskRequestModel model;
            
            using (var streamReader = new StreamReader(req.Body))
            {
                var stringContent = await streamReader.ReadToEndAsync();
                model = JsonConvert.DeserializeObject<ArchiveTaskRequestModel>(stringContent)!;
            }

            try
            {
                if (model.Archive)
                {
                    await _toDoService.ArchiveTaskAsync(model.TaskId);
                
                    _logger.LogInformation("Task successfully archived.");
                }
                else
                {
                    await _toDoService.UnarchiveTaskAsync(model.TaskId);
                
                    _logger.LogInformation("Task successfully unarchived.");
                }
            }
            catch (TaskNotFoundException e)
            {
                return await req.CreateApiResponseAsync(HttpStatusCode.BadRequest, new ApiResponseMessage
                {
                    OperationName = nameof(ArchiveToDoItemFunction),
                    Error = e.Message
                });
            }

            return await req.CreateApiResponseAsync(HttpStatusCode.OK, new ApiResponseMessage
            {
                OperationName = nameof(ArchiveToDoItemFunction),
                Message = model.Archive 
                    ? "Task successfully archived." 
                    : "Task successfully unarchived."
            });
        }
    }
}