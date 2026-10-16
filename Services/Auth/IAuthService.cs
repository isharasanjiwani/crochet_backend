using CrochetWorldApi.Dtos.Auth.Requests;
using CrochetWorldApi.Dtos.Auth.Responses;

namespace CrochetWorldApi.Services.Auth;

public interface IAuthService
{
    public Task<UserResponseDto?> CreateUserAsync(CreateUserRequestDto request);
    public Task<UserResponseDto?> LoginUserAsync(LoginUserRequestDto request);
    public Task<UserResponseDto?> RefreshTokenAsync(RefreshTokenRequestDto request);
}
