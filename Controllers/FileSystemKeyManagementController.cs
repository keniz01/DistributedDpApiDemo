using DistributedDpApiDemo.Models;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using RepoDb;
using System.ComponentModel.DataAnnotations;

namespace DistributedDpApiDemo.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FileSystemKeyManagementController : ControllerBase
{
    private readonly IDataProtector _dataProtector;
    private readonly IConfiguration _configuration;

    public FileSystemKeyManagementController(IDataProtectionProvider provider, IConfiguration configuration)
    {
        _dataProtector = provider.CreateProtector("UhHZr-yjMEW_lsCDUAD-JgQsId3WLmZkeMc4GoHxJXOQ");
        _configuration = configuration;
    }

    [HttpPost()]
    public async Task<IResult> AddUserCredentialsAsync([Required]AddCredentialsRequest request)
    {
        var credentials = new Credentials
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
        var response = await connection.QueryAsync<Credentials>(m => m.Id == request.Id);
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
