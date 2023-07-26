using Newtonsoft.Json;

namespace YS.Azure.ToDo.Models
{
    public class IdEntity
    {
        [JsonProperty("id")]
        public string Id { get; set; } = null!;
    }
}