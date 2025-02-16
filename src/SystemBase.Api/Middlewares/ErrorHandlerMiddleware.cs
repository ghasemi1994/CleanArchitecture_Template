
using Newtonsoft.Json;
using Serilog.Context;
using System.Diagnostics;
using System.Net;
using System.Text;
using SystemBase.Domain.Interfaces;
using SystemBase.Framework.Models;

namespace Insurance.Apis.Middlewares;



public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IWebHostEnvironment _env;
    private readonly ILogger<ErrorHandlerMiddleware> _logger;
    private readonly Stopwatch _timer;

    public ErrorHandlerMiddleware(RequestDelegate next, IWebHostEnvironment env, ILogger<ErrorHandlerMiddleware> logger)
    {
        _next = next;
        _env = env;
        _logger = logger;
        _timer = new Stopwatch();
    }

    public async Task Invoke(HttpContext context, ICurrentUserService currentUserService)
    {
        string? message = null;
        HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError;        
        var requestModel = await _formatRequest(context.Request);

        try
        {
            _timer.Start();
            await _next(context);
            _timer.Stop();

            //if (_timer.ElapsedMilliseconds > 3000)
                //_logger.LogWarning($"request: '{context.Request.Path}' elapsed time is {_timer.ElapsedMilliseconds}ms");
        }
        catch (Exception ex)
        {
            AddLog(ex, context, currentUserService, requestModel);
            /*switch (ex)
            {               

                default:
                    AddLog(ex, context, currentUserService, requestModel);
                    break;
            }*/

            await WriteToResponseAsync(ex);
        }

        void AddLog(Exception ex, HttpContext httpContext, ICurrentUserService currentUserService, RequestLoggingModel requestLoggingModel)
        {
            using (LogContext.PushProperty("JsonProperties", JsonConvert.SerializeObject(requestModel)))
            using (LogContext.PushProperty("UserId", currentUserService.UserId == 0 ? null : currentUserService.UserId))
            using (LogContext.PushProperty("Path", httpContext.Request.Path))
            using (LogContext.PushProperty("RemoteIpAddress", httpContext.Connection.RemoteIpAddress))
            {
                _logger.LogError(ex, ex?.Message);
            }
        }

        async Task WriteToResponseAsync(Exception ex)
        {
            if (context.Response.HasStarted)
                throw new InvalidOperationException("The response has already started, the http status code middleware will not be executed.");

            string msg = string.Empty;

            if (_env.IsDevelopment())
                msg = ex.Message ?? "error is not available";
            else
                msg = message ?? "در پردازش اطلاعات مشکلی بوجود آمده است لطفا با راهبر سیستم تماس حاصل فرمایید";


            var result = new ApiResult<object>(httpStatusCode, false, msg);

            var json = JsonConvert.SerializeObject(result);

            context.Response.StatusCode = (int)httpStatusCode;

            context.Response.ContentType = "application/json";

            await context.Response.WriteAsync(json);
        }

    }

    private async Task<RequestLoggingModel> _formatRequest(HttpRequest request)
    {
        request.EnableBuffering();
        var body = request.Body;
        var buffer = new byte[Convert.ToInt32(request.ContentLength)];
        await request.Body.ReadAsync(buffer, 0, buffer.Length);
        var bodyAsText = Encoding.UTF8.GetString(buffer);
        request.Body.Seek(0, SeekOrigin.Begin);
        request.Body = body;

        return new RequestLoggingModel
        {
            Body = bodyAsText,
            QueryString = request.QueryString.ToString(),
        };
    }

    private async Task<ResponseLoggingModel> _formatResponse(HttpResponse response)
    {
        response.Body.Seek(0, SeekOrigin.Begin);
        string text = await new StreamReader(response.Body).ReadToEndAsync();
        response.Body.Seek(0, SeekOrigin.Begin);
        return new ResponseLoggingModel
        {
            Body = text,
        };
    }

}
