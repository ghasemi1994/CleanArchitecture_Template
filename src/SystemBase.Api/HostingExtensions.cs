using FluentValidation.AspNetCore;
using Microsoft.Extensions.FileProviders;
using SystemBase.Api.Extension;
using SystemBase.Application;
using SystemBase.Infrastructure;

namespace SystemBase.Api;

public static class HostingExtensions
{

    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddApplication(builder.Configuration);
        builder.Services.AddInfrastructure(builder.Configuration, builder.Environment);


        builder.Services
           ?.AddControllers()
           ?.AddFluentValidation()
           ?.AddNewtonsoftJson();

        builder.Services.AddSwaggerGen();
        builder.Services.AddEndpointsApiExplorer();

        builder.AddSerilog();

        builder.AddCorsPolicies(builder.Configuration);

        builder.AddSwagger();


        return builder.Build();
    }

    public static async Task<WebApplication> ConfigurePipeline(this WebApplication app)
    {
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            //app.UseDeveloperExceptionPage();
        }

        //swagger
        app.UseApplicationSwagger();

        app.UseStaticFiles();
        app.UseStaticFiles(new StaticFileOptions()
        {
            FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory())),
        });

        app.UseCors();

        //app.UseHttpsRedirection();

        app.UseAuthentication();

        app.UseAuthorization();

        app.MapControllers();


        app.UseErrorHandlerMiddleware();


        await SystemBase.Infrastructure.Persistence.Seed.ApplicationDbContextSeed.PrePoulation(app, true);

        return app;
    }
}
