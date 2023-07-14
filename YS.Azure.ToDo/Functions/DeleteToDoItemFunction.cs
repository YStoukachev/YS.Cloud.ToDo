using System.Net;
using System.Web;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using YS.Azure.ToDo.Contracts;
using YS.Azure.ToDo.Helpers;
using YS.Azure.ToDo.Models;

namespace YS.Azure.ToDo.Functions
{
    public class DeleteToDoItemFunction
    {
        private readonly ILogger<DeleteToDoItemFunction> _logger;
        private readonly IToDoService _toDoService;

        public DeleteToDoItemFunction(
            ILogger<DeleteToDoItemFunction> logger,
            IToDoService toDoService)
        {
            _logger = logger;
            _toDoService = toDoService;
        }

        [Function(nameof(DeleteToDoItemFunction))]
        public async Task<HttpResponseData> RunAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete")]
            HttpRequestData req,
            FunctionContext executionContext)
        {
            _logger.LogInformation("Got request for deleting TODO item.");

            var query = HttpUtility.ParseQueryString(req.Url.Query);
            var itemId = query[nameof(ToDoItem.Id)];

            if (!Guid.TryParse(itemId, out var parsedItemId))
            {
                _logger.LogWarning("Cannot parse item id.");

                return await req.CreateApiResponseAsync(HttpStatusCode.BadRequest, new ApiResponseMessage
                {
                    Error = "Cannot parse item id.",
                    OperationName = nameof(DeleteToDoItemFunction)
                });
            }

            if (parsedItemId == default)
            {
                _logger.LogInformation("Item id cannot be default value.");

                return await req.CreateApiResponseAsync(HttpStatusCode.BadRequest, new ApiResponseMessage
                {
                    Error = "Item id cannot be default value.",
                    OperationName = nameof(DeleteToDoItemFunction)
                });
            }

            await _toDoService.DeleteToDoItemAsync(parsedItemId);

            return await req.CreateApiResponseAsync(HttpStatusCode.OK, new ApiResponseMessage
            {
                OperationName = nameof(DeleteToDoItemFunction),
                Message = "Item successfully deleted."
            });
        }
    }
}