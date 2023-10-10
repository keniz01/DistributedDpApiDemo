using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using RepoDb;
using SecretManagement.Shared.Entities;
using SecretManagement.Shared.Infrastructure;
using SecretManagement.Shared.Models.Requests;
using SecretManagement.Shared.Models.Responses;
using System.ComponentModel.DataAnnotations;

namespace SecretManagement.DataProtectionDatabase.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DatabaseStorageController : ControllerBase
{
    private readonly IApplicationRepository _applicationRepository;

    public DatabaseStorageController(
        IApplicationRepository applicationRepository)
    {
        _applicationRepository = applicationRepository;
    }

    [HttpPost()]
    public async Task<IResult> AddUserCredentialsAsync([Required] AddCredentialsRequest request, CancellationToken cancellationToken)
    {
        var credential = new Credential
        {
            PainTextSecretKey = request.SecretKey
        };

        var id = await _applicationRepository.AddAsync(credential, cancellationToken);
        return Results.Ok(id);
    }

    [HttpGet()]
    public async Task<IResult> GetUserCredentialsAsync([FromQuery] GetCredentialRequest request, CancellationToken cancellationToken)
    {
        var response = await _applicationRepository.GetAsync(request.Id, cancellationToken);
        var credentialResponse = new GetCredentialResponse
        {
            Id = response.Id,
            SecretKey = response.CipheredSecretKey,
            PlainTextSecretKey = response.PainTextSecretKey
        };

        return Results.Ok(credentialResponse);
    }
}
