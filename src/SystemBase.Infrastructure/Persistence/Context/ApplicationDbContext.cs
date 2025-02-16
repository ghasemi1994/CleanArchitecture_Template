
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Reflection;
using SystemBase.Domain.Entities;
using SystemBase.Application.Common.Interfaces;
using SystemBase.Domain.Interfaces;
using SystemBase.Framework;

namespace SystemBase.Infrastructure.Persistence.Context;

public class ApplicationDbContext : BaseDbContext , IApplicationDbContext
{
    private readonly IMediator _mediator;
    private readonly ICurrentUserService _currentUserService;
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, 
        ICurrentUserService currentUser, 
        IMediator mediator) : base(options)
    {
        _mediator = mediator;
        _currentUserService = currentUser;
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        changeDatetimeType(builder);
        changeDecimalType(builder);
        builder.ApplyConfigurationsFromAssembly(assembly: Assembly.GetExecutingAssembly());
    }

    #region DbSet
    public DbSet<User> Users { get; set; }
    #endregion

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (EntityEntry<BaseEntity> entry in ChangeTracker.Entries<BaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedDate = DateTime.Now;
                    entry.Entity.CreatedBy = _currentUserService.UserId;
                    entry.Entity.IsDeleted = false;
                    break;
                case EntityState.Modified:
                    entry.Entity.LastModifiedDate = DateTime.Now;
                    entry.Entity.ModifiedBy = _currentUserService.UserId;
                    break;
            }
        }

        var result = await base.SaveChangesAsync(cancellationToken);
        await PublishDomainEventsAsync();
        return result;
    }

    #region Utility
    private async Task PublishDomainEventsAsync()
    {
        var domainEvents = ChangeTracker
            .Entries<BaseEntity>()
            .Select(entry => entry.Entity)
            .SelectMany(entity =>
            {
                var domainEvents = entity.DomainEvents;
                return domainEvents;

            }).ToList();

        foreach (var domainEvent in domainEvents)
        {
            await _mediator.Publish(domainEvent);
        }

    }
    private void changeDatetimeType(ModelBuilder builder)
    {
        foreach (var property in builder.Model.GetEntityTypes().SelectMany(t => t.GetProperties())
        .Where(p => p.ClrType == typeof(DateTime) || p.ClrType == typeof(DateTime?)))
        {
            property.SetColumnType("datetime");
        }
    }
    private void changeDecimalType(ModelBuilder builder)
    {
        foreach (var property in builder.Model.GetEntityTypes().SelectMany(s => s.GetProperties())
            .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
        {
            property.SetColumnType("decimal(18,2)");
        }
    }
    #endregion
}
