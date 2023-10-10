namespace SecretManagement.DataProtectionFilesystem.Models;

public class GetCredentialsResponse
{
    public int Id {get; set;}
    public string SecretKey {get; set;} = string.Empty;
    public string PlainTextSecretKey { get; internal set; } = string.Empty;

    public void UnProtectSecretKey( string protectedSecretKey) => SecretKey = protectedSecretKey;
}
