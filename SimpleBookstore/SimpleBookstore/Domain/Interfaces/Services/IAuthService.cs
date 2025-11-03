using SimpleBookstore.Domain.DTOs;
using SimpleBookstore.Domain.Entities;

namespace SimpleBookstore.Domain.Interfaces.Services;

public interface IAuthService
{
    Task<User?> RegisterAsync(UserDto request);
    Task<TokenResponseDto?> LoginAsync(UserDto request);
    Task<TokenResponseDto?> RefreshTokensAsync(RefreshTokenRequestDto request);
}
