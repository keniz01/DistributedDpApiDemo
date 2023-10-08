using DistributedDpApiDemo.Models;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using RepoDb;
using System.ComponentModel.DataAnnotations;

namespace DistributedDpApiDemo.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FilesystemStorageController : ControllerBase
{
    private readonly IDataProtector _dataProtector;
    private readonly IConfiguration _configuration;

    public FilesystemStorageController(IDataProtectionProvider provider, IConfiguration configuration)
    {
        _dataProtector = provider.CreateProtector("secure-app");
        _configuration = configuration;
    }

    [HttpPost()]
    public async Task<IResult> AddUserCredentialsAsync([Required]AddCredentialsRequest request)
    {
        var credentials = new Credential
        {
            SecretKey = _dataProtector.Protect(request.SecretKey)
        };
        
        var connection = await CreateConnection(CancellationToken.None);
        var id = await connection.InsertAsync(credentials);
        return Results.Ok(id);
    }

    [HttpGet()]
    public async Task<IResult> GetUserCredentialsAsync([FromQuery] GetCredentialsRequest request)
    {
        var connection = await CreateConnection(CancellationToken.None);
        var response = await connection.QueryAsync<Credential>(m => m.Id == request.Id);
        var credentials = new GetCredentialsResponse
        {
            Id = response.First().Id,
            SecretKey = response.First().SecretKey,
            PlainTextSecretKey = _dataProtector.Unprotect(response.First().SecretKey)
        };

        return Results.Ok(credentials);
    }

    private async Task<NpgsqlConnection> CreateConnection(CancellationToken cancellationToken)
    {
        var connectionString = _configuration.GetConnectionString("DefaultContext")!;
        var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync(cancellationToken);
        return connection;
    }
}
