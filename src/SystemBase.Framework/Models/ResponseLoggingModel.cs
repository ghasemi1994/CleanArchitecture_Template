
using Newtonsoft.Json;

namespace SystemBase.Framework.Models;

public class ResponseLoggingModel
{
    [JsonProperty("body")]
    public string Body { get; set; }
}
