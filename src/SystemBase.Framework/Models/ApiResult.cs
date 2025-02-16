
using Newtonsoft.Json;
using System.Net;

namespace SystemBase.Framework.Models;


public class ApiResult<T>
{

    public ApiResult()
    {

    }

    public ApiResult(T data, HttpStatusCode statusCode, bool success, string message = null)
    {
        Data = data;
        StatusCode = statusCode;
        Succeeded = success;
        Message = message;
    }

    public ApiResult(HttpStatusCode statusCode, bool success, string message = null)
    {
        StatusCode = statusCode;
        Succeeded = success;
        Message = message;
    }

    [JsonProperty("data")]
    public T Data { get; set; }
    [JsonProperty("succeeded")]
    public bool Succeeded { get; set; }
    [JsonProperty("message")]
    public string Message { get; set; }
    [JsonProperty("statusCode")]
    public HttpStatusCode StatusCode { get; set; }
    [JsonProperty("errors")]
    public IDictionary<string, string[]> Errors { get; set; }

}


