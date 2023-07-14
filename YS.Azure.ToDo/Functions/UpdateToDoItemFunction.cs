using System.Net;
using FluentValidation;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using YS.Azure.ToDo.Contracts;
using YS.Azure.ToDo.Helpers;
using YS.Azure.ToDo.Models;

namespace YS.Azure.ToDo.Functions
{
    public class UpdateToDoItemFunction
    {
        private readonly ILogger<UpdateToDoItemFunction> _logger;
        private readonly IToDoService _toDoService;
        private readonly IValidator<ToDoItem> _toDoValidator;

        public UpdateToDoItemFunction(
            ILogger<UpdateToDoItemFunction> logger, 
            IToDoService toDoService, 
            IValidator<ToDoItem> toDoValidator)
        {
            _logger = logger;
            _toDoService = toDoService;
            _toDoValidator = toDoValidator;
        }

        [Function(nameof(UpdateToDoItemFunction))]
        public async Task<HttpResponseData> RunAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put")] HttpRequestData req)
        {
            _logger.LogInformation("Got request for updating TODO item.");
            
            ToDoItem item;

            using (var streamReader = new StreamReader(req.Body))
            {
                var stringContent = await streamReader.ReadToEndAsync();
                item = JsonConvert.DeserializeObject<ToDoItem>(stringContent)!;
            }

            var validationResult = await _toDoValidator.ValidateAsync(item);
            
            _logger.LogInformation("TODO item validated.");
            
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("TODO item is not valid.");
                
                return await req.CreateApiResponseAsync(HttpStatusCode.BadRequest, new ApiResponseMessage
                {
                    OperationName = nameof(UpdateToDoItemFunction),
                    Error = "Input object is invalid"
                });
            }
            
            var updatedItem = await _toDoService.UpdateToDoItemAsync(item.Id, item);
            
            _logger.LogInformation("TODO item successfully updated.");

            return await req.CreateApiResponseAsync(HttpStatusCode.OK, new ApiResponseMessage
            {
                OperationName = nameof(UpdateToDoItemFunction),
                Response = updatedItem,
                Message = "Item successfully updated."
            });
        }
    }
}