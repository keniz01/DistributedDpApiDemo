using SecretManagement.Shared.Entities;

namespace SecretManagement.Shared.Infrastructure
{
    public interface IApplicationRepository
    {
        Task<Credential> AddAsync(Credential credential, CancellationToken cancellationToken);
        Task<Credential> GetAsync(int id, CancellationToken cancellationToken);
    }
}