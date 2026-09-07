using CustomerTestApp1.Models;
using CustomerTestApp1.Responses;

namespace CustomerTestApp1.Services.Interfaces
{
    public interface IUserService
    {
        Task<ResultData<User>> GetUsersAsync();
        Task<ResultData<User>> GetUserByIdAsync(int id);
        Task<ResultData<User>> UpdateUserAsync(User user);
        Task<ResultData<User>> DeleteUserAsync(int id);
    }
}
