namespace SecretManagement.Shared.Models.Responses;

public class GetCredentialResponse
{
    public int Id { get; set; }
    public string SecretKey { get; set; } = string.Empty;
    public string PlainTextSecretKey { get; set; } = string.Empty;
}
