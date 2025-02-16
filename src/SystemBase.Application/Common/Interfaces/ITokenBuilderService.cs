using SystemBase.Domain.Entities;

namespace SystemBase.Domain.Interfaces;

public interface ITokenBuilderService
{
    Task<string> GetJweTokenAsync(User user, CancellationToken cancellationToken);
}
