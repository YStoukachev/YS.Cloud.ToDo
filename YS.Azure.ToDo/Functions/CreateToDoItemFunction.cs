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
    public class CreateToDoItemFunction
    {
        private readonly IValidator<ToDoItem> _toDoItemValidator;
        private readonly IToDoService _toDoService;
        private readonly ILogger<CreateToDoItemFunction> _logger;

        public CreateToDoItemFunction(
            IValidator<ToDoItem> toDoItemValidator, 
            IToDoService toDoService, 
            ILogger<CreateToDoItemFunction> logger)
        {
            _toDoItemValidator = toDoItemValidator;
            _toDoService = toDoService;
            _logger = logger;
        }

        [Function(nameof(CreateToDoItemFunction))]
        public async Task<HttpResponseData> RunAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")]
            HttpRequestData req)
        {
            _logger.LogInformation("Got request for creating TODO item.");
            
            ToDoItem item;

            using (var streamReader = new StreamReader(req.Body))
            {
                var stringContent = await streamReader.ReadToEndAsync();
                item = JsonConvert.DeserializeObject<ToDoItem>(stringContent)!;
            }

            var validationResult = await _toDoItemValidator.ValidateAsync(item);
            
            _logger.LogInformation("TODO item validated.");

            if (!validationResult.IsValid)
            {
                _logger.LogWarning("TODO item is not valid.");
                
                return await req.CreateApiResponseAsync(HttpStatusCode.BadRequest, new ApiResponseMessage
                {
                    OperationName = nameof(CreateToDoItemFunction),
                    Error = "Input object is invalid"
                });
            }

            var createdItem = await _toDoService.CreateToDoItemAsync(item);
            
            _logger.LogInformation("TODO item successfully created.");

            return await req.CreateApiResponseAsync(HttpStatusCode.OK, new ApiResponseMessage
            {
                OperationName = nameof(CreateToDoItemFunction),
                Response = createdItem,
                Message = "Item successfully created."
            });
        }
    }
}