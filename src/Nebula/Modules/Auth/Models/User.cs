using System.Text.Json.Serialization;
using Nebula.Modules.Auth.Helpers;
using System.ComponentModel.DataAnnotations;

namespace Nebula.Modules.Auth.Models;

public class User
{
    public int Id { get; set; }

    [MaxLength(50)]
    public string UserName { get; set; } = string.Empty;

    [JsonIgnore]
    [MaxLength(100)]
    public string PasswordHash { get; set; } = string.Empty;

    [MaxLength(100)]
    public string Email { get; set; } = string.Empty;

    [MaxLength(100)]
    public string FullName { get; set; } = string.Empty;

    [MaxLength(15)]
    public string PhoneNumber { get; set; } = string.Empty;

    [MaxLength(10)]
    public string AccountType { get; set; } = AccountTypeHelper.Personal;
}
