using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace RefineCMS.Models;

[Table("Users")]
[Index(nameof(UserLogin), IsUnique = true)]
public class User : BaseModel
{
    [Column("UserLogin")]
    public required string UserLogin { get; set; }

    [Column("UserPass")]
    [JsonIgnore]
    public string UserPass { get; set; } = string.Empty;

    [Column("UserEmail")]
    public string UserEmail { get; set; } = string.Empty;

    [Column("UserStatus")]
    public int UserStatus { get; set; } = 0;

    [Column("DisplayName")]
    public string DisplayName { get; set; } = string.Empty;

    //================================================================================

    public bool VerifyPassword(string password)
    {
        return BCrypt.Net.BCrypt.Verify(password, UserPass);
    }

    public void SetPassword(string password)
    {
        UserPass = BCrypt.Net.BCrypt.HashPassword(password);
    }
}