using System.Net;
using Ballast.Business.Repository.Interfaces;
using Ballast.Business.Services.Interfaces;
using Ballast.Business.Validation;
using Ballast.DTO;

namespace Ballast.Business.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenRepository _tokenRepository;
        private readonly IHashService _security;

        public UserService(IUserRepository userRepository, ITokenRepository tokenRepository, IHashService security)
        {
            _userRepository = userRepository;
            _tokenRepository = tokenRepository;
            _security = security;
        }

        public async Task<ValidationResult<bool>> Create(UserCreateDTO userCreateDTO)
        {
            var userCreateResult = new ValidationResult<bool>();

            userCreateDTO.Password = _security.HashPassword(userCreateDTO.Password);
            var validationResult = new UserValidation().ValidateUserCreateDTO(userCreateDTO);
            if (validationResult.Errors.Count > 0)
            {
                userCreateResult.Errors = validationResult.Errors;
                return userCreateResult;
            }
            try
            {
                var user = await _userRepository.AddAsync(userCreateDTO.ConvertToUserDTO());
                if (user == false)
                {
                    userCreateResult.CustomResultCode = (int)HttpStatusCode.NotFound;
                    return userCreateResult;
                }
                userCreateResult.SingleResult = true;
                userCreateResult.CustomResultCode = (int)HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                userCreateResult.CustomResultCode = (int)HttpStatusCode.InternalServerError;
                userCreateResult.Errors.Add(ex.Message);
            }
            return userCreateResult;
        }

        public async Task<ValidationResult<UserDTO>> GetByUserName(string userName)
        {
            var validationResult = new UserValidation().ValidateUserName(userName);
            if (validationResult.Errors.Count > 0)
            {
                return validationResult;
            }
            try
            {
                var user = await _userRepository.GetByUserNameAsync(userName);
                if (user == null)
                {
                    validationResult.CustomResultCode = (int)HttpStatusCode.NotFound;
                    return validationResult;
                }
                validationResult.SingleResult = user;
                validationResult.CustomResultCode = (int)HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                validationResult.CustomResultCode = (int)HttpStatusCode.InternalServerError;
                validationResult.Errors.Add(ex.Message);
            }
            return validationResult;
        }

        public async Task<ValidationResult<UserGetDTO>> GetByUserId(int userId)
        {
            var validationResult = new UserValidation().ValidateUserId(userId);
            if (validationResult.Errors.Count > 0)
            {
                return validationResult;
            }
            try
            {
                var user = await _userRepository.GetByUserIdAsync(userId);
                if (user == null)
                {
                    validationResult.CustomResultCode = (int)HttpStatusCode.NotFound;
                    return validationResult;
                }
                validationResult.SingleResult = user;
                validationResult.CustomResultCode = (int)HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                validationResult.CustomResultCode = (int)HttpStatusCode.InternalServerError;
                validationResult.Errors.Add(ex.Message);
            }
            return validationResult;
        }

        public async Task<ValidationResult<string>> Login(UserLoginDTO userLoginDTO)
        {
            var validationResult = new ValidationResult<string>();
            var newTokenCode = Guid.NewGuid();

            var getUserResult = await GetByUserName(userLoginDTO.UserName);
            if (getUserResult.Errors.Count == 0 && getUserResult.SingleResult != null)
            {
                var userDTO = getUserResult.SingleResult;
                if (userDTO.PasswordHash == _security.HashPassword(userLoginDTO.Password))
                {
                    var newToken = new TokenDTO()
                    {
                        UserId = userDTO.UserId,
                        ExpiresIn = DateTime.Now.AddHours(1),
                        TokenCode = newTokenCode,
                    };
                    await _tokenRepository.AddAsync(newToken);
                }
                else
                {
                    validationResult.Errors.Add("Invalid credentials");
                    validationResult.CustomResultCode = (int)HttpStatusCode.Unauthorized;
                }
                validationResult.SingleResult = newTokenCode.ToString();
            }
            else
            {
                validationResult.Errors.Add("User not Found");
                validationResult.CustomResultCode = getUserResult.CustomResultCode;
            }

            return await Task.FromResult(validationResult);
        }

        public async Task<ValidationResult<bool>> ChangePassword(UserChangePasswordDTO userChangePasswordDTO)
        {
            var userChangePasswordResult = new ValidationResult<bool>();

            userChangePasswordDTO.NewPassword = _security.HashPassword(userChangePasswordDTO.NewPassword);
            var validationResult = new UserValidation().ValidateUserChangePassword(userChangePasswordDTO);
            if (validationResult.Errors.Count > 0)
            {
                userChangePasswordResult.Errors = validationResult.Errors;
                return userChangePasswordResult;
            }
            try
            {
                var affectedRowCount = await _userRepository.ChangePassword(userChangePasswordDTO);
                userChangePasswordResult.CustomResultCode = (int)HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                userChangePasswordResult.CustomResultCode = (int)HttpStatusCode.InternalServerError;
                userChangePasswordResult.Errors.Add(ex.Message);
            }
            return userChangePasswordResult;
        }

        public async Task<bool> ValidateToken(string tokenCode)
        {
            var tokenDTO = await _tokenRepository.GetByToken(Guid.Parse(tokenCode));
            var accessAllowed = false;
            if (tokenDTO == null)
            {
                accessAllowed = false;
            }
            else
            {
                if (tokenDTO.ExpiresIn > DateTime.Now)
                {
                    accessAllowed = true;
                }
            }
            return await Task.FromResult(accessAllowed);
        }
    }
}
