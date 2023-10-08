using DistributedDpApiDemo.Models;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataProtectionApi.Infrastructure
{
    public class ApplicationContext : DbContext, IDataProtectionKeyContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options) { }

        public DbSet<DataProtectionKey> DataProtectionKeys => Set<DataProtectionKey>();
        public DbSet<Credential> Credentials => Set<Credential>();
    }
}