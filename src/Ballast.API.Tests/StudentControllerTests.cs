using System.Net;
using Ballast.API.Controllers;
using Ballast.Business.Services.Interfaces;
using Ballast.Business.Validation;
using Ballast.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace Ballast.API.Tests
{
    [TestClass]
    public class StudentControllerTests
    {
        [TestInitialize]
        public void Setup()
        {

        }

        #region GetAll
        [TestMethod]
        public void When_Student_GetAll_Is_Authenticated_Returns_Data_And_Has_SameRecordCount_As_Source()
        {
            //Arrange
            var studentsToRetrieve = GetStudentDTOList();
            var pageNumber = 1;
            var pageSize = 50;
            var token = Guid.Empty;
            var validationResult = new ValidationResult<StudentDTO>()
            {
                CustomResultCode = 200,
                Errors = new List<string>(),
                ResultList = studentsToRetrieve.ToList(),
                SingleResult = null
            };

            var loggerMock = new Mock<ILogger<StudentController>>();
            var studentMock = new Mock<IStudentService>();
            var userMock = new Mock<IUserService>();

            studentMock.Setup(x => x.GetAll(It.IsAny<int>(), It.IsAny<int>())).Returns(Task.FromResult(validationResult));
            userMock.Setup(x => x.ValidateToken(It.IsAny<string>())).Returns(Task.FromResult(true));

            var studentController = new StudentController(loggerMock.Object, studentMock.Object, userMock.Object);

            //ACT
            var controllerResult = studentController.GetAllStudents(token.ToString(), pageNumber, pageSize);

            //Assert
            Assert.IsNotNull(controllerResult);
            Assert.IsInstanceOfType(controllerResult.Result, typeof(OkObjectResult));
            Assert.AreEqual(studentsToRetrieve.Count, ((List<StudentDTO>)((OkObjectResult)controllerResult.Result).Value).Count);
        }

        [TestMethod]
        public void When_Student_GetAll_Is_Not_Authenticated_Returns_Unauthorized()
        {
            //Arrange
            var studentsToRetrieve = GetStudentDTOList();
            var pageNumber = 1;
            var pageSize = 50;
            var token = Guid.Empty;
            var validationResult = new ValidationResult<StudentDTO>()
            {
                CustomResultCode = 200,
                Errors = new List<string>(),
                ResultList = studentsToRetrieve.ToList(),
                SingleResult = null
            };

            var loggerMock = new Mock<ILogger<StudentController>>();
            var studentMock = new Mock<IStudentService>();
            var userMock = new Mock<IUserService>();

            studentMock.Setup(x => x.GetAll(It.IsAny<int>(), It.IsAny<int>())).Returns(Task.FromResult(validationResult));
            userMock.Setup(x => x.ValidateToken(It.IsAny<string>())).Returns(Task.FromResult(false));

            var studentController = new StudentController(loggerMock.Object, studentMock.Object, userMock.Object);

            //ACT
            var controllerResult = studentController.GetAllStudents(token.ToString(), pageNumber, pageSize);

            //Assert
            Assert.IsNotNull(controllerResult);
            Assert.IsInstanceOfType(controllerResult.Result, typeof(UnauthorizedResult));
        }

        [TestMethod]
        public void When_Student_GetAll_Is_Authenticated_But_HasErrors_Returns_BadRequest()
        {
            //Arrange
            var studentsToRetrieve = GetStudentDTOList();
            var pageNumber = 1;
            var pageSize = 50;
            var token = Guid.Empty;
            var validationResult = new ValidationResult<StudentDTO>()
            {
                CustomResultCode = (int)HttpStatusCode.BadRequest,
                Errors = new List<string>()
                {
                    "Some Error Message"
                },
                ResultList = studentsToRetrieve.ToList(),
                SingleResult = null
            };

            var loggerMock = new Mock<ILogger<StudentController>>();
            var studentMock = new Mock<IStudentService>();
            var userMock = new Mock<IUserService>();

            studentMock.Setup(x => x.GetAll(It.IsAny<int>(), It.IsAny<int>())).Returns(Task.FromResult(validationResult));
            userMock.Setup(x => x.ValidateToken(It.IsAny<string>())).Returns(Task.FromResult(true));

            var studentController = new StudentController(loggerMock.Object, studentMock.Object, userMock.Object);

            //ACT
            var controllerResult = studentController.GetAllStudents(token.ToString(), pageNumber, pageSize);

            //Assert
            Assert.IsNotNull(controllerResult);
            Assert.IsInstanceOfType(controllerResult.Result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public void When_Student_GetAll_Is_Authenticated_But_HasUnknowErrors_Returns_InternalServerError()
        {
            //Arrange
            var studentsToRetrieve = GetStudentDTOList();
            var pageNumber = 1;
            var pageSize = 50;
            var token = Guid.Empty;
            var validationResult = new ValidationResult<StudentDTO>()
            {
                CustomResultCode = (int)HttpStatusCode.OK,
                Errors = new List<string>()
                {
                    "Some Error Message"
                },
                ResultList = studentsToRetrieve.ToList(),
                SingleResult = null
            };

            var loggerMock = new Mock<ILogger<StudentController>>();
            var studentMock = new Mock<IStudentService>();
            var userMock = new Mock<IUserService>();

            studentMock.Setup(x => x.GetAll(It.IsAny<int>(), It.IsAny<int>())).Returns(Task.FromResult(validationResult));
            userMock.Setup(x => x.ValidateToken(It.IsAny<string>())).Returns(Task.FromResult(true));

            var studentController = new StudentController(loggerMock.Object, studentMock.Object, userMock.Object);

            //ACT
            var controllerResult = studentController.GetAllStudents(token.ToString(), pageNumber, pageSize);

            //Assert
            Assert.IsNotNull(controllerResult);
            Assert.IsInstanceOfType(controllerResult.Result, typeof(ObjectResult));
            Assert.AreEqual((int)HttpStatusCode.InternalServerError, (((ObjectResult)controllerResult.Result).StatusCode));
        }
        #endregion

        #region GetById
        [TestMethod]
        public void When_Student_GetById_Is_Authenticated_Returns_Data()
        {
            //Arrange
            var studentsToRetrieve = GetStudentDTOList();
            var studentId = studentsToRetrieve.Last().StudentId;
            var token = Guid.Empty;
            var validationResult = new ValidationResult<StudentDTO>()
            {
                CustomResultCode = 200,
                Errors = new List<string>(),
                ResultList = new List<StudentDTO>(),
                SingleResult = studentsToRetrieve.Last()
            };

            var loggerMock = new Mock<ILogger<StudentController>>();
            var studentMock = new Mock<IStudentService>();
            var userMock = new Mock<IUserService>();

            studentMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(Task.FromResult(validationResult));
            userMock.Setup(x => x.ValidateToken(It.IsAny<string>())).Returns(Task.FromResult(true));

            var studentController = new StudentController(loggerMock.Object, studentMock.Object, userMock.Object);

            //ACT
            var controllerResult = studentController.GetStudentById(token.ToString(), studentId);

            //Assert
            Assert.IsNotNull(controllerResult);
            Assert.IsInstanceOfType(controllerResult.Result, typeof(OkObjectResult));
            StudentDTO? retrievedStudentDTO = ((OkObjectResult)controllerResult.Result).Value as StudentDTO;
            Assert.AreEqual(retrievedStudentDTO.StudentId, studentsToRetrieve.Last().StudentId);
        }

        [TestMethod]
        public void When_Student_GetById_Is_Not_Authenticated_Returns_Unauthorized()
        {
            //Arrange
            var studentsToRetrieve = GetStudentDTOList();
            var studentId = studentsToRetrieve.Last().StudentId;
            var token = Guid.Empty;
            var validationResult = new ValidationResult<StudentDTO>()
            {
                CustomResultCode = 200,
                Errors = new List<string>(),
                ResultList = new List<StudentDTO>(),
                SingleResult = studentsToRetrieve.Last()
            };

            var loggerMock = new Mock<ILogger<StudentController>>();
            var studentMock = new Mock<IStudentService>();
            var userMock = new Mock<IUserService>();

            studentMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(Task.FromResult(validationResult));
            userMock.Setup(x => x.ValidateToken(It.IsAny<string>())).Returns(Task.FromResult(false));

            var studentController = new StudentController(loggerMock.Object, studentMock.Object, userMock.Object);

            //ACT
            var controllerResult = studentController.GetStudentById(token.ToString(), studentId);

            //Assert
            Assert.IsNotNull(controllerResult);
            Assert.IsInstanceOfType(controllerResult.Result, typeof(UnauthorizedResult));
        }

        [TestMethod]
        public void When_Student_GetById_Is_Authenticated_But_HasErrors_Returns_BadRequest()
        {
            //Arrange
            var studentsToRetrieve = GetStudentDTOList();
            var studentId = studentsToRetrieve.Last().StudentId;
            var token = Guid.Empty;
            var validationResult = new ValidationResult<StudentDTO>()
            {
                CustomResultCode = (int)HttpStatusCode.BadRequest,
                Errors = new List<string>()
                {
                    "Some Error Message"
                },
                ResultList = studentsToRetrieve.ToList(),
                SingleResult = null
            };

            var loggerMock = new Mock<ILogger<StudentController>>();
            var studentMock = new Mock<IStudentService>();
            var userMock = new Mock<IUserService>();

            studentMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(Task.FromResult(validationResult));
            userMock.Setup(x => x.ValidateToken(It.IsAny<string>())).Returns(Task.FromResult(true));

            var studentController = new StudentController(loggerMock.Object, studentMock.Object, userMock.Object);

            //ACT
            var controllerResult = studentController.GetStudentById(token.ToString(), studentId);

            //Assert
            Assert.IsNotNull(controllerResult);
            Assert.IsInstanceOfType(controllerResult.Result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public void When_Student_GetById_Is_Authenticated_But_HasUnknowErrors_Returns_InternalServerError()
        {
            //Arrange
            var studentsToRetrieve = GetStudentDTOList();
            var studentId = studentsToRetrieve.Last().StudentId;
            var token = Guid.Empty;
            var validationResult = new ValidationResult<StudentDTO>()
            {
                CustomResultCode = (int)HttpStatusCode.OK,
                Errors = new List<string>()
                {
                    "Some Error Message"
                },
                ResultList = studentsToRetrieve.ToList(),
                SingleResult = null
            };

            var loggerMock = new Mock<ILogger<StudentController>>();
            var studentMock = new Mock<IStudentService>();
            var userMock = new Mock<IUserService>();

            studentMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(Task.FromResult(validationResult));
            userMock.Setup(x => x.ValidateToken(It.IsAny<string>())).Returns(Task.FromResult(true));

            var studentController = new StudentController(loggerMock.Object, studentMock.Object, userMock.Object);

            //ACT
            var controllerResult = studentController.GetStudentById(token.ToString(), studentId);

            //Assert
            Assert.IsNotNull(controllerResult);
            Assert.IsInstanceOfType(controllerResult.Result, typeof(ObjectResult));
            Assert.AreEqual((int)HttpStatusCode.InternalServerError, (((ObjectResult)controllerResult.Result).StatusCode));
        }

        [TestMethod]
        public void When_Student_GetById_Is_Authenticated_And_NoData_Returns_NotFound()
        {
            //Arrange
            var studentsToRetrieve = GetStudentDTOList();
            var studentId = int.MaxValue;
            var token = Guid.Empty;
            var validationResult = new ValidationResult<StudentDTO>()
            {
                CustomResultCode = (int)HttpStatusCode.OK,
                Errors = new List<string>(),
                ResultList = new List<StudentDTO>(),
                SingleResult = null
            };

            var loggerMock = new Mock<ILogger<StudentController>>();
            var studentMock = new Mock<IStudentService>();
            var userMock = new Mock<IUserService>();

            studentMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(Task.FromResult(validationResult));
            userMock.Setup(x => x.ValidateToken(It.IsAny<string>())).Returns(Task.FromResult(true));

            var studentController = new StudentController(loggerMock.Object, studentMock.Object, userMock.Object);

            //ACT
            var controllerResult = studentController.GetStudentById(token.ToString(), studentId);

            //Assert
            Assert.IsNotNull(controllerResult);
            Assert.IsInstanceOfType(controllerResult.Result, typeof(NotFoundResult));
            //Assert.AreEqual((int)HttpStatusCode.InternalServerError, (((ObjectResult)controllerResult.Result).StatusCode));
        }
        #endregion

        #region Create

        [TestMethod]
        public void When_Student_Create_IsNot_Authenticated_Returns_UnAuthorized()
        {
            //Arrange
            var studentsToRetrieve = GetStudentDTOList();
            var studentId = studentsToRetrieve.Last().StudentId;
            var token = Guid.Empty;
            var validationResult = new ValidationResult<StudentDTO>()
            {
                CustomResultCode = 200,
                Errors = new List<string>(),
                ResultList = new List<StudentDTO>(),
                SingleResult = studentsToRetrieve.Last()
            };

            var studentCreateDTO = new StudentCreateDTO()
            {
                DocumentTypeId = 1,
                BirthDate = DateTime.Now,
                DocumentNumber = "123456",
                Names = "Names",
                LastNames = "Last Names"
            };

            var loggerMock = new Mock<ILogger<StudentController>>();
            var studentMock = new Mock<IStudentService>();
            var userMock = new Mock<IUserService>();

            studentMock.Setup(x => x.Create(It.IsAny<StudentCreateDTO>())).Returns(Task.FromResult(validationResult));
            userMock.Setup(x => x.ValidateToken(It.IsAny<string>())).Returns(Task.FromResult(false));

            var studentController = new StudentController(loggerMock.Object, studentMock.Object, userMock.Object);

            //ACT
            var controllerResult = studentController.CreateStudent(token.ToString(), studentCreateDTO);

            //Assert
            Assert.IsNotNull(controllerResult);
            Assert.IsInstanceOfType(controllerResult.Result, typeof(UnauthorizedResult));
        }

        [TestMethod]
        public void When_Student_Create_Is_Authenticated_Returns_Data_Then_Returns_OK()
        {
            //Arrange
            var studentsToRetrieve = GetStudentDTOList();
            var studentId = studentsToRetrieve.Last().StudentId;
            var token = Guid.Empty;
            var validationResult = new ValidationResult<StudentDTO>()
            {
                CustomResultCode = 200,
                Errors = new List<string>(),
                ResultList = new List<StudentDTO>(),
                SingleResult = studentsToRetrieve.Last()
            };

            var studentCreateDTO = new StudentCreateDTO()
            {
                DocumentTypeId = 1,
                BirthDate = DateTime.Now,
                DocumentNumber = "123456",
                Names = "Names",
                LastNames = "Last Names"
            };

            var loggerMock = new Mock<ILogger<StudentController>>();
            var studentMock = new Mock<IStudentService>();
            var userMock = new Mock<IUserService>();

            studentMock.Setup(x => x.Create(It.IsAny<StudentCreateDTO>())).Returns(Task.FromResult(validationResult));
            userMock.Setup(x => x.ValidateToken(It.IsAny<string>())).Returns(Task.FromResult(true));

            var studentController = new StudentController(loggerMock.Object, studentMock.Object, userMock.Object);

            //ACT
            var controllerResult = studentController.CreateStudent(token.ToString(), studentCreateDTO);

            //Assert
            Assert.IsNotNull(controllerResult);
            Assert.IsInstanceOfType(controllerResult.Result, typeof(CreatedAtActionResult));
        }

        [TestMethod]
        public void When_Student_GetById_Is_Authenticated_And_HasUnknownError_Returns_InternalServerError()
        {
            //Arrange
            var studentsToRetrieve = GetStudentDTOList();
            var studentId = studentsToRetrieve.Last().StudentId;
            var token = Guid.Empty;
            var validationResult = new ValidationResult<StudentDTO>()
            {
                CustomResultCode = (int)HttpStatusCode.OK,
                Errors = new List<string>()
                {
                    "Some BadRequest Error Message"
                },
                ResultList = studentsToRetrieve.ToList(),
                SingleResult = null
            };

            var loggerMock = new Mock<ILogger<StudentController>>();
            var studentMock = new Mock<IStudentService>();
            var userMock = new Mock<IUserService>();

            studentMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(Task.FromResult(validationResult));
            studentMock.Setup(x => x.Create(It.IsAny<StudentCreateDTO>())).Returns(Task.FromResult(validationResult));
            userMock.Setup(x => x.ValidateToken(It.IsAny<string>())).Returns(Task.FromResult(true));

            var studentController = new StudentController(loggerMock.Object, studentMock.Object, userMock.Object);

            //ACT
            var controllerResult = studentController.GetStudentById(token.ToString(), studentId);

            //Assert
            Assert.IsNotNull(controllerResult);
            Assert.IsInstanceOfType(controllerResult.Result, typeof(ObjectResult));
            Assert.AreEqual((int)HttpStatusCode.InternalServerError, (((ObjectResult)controllerResult.Result).StatusCode));
        }

        [TestMethod]
        public void When_Student_GetById_Is_Authenticated_And_Is_BadRequest_Returns_BadRequest()
        {
            //Arrange
            var studentsToRetrieve = GetStudentDTOList();
            var studentId = studentsToRetrieve.Last().StudentId;
            var token = Guid.Empty;
            var validationResult = new ValidationResult<StudentDTO>()
            {
                CustomResultCode = (int)HttpStatusCode.BadRequest,
                Errors = new List<string>()
                {
                    "Some BadRequest Error Message"
                },
                ResultList = studentsToRetrieve.ToList(),
                SingleResult = null
            };

            var studentCreateDTO = new StudentCreateDTO()
            {
                DocumentTypeId = 1,
                BirthDate = DateTime.Now,
                DocumentNumber = "123456",
                Names = "Names",
                LastNames = "Last Names"
            };

            var loggerMock = new Mock<ILogger<StudentController>>();
            var studentMock = new Mock<IStudentService>();
            var userMock = new Mock<IUserService>();

            studentMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(Task.FromResult(validationResult));
            studentMock.Setup(x => x.Create(It.IsAny<StudentCreateDTO>())).Returns(Task.FromResult(validationResult));
            userMock.Setup(x => x.ValidateToken(It.IsAny<string>())).Returns(Task.FromResult(true));

            var studentController = new StudentController(loggerMock.Object, studentMock.Object, userMock.Object);

            //ACT
            var controllerResult = studentController.CreateStudent(token.ToString(), studentCreateDTO);

            //Assert
            Assert.IsNotNull(controllerResult);
            Assert.IsInstanceOfType(controllerResult.Result, typeof(ObjectResult));
            Assert.AreEqual((int)HttpStatusCode.BadRequest, (((ObjectResult)controllerResult.Result).StatusCode));
        }

        [TestMethod]
        public void When_Student_Create_Is_Authenticated_And_HasUnknown_Error_Returns_InternalServerError()
        {
            //Arrange
            var studentsToRetrieve = GetStudentDTOList();
            var studentId = studentsToRetrieve.Last().StudentId;
            var token = Guid.Empty;
            var validationResult = new ValidationResult<StudentDTO>()
            {
                CustomResultCode = (int)HttpStatusCode.OK,
                Errors = new List<string>()
                {
                    "Some Error Message"
                },
                ResultList = studentsToRetrieve.ToList(),
                SingleResult = null
            };

            var loggerMock = new Mock<ILogger<StudentController>>();
            var studentMock = new Mock<IStudentService>();
            var userMock = new Mock<IUserService>();

            studentMock.Setup(x => x.Create(It.IsAny<StudentCreateDTO>())).Returns(Task.FromResult(validationResult));
            userMock.Setup(x => x.ValidateToken(It.IsAny<string>())).Returns(Task.FromResult(true));

            var studentController = new StudentController(loggerMock.Object, studentMock.Object, userMock.Object);

            var studentCreateDTO = new StudentCreateDTO()
            {
                DocumentTypeId = 1,
                BirthDate = DateTime.Now,
                DocumentNumber = "123456",
                Names = "Names",
                LastNames = "Last Names"
            };

            //ACT
            var controllerResult = studentController.CreateStudent(token.ToString(), studentCreateDTO);

            //Assert
            Assert.IsNotNull(controllerResult);
            Assert.IsInstanceOfType(controllerResult.Result, typeof(ObjectResult));
            Assert.AreEqual((int)HttpStatusCode.InternalServerError, (((ObjectResult)controllerResult.Result).StatusCode));
        }
        #endregion

        #region Update
        [TestMethod]
        public void When_Student_ChangePassword_Is_Authenticated_IsOK_Returns_NoContent()
        {
            //Arrange
            var studentsToRetrieve = GetStudentDTOList();
            var studentId = studentsToRetrieve.Last().StudentId;
            var token = Guid.Empty;
            var validationResult = new ValidationResult<bool>()
            {
                CustomResultCode = 200,
                Errors = new List<string>(),
                ResultList = new List<bool>(),
                SingleResult = true
            };

            var studentUpdateDTO = new StudentUpdateDTO()
            {
                DocumentTypeId = 4,
                DocumentNumber = "654321",
                Names = new Random().NextInt64().ToString(),
                LastNames = new Random().NextInt64().ToString(),
                BirthDate = DateTime.Now
            };

            var loggerMock = new Mock<ILogger<StudentController>>();
            var studentMock = new Mock<IStudentService>();
            var userMock = new Mock<IUserService>();

            studentMock.Setup(x => x.Update(It.IsAny<int>(), It.IsAny<StudentUpdateDTO>())).Returns(Task.FromResult(validationResult));
            userMock.Setup(x => x.ValidateToken(It.IsAny<string>())).Returns(Task.FromResult(true));

            var studentController = new StudentController(loggerMock.Object, studentMock.Object, userMock.Object);

            //ACT
            var controllerResult = studentController.ChangePassword(token.ToString(), studentId, studentUpdateDTO);

            //Assert
            Assert.IsNotNull(controllerResult);
            Assert.IsInstanceOfType(controllerResult.Result, typeof(NoContentResult));
        }

        [TestMethod]
        public void When_Student_ChangePassword_Is_NotAuthenticated_Returns_UnAuthorized()
        {
            //Arrange
            var studentsToRetrieve = GetStudentDTOList();
            var studentId = studentsToRetrieve.Last().StudentId;
            var token = Guid.Empty;
            var validationResult = new ValidationResult<bool>()
            {
                CustomResultCode = 200,
                Errors = new List<string>(),
                ResultList = new List<bool>(),
                SingleResult = true
            };

            var studentUpdateDTO = new StudentUpdateDTO()
            {
                DocumentTypeId = 4,
                DocumentNumber = "654321",
                Names = new Random().NextInt64().ToString(),
                LastNames = new Random().NextInt64().ToString(),
                BirthDate = DateTime.Now
            };

            var loggerMock = new Mock<ILogger<StudentController>>();
            var studentMock = new Mock<IStudentService>();
            var userMock = new Mock<IUserService>();

            studentMock.Setup(x => x.Update(It.IsAny<int>(), It.IsAny<StudentUpdateDTO>())).Returns(Task.FromResult(validationResult));
            userMock.Setup(x => x.ValidateToken(It.IsAny<string>())).Returns(Task.FromResult(false));

            var studentController = new StudentController(loggerMock.Object, studentMock.Object, userMock.Object);

            //ACT
            var controllerResult = studentController.ChangePassword(token.ToString(), studentId, studentUpdateDTO);

            //Assert
            Assert.IsNotNull(controllerResult);
            Assert.IsInstanceOfType(controllerResult.Result, typeof(UnauthorizedResult));
        }

        [TestMethod]
        public void When_Student_ChangePassword_Is_Authenticated_AndBadRequest_Returns_BadRequest()
        {
            //Arrange
            var studentsToRetrieve = GetStudentDTOList();
            var studentId = studentsToRetrieve.Last().StudentId;
            var token = Guid.Empty;
            var validationResult = new ValidationResult<bool>()
            {
                CustomResultCode = (int)HttpStatusCode.BadRequest,
                Errors = new List<string>() { "Some random error"},
                ResultList = new List<bool>(),
                SingleResult = true
            };

            var studentUpdateDTO = new StudentUpdateDTO()
            {
                DocumentTypeId = 4,
                DocumentNumber = "654321",
                Names = new Random().NextInt64().ToString(),
                LastNames = new Random().NextInt64().ToString(),
                BirthDate = DateTime.Now
            };

            var loggerMock = new Mock<ILogger<StudentController>>();
            var studentMock = new Mock<IStudentService>();
            var userMock = new Mock<IUserService>();

            studentMock.Setup(x => x.Update(It.IsAny<int>(), It.IsAny<StudentUpdateDTO>())).Returns(Task.FromResult(validationResult));
            userMock.Setup(x => x.ValidateToken(It.IsAny<string>())).Returns(Task.FromResult(true));

            var studentController = new StudentController(loggerMock.Object, studentMock.Object, userMock.Object);

            //ACT
            var controllerResult = studentController.ChangePassword(token.ToString(), studentId, studentUpdateDTO);

            //Assert
            Assert.IsNotNull(controllerResult);
            Assert.IsInstanceOfType(controllerResult.Result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public void When_Student_ChangePassword_Is_Authenticated_AndUnknownError_Returns_InternalServerError()
        {
            //Arrange
            var studentsToRetrieve = GetStudentDTOList();
            var studentId = studentsToRetrieve.Last().StudentId;
            var token = Guid.Empty;
            var validationResult = new ValidationResult<bool>()
            {
                CustomResultCode = (int)HttpStatusCode.InternalServerError,
                Errors = new List<string>() { "Some random error" },
                ResultList = new List<bool>(),
                SingleResult = true
            };

            var studentUpdateDTO = new StudentUpdateDTO()
            {
                DocumentTypeId = 4,
                DocumentNumber = "654321",
                Names = new Random().NextInt64().ToString(),
                LastNames = new Random().NextInt64().ToString(),
                BirthDate = DateTime.Now
            };

            var loggerMock = new Mock<ILogger<StudentController>>();
            var studentMock = new Mock<IStudentService>();
            var userMock = new Mock<IUserService>();

            studentMock.Setup(x => x.Update(It.IsAny<int>(), It.IsAny<StudentUpdateDTO>())).Returns(Task.FromResult(validationResult));
            userMock.Setup(x => x.ValidateToken(It.IsAny<string>())).Returns(Task.FromResult(true));

            var studentController = new StudentController(loggerMock.Object, studentMock.Object, userMock.Object);

            //ACT
            var controllerResult = studentController.ChangePassword(token.ToString(), studentId, studentUpdateDTO);

            //Assert
            Assert.IsNotNull(controllerResult);
            Assert.AreEqual(((ObjectResult)controllerResult.Result).StatusCode, 500);
            
        }
        #endregion

        #region Delete
        [TestMethod]
        public void When_Student_Delete_Is_Authenticated_IsOK_Returns_NoContent()
        {
            //Arrange
            var studentsToRetrieve = GetStudentDTOList();
            var studentId = studentsToRetrieve.Last().StudentId;
            var token = Guid.Empty;
            var validationResult = new ValidationResult<int>()
            {
                CustomResultCode = 200,
                Errors = new List<string>(),
                ResultList = new List<int>(),
                SingleResult = 0
            };

            var loggerMock = new Mock<ILogger<StudentController>>();
            var studentMock = new Mock<IStudentService>();
            var userMock = new Mock<IUserService>();

            studentMock.Setup(x => x.Delete(It.IsAny<int>())).Returns(Task.FromResult(validationResult));
            userMock.Setup(x => x.ValidateToken(It.IsAny<string>())).Returns(Task.FromResult(true));

            var studentController = new StudentController(loggerMock.Object, studentMock.Object, userMock.Object);

            //ACT
            var controllerResult = studentController.DeleteStudent(token.ToString(), studentId);

            //Assert
            Assert.IsNotNull(controllerResult);
            Assert.IsInstanceOfType(controllerResult.Result, typeof(NoContentResult));
        }
        [TestMethod]
        public void When_Student_Delete_Is_NotAuthenticated_Returns_UnAuthorized()
        {
            //Arrange
            var studentsToRetrieve = GetStudentDTOList();
            var studentId = studentsToRetrieve.Last().StudentId;
            var token = Guid.Empty;
            var validationResult = new ValidationResult<int>()
            {
                CustomResultCode = 200,
                Errors = new List<string>(),
                ResultList = new List<int>(),
                SingleResult = 0
            };

            var studentUpdateDTO = new StudentUpdateDTO()
            {
                DocumentTypeId = 4,
                DocumentNumber = "654321",
                Names = new Random().NextInt64().ToString(),
                LastNames = new Random().NextInt64().ToString(),
                BirthDate = DateTime.Now
            };

            var loggerMock = new Mock<ILogger<StudentController>>();
            var studentMock = new Mock<IStudentService>();
            var userMock = new Mock<IUserService>();

            studentMock.Setup(x => x.Delete(It.IsAny<int>())).Returns(Task.FromResult(validationResult));
            userMock.Setup(x => x.ValidateToken(It.IsAny<string>())).Returns(Task.FromResult(false));

            var studentController = new StudentController(loggerMock.Object, studentMock.Object, userMock.Object);

            //ACT
            var controllerResult = studentController.DeleteStudent(token.ToString(), studentId);

            //Assert
            Assert.IsNotNull(controllerResult);
            Assert.IsInstanceOfType(controllerResult.Result, typeof(UnauthorizedResult));
        }
        [TestMethod]
        public void When_Student_Delete_Is_Authenticated_AndBadRequest_Returns_BadRequest()
        {
            //Arrange
            var studentsToRetrieve = GetStudentDTOList();
            var studentId = studentsToRetrieve.Last().StudentId;
            var token = Guid.Empty;
            var validationResult = new ValidationResult<int>()
            {
                CustomResultCode = (int)HttpStatusCode.BadRequest,
                Errors = new List<string>() { "Some random error" },
                ResultList = new List<int>(),
                SingleResult = 0
            };

            var loggerMock = new Mock<ILogger<StudentController>>();
            var studentMock = new Mock<IStudentService>();
            var userMock = new Mock<IUserService>();

            studentMock.Setup(x => x.Delete(It.IsAny<int>())).Returns(Task.FromResult(validationResult));
            userMock.Setup(x => x.ValidateToken(It.IsAny<string>())).Returns(Task.FromResult(true));

            var studentController = new StudentController(loggerMock.Object, studentMock.Object, userMock.Object);

            //ACT
            var controllerResult = studentController.DeleteStudent(token.ToString(), studentId);

            //Assert
            Assert.IsNotNull(controllerResult);
            Assert.IsInstanceOfType(controllerResult.Result, typeof(BadRequestObjectResult));
        }
        public void When_Student_Delete_Is_Authenticated_AndBadRequest_ByNegativeId_Returns_BadRequest()
        {
            //Arrange
            var studentId = -1;
            var token = Guid.Empty;
            var validationResult = new ValidationResult<int>()
            {
                CustomResultCode = (int)HttpStatusCode.BadRequest,
                Errors = new List<string>() { "Some random error" },
                ResultList = new List<int>(),
                SingleResult = 0
            };

            var loggerMock = new Mock<ILogger<StudentController>>();
            var studentMock = new Mock<IStudentService>();
            var userMock = new Mock<IUserService>();

            studentMock.Setup(x => x.Delete(It.IsAny<int>())).Returns(Task.FromResult(validationResult));
            userMock.Setup(x => x.ValidateToken(It.IsAny<string>())).Returns(Task.FromResult(true));

            var studentController = new StudentController(loggerMock.Object, studentMock.Object, userMock.Object);

            //ACT
            var controllerResult = studentController.DeleteStudent(token.ToString(), studentId);

            //Assert
            Assert.IsNotNull(controllerResult);
            Assert.IsInstanceOfType(controllerResult.Result, typeof(BadRequestObjectResult));
        }
        [TestMethod]
        public void When_Student_Delete_Is_Authenticated_AndUnknownError_Returns_InternalServerError()
        {
            //Arrange
            var studentsToRetrieve = GetStudentDTOList();
            var studentId = studentsToRetrieve.Last().StudentId;
            var token = Guid.Empty;
            var validationResult = new ValidationResult<int>()
            {
                CustomResultCode = (int)HttpStatusCode.InternalServerError,
                Errors = new List<string>() { "Some random error" },
                ResultList = new List<int>(),
                SingleResult = 0
            };

            var loggerMock = new Mock<ILogger<StudentController>>();
            var studentMock = new Mock<IStudentService>();
            var userMock = new Mock<IUserService>();

            studentMock.Setup(x => x.Delete(It.IsAny<int>())).Returns(Task.FromResult(validationResult));
            userMock.Setup(x => x.ValidateToken(It.IsAny<string>())).Returns(Task.FromResult(true));

            var studentController = new StudentController(loggerMock.Object, studentMock.Object, userMock.Object);

            //ACT
            var controllerResult = studentController.DeleteStudent(token.ToString(), studentId);

            //Assert
            Assert.IsNotNull(controllerResult);
            Assert.AreEqual(((ObjectResult)controllerResult.Result).StatusCode, 500);

        }
        #endregion

        private static List<StudentDTO> GetStudentDTOList()
        {
            return new List<StudentDTO>()
            {
                new StudentDTO
                {
                   StudentId= 20_000,
                   DocumentTypeId = (int)EDocumentType.IdCard,
                   DocumentNumber = "20000",
                   Names = "Name 20000",
                   LastNames = "Last names 20000",
                   BirthDate = DateTime.Now.AddYears(-15),
                   Enabled = true
                },
                new StudentDTO
                {
                   StudentId= 20_001,
                   DocumentTypeId = (int)EDocumentType.NationalId,
                   DocumentNumber = "20001",
                   Names = "Name 20001",
                   LastNames = "Last names 20001",
                   BirthDate = DateTime.Now.AddYears(-18),
                   Enabled = true
                },
                new StudentDTO
                {
                   StudentId= 20_002,
                   DocumentTypeId = (int)EDocumentType.ForeignId,
                   DocumentNumber = "20002",
                   Names = "Name 20002",
                   LastNames = "Last names 20002",
                   BirthDate = DateTime.Now.AddYears(-20),
                   Enabled = true
                },
                new StudentDTO
                {
                   StudentId= 20_002,
                   DocumentTypeId = (int)EDocumentType.Passport,
                   DocumentNumber = "20003",
                   Names = "Name 20003",
                   LastNames = "Last names 20003",
                   BirthDate = DateTime.Now.AddYears(-50),
                   Enabled = true
                },
            };
        }
    }
}