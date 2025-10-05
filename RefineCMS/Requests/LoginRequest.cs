using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace RefineCMS.Requests;

[DataContract]
public class LoginRequest
{
    private string _username = string.Empty;

    [DataMember]
    [Required]
    public required string Username
    {
        get => _username.Trim();
        set => _username = value;
    }

    [DataMember]
    [Required]
    [DataType(DataType.Password)]
    public required string Password { get; set; }
}
