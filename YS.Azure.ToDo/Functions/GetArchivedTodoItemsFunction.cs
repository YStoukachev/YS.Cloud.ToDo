using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using YS.Azure.ToDo.Contracts.Services;
using YS.Azure.ToDo.Helpers;
using YS.Azure.ToDo.Models.Responses;

namespace YS.Azure.ToDo.Functions
{
    public class GetArchivedTodoItemsFunction
    {
        private readonly ILogger<GetArchivedTodoItemsFunction> _logger;
        private readonly IToDoService _toDoService;

        public GetArchivedTodoItemsFunction(
            ILogger<GetArchivedTodoItemsFunction> logger, 
            IToDoService toDoService)
        {
            _logger = logger;
            _toDoService = toDoService;
        }

        [Function(nameof(GetArchivedTodoItemsFunction))]
        public async Task<HttpResponseData> RunAsync(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "task/archive/get")] HttpRequestData req)
        {
           _logger.LogInformation("Got request for getting archived tasks.");

           var archivedTasks = await _toDoService.GetArchivedTasksAsync();

           if (archivedTasks.Count == 0)
           {
               return await req.CreateApiResponseAsync(HttpStatusCode.NotFound, new ApiResponseMessage
               {
                   Error = "Archived tasks not found.",
                   OperationName = nameof(GetArchivedTodoItemsFunction)
               });
           }

           return await req.CreateApiResponseAsync(HttpStatusCode.OK, new ApiResponseMessage
           {
               Message = "Archived tasks successfully found",
               OperationName = nameof(GetArchivedTodoItemsFunction),
               Response = archivedTasks
           });
        }
    }
}