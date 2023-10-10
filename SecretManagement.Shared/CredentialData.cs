using System.ComponentModel.DataAnnotations.Schema;

namespace SecretManagement.Shared
{
    [Table("credentials")]
    public class CredentialData
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("secret_key")]
        public string SecretKey { get; set; } = string.Empty;
    }
}