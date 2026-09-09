using CustomerTestApp1.DTOS;
using CustomerTestApp1.Responses;

namespace CustomerTestApp1.Services.Interfaces
{
    public interface IAuthService
    {
        Task<ResultData<AuthResponseDto>> LoginAsync(LoginDto dto);
        Task<ResultData<AuthResponseDto>> RefreshAsync(RefreshRequestDto dto);
    }
}
