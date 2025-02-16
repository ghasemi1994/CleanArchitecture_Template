using Serilog;
using SystemBase.Api;

try
{
    var builder = WebApplication.CreateBuilder(args);

    var app = await builder.ConfigureServices().ConfigurePipeline();

    Log.Information("application started.");

    app.Run();
}
catch (Exception ex)
{
    throw new Exception(ex.Message, ex);
}
