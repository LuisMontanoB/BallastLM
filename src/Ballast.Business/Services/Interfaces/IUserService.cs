using Ballast.Business.Validation;
using Ballast.DTO;

namespace Ballast.Business.Services.Interfaces
{
    public interface IUserService
    {
        Task<ValidationResult<bool>> Create(UserCreateDTO userCreateDTO);
        Task<ValidationResult<UserDTO>> GetByUserName(string userName);
        Task<ValidationResult<UserGetDTO>> GetByUserId(int userId);
        Task<bool> ValidateToken(string tokenCode);
        Task<ValidationResult<string>> Login(UserLoginDTO userLoginDTO);
        Task<ValidationResult<bool>> ChangePassword(UserChangePasswordDTO userChangePasswordDTO);
    }
}
