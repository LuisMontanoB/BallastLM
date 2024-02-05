using System.Net;
using Ballast.Business.Services.Interfaces;
using Ballast.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Ballast.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly ILogger<StudentController> _logger;
        private readonly IStudentService _studentService;
        private readonly IUserService _userService;

        private Guid transactionId { get; set; }

        public StudentController(ILogger<StudentController> logger, IStudentService studentService, IUserService userService)
        {
            _logger = logger;
            _studentService = studentService;
            _userService = userService;
        }

        [HttpGet("GetAll")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<StudentDTO>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllStudents([FromHeader] string token, [FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            _logger.LogInformation($"GetAllStudents:Enter pageNumber={pageNumber}, pageSize={pageSize}");

            var validateToken = await _userService.ValidateToken(token);
            if (!validateToken)
            {
                return Unauthorized();
            }

            var getStudentsResult = await _studentService.GetAll(pageNumber, pageSize);
            if (!getStudentsResult.Errors.Any())
            {
                _logger.LogInformation($"GetAllStudents:Returning OK {transactionId}");

                return Ok(getStudentsResult.ResultList);
            }
            else
            {
                if (getStudentsResult.CustomResultCode == (int)HttpStatusCode.BadRequest)
                {
                    var errors = Newtonsoft.Json.JsonConvert.SerializeObject(getStudentsResult.Errors);
                    _logger.LogInformation($"GetAllStudents:Returning InternalServerError and errors({errors}) {transactionId} ");

                    return BadRequest(errors);
                }
                else
                {
                    var errors = Newtonsoft.Json.JsonConvert.SerializeObject(getStudentsResult.Errors);
                    _logger.LogInformation($"GetAllStudents:Returning InternalServerError and errors({errors}) {transactionId} ");

                    return StatusCode(StatusCodes.Status500InternalServerError, errors);

                }
            }
        }

        [HttpGet("GetById")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StudentDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetStudentById([FromHeader] string token, [FromQuery] int studentId)
        {
            _logger.LogInformation($"GetStudentById:Enter studentId={studentId}");
            var validateToken = await _userService.ValidateToken(token);
            if (!validateToken)
            {
                return Unauthorized();
            }

            var getStudentsResult = await _studentService.GetById(studentId);
            if (!getStudentsResult.Errors.Any())
            {
                _logger.LogInformation($"GetStudentById:Returning OK {transactionId}");
                if (getStudentsResult.SingleResult == null)
                {
                    return NotFound();
                }
                return Ok(getStudentsResult.SingleResult);
            }
            else
            {
                if (getStudentsResult.CustomResultCode == (int)HttpStatusCode.BadRequest)
                {
                    var errors = Newtonsoft.Json.JsonConvert.SerializeObject(getStudentsResult.Errors);
                    _logger.LogInformation($"GetAllStudents:Returning InternalServerError and errors({errors}) {transactionId} ");

                    return BadRequest(errors);
                }
                else
                {
                    var errors = Newtonsoft.Json.JsonConvert.SerializeObject(getStudentsResult.Errors);
                    _logger.LogInformation($"GetStudentById:Returning InternalServerError and errors({errors}) {transactionId} ");

                    return StatusCode(StatusCodes.Status500InternalServerError, errors);
                }
            }
        }

        [HttpPost("")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(StudentDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateStudent([FromHeader] string token, [FromBody] StudentCreateDTO studentCreateDTO)
        {
            _logger.LogInformation($"CreateStudent:Enter {transactionId}");
            var validateToken = await _userService.ValidateToken(token);
            if (!validateToken)
            {
                return Unauthorized();
            }

            var createStudentResult = await _studentService.Create(studentCreateDTO);

            if (!createStudentResult.Errors.Any())
            {
                _logger.LogInformation($"CreateStudent:Returning CreatedAtAction StudentId:({createStudentResult.SingleResult?.StudentId}) {transactionId}");

                return CreatedAtAction("GetStudentById", new { StudentId = createStudentResult.SingleResult?.StudentId }, createStudentResult.SingleResult);
            }
            else
            {
                var errors = Newtonsoft.Json.JsonConvert.SerializeObject(createStudentResult.Errors);

                if (createStudentResult.CustomResultCode == (int)HttpStatusCode.BadRequest)
                {
                    _logger.LogInformation($"CreateStudent:Returning BadRequest and errors({errors}) {transactionId}");

                    return BadRequest(string.Join(";", errors));
                }
                else
                {
                    _logger.LogInformation($"CreateStudent:Returning InternalServerError and errors({errors}) {transactionId}");

                    return StatusCode(StatusCodes.Status500InternalServerError, errors);
                }
            }
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ChangePassword([FromHeader] string token, [FromQuery] int studentId, [FromBody] StudentUpdateDTO studentUpdateDTO)
        {
            _logger.LogInformation($"UpdateStudent:Enter {transactionId}, studentId = {studentId}");

            var validateToken = await _userService.ValidateToken(token);
            if (!validateToken)
            {
                return Unauthorized();
            }

            var updateStudentResult = await _studentService.Update(studentId, studentUpdateDTO);
            if (!updateStudentResult.Errors.Any())
            {
                _logger.LogInformation($"UpdateStudent:Returning NoContent {transactionId}");

                return NoContent();
            }
            else
            {
                var errors = Newtonsoft.Json.JsonConvert.SerializeObject(updateStudentResult.Errors);

                if (updateStudentResult.CustomResultCode == (int)HttpStatusCode.BadRequest)
                {
                    _logger.LogInformation($"UpdateStudent:Returning BadRequest and errors({errors}) {transactionId}");

                    return BadRequest(string.Join(";", errors));
                }
                else
                {
                    _logger.LogInformation($"UpdateStudent:Returning InternalServerError and errors({errors}) {transactionId}");

                    return StatusCode(StatusCodes.Status500InternalServerError, errors);
                }
            }
        }

        [HttpDelete("{studentId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteStudent([FromHeader] string token, [FromQuery] int studentId)
        {
            _logger.LogInformation($"DeleteStudent:Enter studentId:({studentId}){transactionId}");

            var validateToken = await _userService.ValidateToken(token);
            if (!validateToken)
            {
                return Unauthorized();
            }

            var deleteUserResult = await _studentService.Delete(studentId);
            if (!deleteUserResult.Errors.Any())
            {
                _logger.LogInformation($"DeleteStudent:Returning NoContent {transactionId}");

                return NoContent();
            }

            else
            {
                var errors = Newtonsoft.Json.JsonConvert.SerializeObject(deleteUserResult.Errors);

                if (deleteUserResult.CustomResultCode == (int)HttpStatusCode.BadRequest)
                {
                    _logger.LogInformation($"DeleteStudent:Returning BadRequest and errors({errors})  {transactionId}");

                    return BadRequest(string.Join(";", errors));
                }
                else
                {
                    _logger.LogInformation($"DeleteStudent:Returning InternalServerError and errors({errors})  {transactionId}");

                    return StatusCode(StatusCodes.Status500InternalServerError, errors);
                }
            }
        }
    }
}