using System.Net;
using Microsoft.Azure.Functions.Worker.Http;
using YS.Azure.ToDo.Models;

namespace YS.Azure.ToDo.Helpers
{
    public static class ResponseHelper
    {
        public static async Task<HttpResponseData> CreateApiResponseAsync(
            this HttpRequestData req, 
            HttpStatusCode statusCode, 
            ApiResponseMessage? responseBody = null,
            CancellationToken cancellationToken = default)
        {
            var response = req.CreateResponse(statusCode);

            if (responseBody != null)
            {
                responseBody.ExecutionTime = DateTime.Now;

                await response.WriteAsJsonAsync(responseBody, cancellationToken);
            }
            
            return response;
        }
    }
}