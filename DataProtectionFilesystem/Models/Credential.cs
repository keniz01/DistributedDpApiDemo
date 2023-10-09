using System.ComponentModel.DataAnnotations.Schema;

namespace SecretManagement.DataProtectionFilesystem.Models;

[Table("credentials")]
public class Credential
{
    [Column("id")]
    public int Id {get; set;}

    [Column("secret_key")]
    public string SecretKey {get; set;} = string.Empty;
}
