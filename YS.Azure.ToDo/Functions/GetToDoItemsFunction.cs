using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using YS.Azure.ToDo.Contracts.Services;
using YS.Azure.ToDo.Helpers;
using YS.Azure.ToDo.Models;

namespace YS.Azure.ToDo.Functions
{
    public class GetToDoItemsFunction
    {
        private readonly ILogger<GetToDoItemsFunction> _logger;
        private readonly IToDoService _toDoService;

        public GetToDoItemsFunction(
            ILogger<GetToDoItemsFunction> logger, 
            IToDoService toDoService)
        {
            _logger = logger;
            _toDoService = toDoService;
        }

        [Function(nameof(GetToDoItemsFunction))]
        public async Task<HttpResponseData> RunAsync(
            [HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req,
            FunctionContext executionContext)
        {
            _logger.LogInformation("Got request for getting TODO items.");

            var items = (await _toDoService.GetToDosAsync()).ToList();

            if (items.Count == 0)
            {
                _logger.LogInformation("TODO items not found.");

                return await req.CreateApiResponseAsync(HttpStatusCode.NotFound, new ApiResponseMessage
                {
                    OperationName = nameof(GetToDoItemsFunction),
                    Response = Array.Empty<object>(),
                    Message = "Items not found"
                });
            }

            _logger.LogInformation("TODO items successfully got.");

            return await req.CreateApiResponseAsync(HttpStatusCode.OK, new ApiResponseMessage
            {
                OperationName = nameof(GetToDoItemsFunction),
                Response = items,
                Message = "Item successfully found."
            });
        }
    }
}