using ResellioBackend.UserManagementSystem.Models.Base;
using ResellioBackend.UserManagementSystem.Repositories.Abstractions;
using ResellioBackend.UserManagementSystem.Results;
using ResellioBackend.UserManagementSystem.Services.Abstractions;

namespace ResellioBackend.UserManagementSystem.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUsersRepository<UserBase> _usersRepository;

        public UserService(IUsersRepository<UserBase> usersRepository) 
        {
            _usersRepository = usersRepository; 
        }

        public async Task<GetUserInfoResult> GetUserInfoAsync(int userId)
        {
            var user = await  _usersRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return new GetUserInfoResult()
                {
                    Success = false,
                    Message = "Something wrong with your token"
                };
            }
            var userInfo = user.GetMyInfo();

            return new GetUserInfoResult()
            {
                Success = true,
                userInfo = userInfo,
            };
        }
    }
}
