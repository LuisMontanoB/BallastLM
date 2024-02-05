using Ballast.Business.Repository.Interfaces;
using Ballast.DTO;
using Microsoft.Extensions.Configuration;

namespace Ballast.Business.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly Data.User _userDL;

        public UserRepository(IConfiguration configuration)
        {
            _userDL = new Data.User(configuration);
        }

        public async Task<bool> AddAsync(UserDTO entity)
        {
            await _userDL.InsertAsync(entity);
            return await Task.FromResult(true);
        }

        public async Task<UserDTO?> GetByUserNameAsync(string userName)
        {
            return await _userDL.GetByUserNameAsync(userName);
        }

        public async Task<UserGetDTO?> GetByUserIdAsync(int userId)
        {
            return await _userDL.GetByUserIdAsync(userId);
        }

        public async Task<int> ChangePassword(UserChangePasswordDTO userChangePasswordDTO)
        {
            return await _userDL.ChangePasswordAsync(userChangePasswordDTO);
        }
        
    }
}
