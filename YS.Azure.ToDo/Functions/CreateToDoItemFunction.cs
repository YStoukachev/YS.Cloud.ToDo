using System.Net;
using FluentValidation;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using YS.Azure.ToDo.Contracts;
using YS.Azure.ToDo.Contracts.Services;
using YS.Azure.ToDo.Helpers;
using YS.Azure.ToDo.Models;
using YS.Azure.ToDo.Models.Responses;

namespace YS.Azure.ToDo.Functions
{
    public class CreateToDoItemFunction
    {
        private readonly IValidator<ToDoItemModel> _toDoItemValidator;
        private readonly IToDoService _toDoService;
        private readonly ILogger<CreateToDoItemFunction> _logger;
        public CreateToDoItemFunction(
            IValidator<ToDoItemModel> toDoItemValidator, 
            IToDoService toDoService, 
            ILogger<CreateToDoItemFunction> logger)
        {
            _toDoItemValidator = toDoItemValidator;
            _toDoService = toDoService;
            _logger = logger;
        }

        [Function(nameof(CreateToDoItemFunction))]
        public async Task<HttpResponseData> RunAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "task/create")]
            HttpRequestData req)
        {
            _logger.LogInformation("Got request for creating TODO item.");
            
            ToDoItemModel itemModel;

            using (var streamReader = new StreamReader(req.Body))
            {
                var stringContent = await streamReader.ReadToEndAsync();
                itemModel = JsonConvert.DeserializeObject<ToDoItemModel>(stringContent)!;
            }
            
            itemModel.Id = Guid.NewGuid();

            var validationResult = await _toDoItemValidator.ValidateAsync(itemModel);
            
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

            var createdItem = await _toDoService.CreateToDoItemAsync(itemModel);
            
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