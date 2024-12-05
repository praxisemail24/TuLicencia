using Newtonsoft.Json;

namespace SmartLicencia.Entity
{
    public class ResponseJSONModel<T>
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("message")]
        public string? Message { get; set; }

        [JsonProperty("data")]
        public T? Data { get; set; }
        public Exception? Error { get; set; }
    }

    public class ResponseJSON: ResponseJSONModel<object>
    {
    }
}
