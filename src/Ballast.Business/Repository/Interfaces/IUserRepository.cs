using Ballast.DTO;

namespace Ballast.Business.Repository.Interfaces
{
    public interface IUserRepository
    {
        Task<UserDTO?> GetByUserNameAsync(string userName);
        Task<UserGetDTO?> GetByUserIdAsync(int userId);
        Task<bool> AddAsync(UserDTO entity);
        Task<int> ChangePassword(UserChangePasswordDTO userChangePasswordDTO);
    }
}
