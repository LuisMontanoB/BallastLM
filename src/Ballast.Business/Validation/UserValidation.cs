using System.Net;
using Ballast.DTO;

namespace Ballast.Business.Validation
{
    public class UserValidation
    {
        public ValidationResult<UserDTO> ValidateUserCreateDTO(UserCreateDTO userCreateDTO)
        {
            var validationResult = ValidateUserName(userCreateDTO.UserName);

            if (string.IsNullOrWhiteSpace(userCreateDTO.Password))
            {
                validationResult.Errors.Add("Password cannot be empty");
            }
            validationResult.SingleResult = userCreateDTO.ConvertToUserDTO();

            if (validationResult.Errors.Count > 0)
            {
                validationResult.CustomResultCode = (int)HttpStatusCode.BadRequest;
            }
            return validationResult;
        }

        public ValidationResult<UserDTO> ValidateUserName(string userName)
        {
            var validationResult = new ValidationResult<UserDTO>();

            if (string.IsNullOrWhiteSpace(userName))
            {
                validationResult.Errors.Add("UserName cannot be empty");
            }

            if (validationResult.Errors.Count > 0)
            {
                validationResult.CustomResultCode = (int)HttpStatusCode.BadRequest;
            }
            return validationResult;
        }

        public ValidationResult<UserGetDTO> ValidateUserId(int userId)
        {
            var validationResult = new ValidationResult<UserGetDTO>();

            if (userId <= 0)
            {
                validationResult.Errors.Add("UserId should be greater than Zero cannot be empty");
            }

            if (validationResult.Errors.Count > 0)
            {
                validationResult.CustomResultCode = (int)HttpStatusCode.BadRequest;
            }
            return validationResult;
        }

        public ValidationResult<UserGetDTO> ValidateUserChangePassword(UserChangePasswordDTO userChangePasswordDTO)
        {
            var validationResult = ValidateUserId(userChangePasswordDTO.UserId);

            if (string.IsNullOrWhiteSpace(userChangePasswordDTO.NewPassword))
            {
                validationResult.Errors.Add("Password cannot be empty");
            }
            return validationResult;
        }
    }
}
