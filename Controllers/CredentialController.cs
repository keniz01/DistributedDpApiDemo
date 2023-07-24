using DistributedDpApiDemo.Models;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using RepoDb;
using System.ComponentModel.DataAnnotations;

namespace DistributedDpApiDemo.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CredentialController : ControllerBase
{
    private readonly IDataProtector _dataProtector;
    private readonly string _connectionString = string.Empty;

    public CredentialController(IDataProtectionProvider provider, IConfiguration configuration)
    {
        _dataProtector = provider.CreateProtector("UhHZr-yjMEW_lsCDUAD-JgQsId3WLmZkeMc4GoHxJXOQ");
        _ = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _connectionString = configuration.GetConnectionString("DefaultContext")!;
    }

    [HttpPost()]
    public async Task<IResult> AddUserCredentialsAsync([Required]AddCredentialsRequest request)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();
        var credentials = new Credentials
        {
            SecretKey = _dataProtector.Protect(request.SecretKey)
        };

        var id = await connection.InsertAsync(credentials);
        return Results.Ok(id);
    }

    [HttpGet()]
    public async Task<IResult> GetUserCredentialsAsync([FromQuery] GetCredentialsRequest request)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();
        var response = await connection.QueryAsync<Credentials>(m => m.Id == request.Id);
        var credentials = new GetCredentialsResponse
        {
            Id = response.First().Id,
            SecretKey = response.First().SecretKey,
            PlainTextSecretKey = _dataProtector.Unprotect(response.First().SecretKey)
        };

        return Results.Ok(credentials);
    }
}
