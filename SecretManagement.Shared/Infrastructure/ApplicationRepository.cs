using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using SecretManagement.Shared.Entities;

namespace SecretManagement.Shared.Infrastructure
{
    public class ApplicationRepository : IApplicationRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IDataProtector _dataProtector;

        public ApplicationRepository(ApplicationDbContext context, IDataProtectionProvider dataProtectionProvider)
        {
            _context = context;
            _dataProtector = dataProtectionProvider.CreateProtector("e4c04b3455d1512d2b9f8b51f43c77f878dbc0319");
        }

        public async Task<Credential> GetAsync(int id, CancellationToken cancellationToken)
        {
            var credential = await _context.Credentials.FirstAsync(x => x.Id == id, cancellationToken);
            return new Credential
            {
                Id = credential.Id,
                CipheredSecretKey = credential.SecretKey,
                PainTextSecretKey = _dataProtector.Unprotect(credential.SecretKey)
            };
        }

        public async Task<Credential> AddAsync(Credential credential, CancellationToken cancellationToken)
        {
            var credentialData = new CredentialData
            {
                Id = credential.Id,
                SecretKey = _dataProtector.Protect(credential.PainTextSecretKey)
            };

            await _context.Credentials.AddAsync(credentialData, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return new Credential
            {
                Id = credentialData.Id,
                CipheredSecretKey = credentialData.SecretKey,
                PainTextSecretKey = credential.PainTextSecretKey
            };
        }
    }
}