
using Newtonsoft.Json;

namespace SystemBase.Framework.Models;

public class RequestLoggingModel
{
    [JsonProperty("body")]
    public string Body { get; set; }
    [JsonProperty("queryString")]
    public string QueryString { get; set; }
}
