
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SystemBase.Domain.Entities;
using SystemBase.Infrastructure.Persistence.Context;
using SystemBase.Utility.Helpers;

namespace SystemBase.Infrastructure.Persistence.Seed;

public static class ApplicationDbContextSeed
{
    public static async Task PrePoulation(IApplicationBuilder app, bool isProd)
    {
        using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>()?.CreateScope() ?? throw new NullReferenceException())
        {
            await SeedData(serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>(), isProd);
        }
    }
    public static async Task SeedData(ApplicationDbContext context, bool isProd)
    {
        if (isProd)
        {
            await context.Database.MigrateAsync();

            await AddUser(context);

            await context.SaveChangesAsync();
        }
    }


    private static async Task AddUser(ApplicationDbContext context)
    {
        if (!await context.Users.AnyAsync())
        {
            await context.Users.AddAsync(new User(
                "admin@123",
                PasswordHasher.HashPassword("admin@123"),
                "09193597354",
                "m.gsmi1994@yahoo.com"));
        }
    }

}
