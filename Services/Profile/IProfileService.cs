using System;
using CrochetWorldApi.Dtos.Common.Response;
using CrochetWorldApi.Dtos.Profile.Requests;
using CrochetWorldApi.Dtos.Profile.Response;

namespace CrochetWorldApi.Services.Profile;

public interface IProfileService
{
    public Task<Result<BaseProfileResDto?>> GetProfileData(Guid id);
    public Task<Result<string?>> EditProfileData(EditProfileReqDto request, Guid id);
}
