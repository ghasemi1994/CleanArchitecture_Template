
using Microsoft.Extensions.Logging;
using SystemBase.Domain.Interfaces;

namespace SystemBase.Infrastructure.Logging;

public class LoggerAdapter<T> : IAppLogger<T>
{

    private readonly ILogger<T> _logger;
    private readonly ICurrentUserService _currentUserService;
    public LoggerAdapter(ILoggerFactory loggerFactory, ICurrentUserService currentUserService)
    {
        _logger = loggerFactory.CreateLogger<T>();
        _currentUserService = currentUserService;
    }

    public void LogWarning(string message, params object[] args)
    {
        using (Serilog.Context.LogContext.PushProperty("UserId", _currentUserService.UserId == 0 ? null : _currentUserService.UserId))
        using (Serilog.Context.LogContext.PushProperty("Path", _currentUserService.HttpContext.Request.Path))
        using (Serilog.Context.LogContext.PushProperty("RemoteIpAddress", _currentUserService.IpAddress))
        {
            _logger.LogWarning(message, args);
        }
    }

    public void LogInformation(string message, params object[] args)
    {
        using (Serilog.Context.LogContext.PushProperty("UserId", _currentUserService.UserId == 0 ? null : _currentUserService.UserId))
        using (Serilog.Context.LogContext.PushProperty("Path", _currentUserService.HttpContext.Request.Path))
        using (Serilog.Context.LogContext.PushProperty("RemoteIpAddress", _currentUserService.IpAddress))
        {
            _logger.LogInformation(message, args);
        }
    }

    public void LoError(string message, params object[] args)
    {
        using (Serilog.Context.LogContext.PushProperty("UserId", _currentUserService.UserId == 0 ? null : _currentUserService.UserId))
        using (Serilog.Context.LogContext.PushProperty("Path", _currentUserService.HttpContext.Request.Path))
        using (Serilog.Context.LogContext.PushProperty("RemoteIpAddress", _currentUserService.IpAddress))
        {
            _logger.LogError(message, args);
        }
    }
}
