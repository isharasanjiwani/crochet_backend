using System;
using CrochetWorldApi.Dtos.Common.Response;
using CrochetWorldApi.Dtos.General.Response;

namespace CrochetWorldApi.Services.General;

public interface IGeneralService
{
    public Task<Result<InitialDataResDto?>> GetInitialData(Guid id);
}
