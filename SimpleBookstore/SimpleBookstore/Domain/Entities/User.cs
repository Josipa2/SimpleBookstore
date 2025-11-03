using System.ComponentModel.DataAnnotations;

namespace SimpleBookstore.Domain.Entities;

public class User
{
    [Key]
    public int Id { get; set; }

    public string Username { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public string Role { get; set; } = string.Empty;

    public string? RefreshToken { get; set; }

    public DateTime? RefreshTokenExpiryTime { get; set; }
}
