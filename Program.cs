//dotnet add package Microsoft.AspNetCore.DataProtection
//dotnet add package RepoDb.PostgreSql
//dotnet add package Npgsql
//dotnet add package Microsoft.EntityFrameworkCore
//dotnet add package Microsoft.EntityFrameworkCore.Design
//dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL 
//dotnet add package Microsoft.AspNetCore.DataProtection.EntityFrameworkCore
//

using DataProtectionApi.Infrastructure;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using RepoDb;
using System;
using System.Diagnostics.Metrics;
using System.Security.Cryptography.X509Certificates;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var keysFolder = Path.Combine(builder.Environment.ContentRootPath, "keys");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultContext"));
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    options.EnableSensitiveDataLogging();
});

var certificate = FindCertificate("deeb4fdc0470fb8a3dcabc1aa6614249661987cd");

builder.Services.AddDataProtection()
    .PersistKeysToDbContext<ApplicationDbContext>()
    .PersistKeysToFileSystem(new DirectoryInfo(keysFolder))
    //.ProtectKeysWithCertificate(certificate)
    .SetApplicationName("DistributedDpApiDemo");

GlobalConfiguration
	.Setup()
	.UsePostgreSql();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
using var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
context.Database.Migrate();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

static X509Certificate2 FindCertificate(string serialNumber)
{
    return Find(serialNumber, StoreLocation.CurrentUser) ?? Find(serialNumber, StoreLocation.LocalMachine);
}
static X509Certificate2 Find(string serialNumber, StoreLocation location)
{
    using (var store = new X509Store(location))
    {
        store.Open(OpenFlags.OpenExistingOnly);
        var certs = store.Certificates.Find(X509FindType.FindBySerialNumber, serialNumber, true);
        return certs.OfType<X509Certificate2>().First();
    }
}
