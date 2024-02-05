using System.Net;
using Ballast.Business.Services.Interfaces;
using Ballast.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Ballast.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<StudentController> _logger;
        private readonly IUserService _userService;
        private Guid transactionId { get; set; }

        public UserController(ILogger<StudentController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        [HttpPost("Login")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login(UserLoginDTO userLoginDTO)
        {
            _logger.LogInformation($"Login:Enter {transactionId}");

            var loginResult = await _userService.Login(userLoginDTO);

            if (!loginResult.Errors.Any())
            {
                _logger.LogInformation($"Login:Returning LoginResult userLoginDTO.UserName:({loginResult.SingleResult}) {transactionId}");

                return Ok(loginResult);
            }
            else
            {
                var errors = Newtonsoft.Json.JsonConvert.SerializeObject(loginResult.Errors);

                if (loginResult.CustomResultCode == (int)HttpStatusCode.BadRequest)
                {
                    _logger.LogInformation($"Login:Returning BadRequest and errors({errors}) {transactionId}");

                    return BadRequest(string.Join(";", errors));
                }
                else
                {
                    _logger.LogInformation($"Login:Returning InternalServerError and errors({errors}) {transactionId}");

                    return StatusCode(StatusCodes.Status500InternalServerError, errors);
                }
            }
        }

        [HttpPost("Create")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(StudentDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateUser(UserCreateDTO userCreateDTO)
        {
            _logger.LogInformation($"CreateUser:Enter {transactionId}");

            var createStudentResult = await _userService.Create(userCreateDTO);

            if (!createStudentResult.Errors.Any())
            {
                _logger.LogInformation($"CreateUser:Returning CreatedAtAction userCreateDTO.UserName:({userCreateDTO.UserName}) {transactionId}");

                return Ok();
            }
            else
            {
                var errors = Newtonsoft.Json.JsonConvert.SerializeObject(createStudentResult.Errors);

                if (createStudentResult.CustomResultCode == (int)HttpStatusCode.BadRequest)
                {
                    _logger.LogInformation($"CreateUser:Returning BadRequest and errors({errors}) {transactionId}");

                    return BadRequest(string.Join(";", errors));
                }
                else
                {
                    _logger.LogInformation($"CreateUser:Returning InternalServerError and errors({errors}) {transactionId}");

                    return StatusCode(StatusCodes.Status500InternalServerError, errors);
                }
            }
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUserById([FromQuery] int userId)
        {
            _logger.LogInformation($"GetUserById:Enter {transactionId}");

            var getByIdStudentResult = await _userService.GetByUserId(userId);

            if (!getByIdStudentResult.Errors.Any())
            {
                if (getByIdStudentResult.CustomResultCode == (int)HttpStatusCode.NotFound)
                {
                    _logger.LogInformation($"GetUserById:Returning NotFound GetUserById:({userId}) {transactionId}");
                    return NotFound();
                }
                else
                {
                    _logger.LogInformation($"GetUserById:Returning OK GetUserById:({userId}) {transactionId}");

                    return Ok(getByIdStudentResult.SingleResult);
                }
            }
            else
            {
                var errors = Newtonsoft.Json.JsonConvert.SerializeObject(getByIdStudentResult.Errors);

                if (getByIdStudentResult.CustomResultCode == (int)HttpStatusCode.BadRequest)
                {
                    _logger.LogInformation($"GetUserById:Returning BadRequest and errors({errors}) {transactionId}");

                    return BadRequest(string.Join(";", errors));
                }
                else
                {
                    _logger.LogInformation($"GetUserById:Returning InternalServerError and errors({errors}) {transactionId}");

                    return StatusCode(StatusCodes.Status500InternalServerError, errors);
                }
            }
        }

        [HttpPatch("ChangePassword")]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ChangePassword([FromBody] UserChangePasswordDTO userChangePasswordDTO)
        {
            _logger.LogInformation($"ChangePassword:Enter {transactionId}");

            var changePasswordResult = await _userService.ChangePassword(userChangePasswordDTO);

            if (!changePasswordResult.Errors.Any())
            {
                return NoContent();
            }
            else
            {
                var errors = Newtonsoft.Json.JsonConvert.SerializeObject(changePasswordResult.Errors);

                if (changePasswordResult.CustomResultCode == (int)HttpStatusCode.BadRequest)
                {
                    _logger.LogInformation($"ChangePassword:Returning BadRequest and errors({errors}) {transactionId}");

                    return BadRequest(string.Join(";", errors));
                }
                else
                {
                    _logger.LogInformation($"ChangePassword:Returning InternalServerError and errors({errors}) {transactionId}");

                    return StatusCode(StatusCodes.Status500InternalServerError, errors);
                }
            }
        }
    }
}