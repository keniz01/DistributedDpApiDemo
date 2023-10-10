namespace SecretManagement.Shared.Entities
{
    public class Credential
    {
        public int Id { get; set; }
        public string CipheredSecretKey { get; set; } = string.Empty;
        public string PainTextSecretKey { get; set; } = string.Empty;
    }
}