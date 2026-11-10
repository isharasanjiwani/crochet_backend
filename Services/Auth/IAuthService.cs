using CrochetWorldApi.Dtos.Auth.Requests;
using CrochetWorldApi.Dtos.Auth.Responses;
using CrochetWorldApi.Dtos.Common.Response;

namespace CrochetWorldApi.Services.Auth;

public interface IAuthService
{
    public Task<Result<UserResponseDto?>> CreateUserAsync(CreateUserRequestDto request);
    public Task<Result<UserResponseDto?>> LoginUserAsync(LoginUserRequestDto request);
    public Task<Result<UserResponseDto?>> RefreshTokenAsync(RefreshTokenRequestDto request);
}
