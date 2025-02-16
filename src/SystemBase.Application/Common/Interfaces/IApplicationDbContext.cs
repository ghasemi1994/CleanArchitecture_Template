
using Microsoft.EntityFrameworkCore;
using SystemBase.Domain.Entities;

namespace SystemBase.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; }
}
