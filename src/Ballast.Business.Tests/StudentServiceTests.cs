using System.Net;
using Ballast.Business.Repository.Interfaces;
using Ballast.Business.Services;
using Ballast.Business.Services.Interfaces;
using Ballast.Business.Validation;
using Ballast.DTO;
using Microsoft.Extensions.Logging;
using Moq;

namespace Ballast.Business.Tests
{
    [TestClass]
    public class StudentServiceTests
    {
        #region GetAll
        [TestMethod]
        public void When_Student_GetAll_Returns_Data_And_Has_SameRecordCount_As_Source()
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

            var studentRepositoryMock = new Mock<IStudentRepository>();
            studentRepositoryMock.Setup(x => x.GetAll(It.IsAny<int>(), It.IsAny<int>())).Returns(Task.FromResult(studentsToRetrieve));

            var studentService = new StudentService(studentRepositoryMock.Object);

            //ACT
            var serviceResult = studentService.GetAll(pageNumber, pageSize);

            //Assert
            Assert.IsNotNull(serviceResult);
            Assert.IsInstanceOfType(serviceResult.Result, typeof(ValidationResult<StudentDTO>));
            Assert.IsNotNull(serviceResult.Result.ResultList);
            Assert.AreEqual(studentsToRetrieve.Count, serviceResult.Result.ResultList.Count);
        }

        [TestMethod]
        public void When_Student_GetAll_ThrowsException_ReturnsInternalErrorCode()
        {
            //Arrange
            var pageNumber = 1;
            var pageSize = 50;
            var token = Guid.Empty;
            var validationResult = new ValidationResult<StudentDTO>()
            {
                CustomResultCode = 500,
                Errors = new List<string>() { "Some random text" },
                ResultList = new List<StudentDTO>(),
                SingleResult = null
            };

            var studentRepositoryMock = new Mock<IStudentRepository>();
            studentRepositoryMock.Setup(x => x.GetAll(It.IsAny<int>(), It.IsAny<int>())).Throws(new Exception());

            var studentService = new StudentService(studentRepositoryMock.Object);

            //ACT
            var serviceResult = studentService.GetAll(pageNumber, pageSize);

            //Assert
            Assert.IsNotNull(serviceResult);
            Assert.IsInstanceOfType(serviceResult.Result, typeof(ValidationResult<StudentDTO>));
            Assert.IsTrue(serviceResult.Result.Errors.Any());
            Assert.AreEqual(500, serviceResult.Result.CustomResultCode);
        }

        #endregion

        #region GetById
        [TestMethod]
        public void When_Student_GetById_Returns_Data_And_Has_Same_StudentId_As_Source()
        {
            //Arrange
            var studentsToRetrieve = GetStudentDTOList();
            var returnedStudent = studentsToRetrieve.Last();
            var studentId = returnedStudent.StudentId;
            var token = Guid.Empty;
            var validationResult = new ValidationResult<StudentDTO>()
            {
                CustomResultCode = 200,
                Errors = new List<string>(),
                ResultList = new List<StudentDTO>(),
                SingleResult = null
            };

            var studentRepositoryMock = new Mock<IStudentRepository>();
            studentRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(returnedStudent));

            var studentService = new StudentService(studentRepositoryMock.Object);

            //ACT
            var serviceResult = studentService.GetById(studentId);

            //Assert
            Assert.IsNotNull(serviceResult);
            Assert.IsInstanceOfType(serviceResult.Result, typeof(ValidationResult<StudentDTO>));
            Assert.IsNotNull(serviceResult.Result.SingleResult);
            Assert.AreEqual(returnedStudent.StudentId, returnedStudent.StudentId);
        }

        [TestMethod]
        public void When_Student_GetById_BadRequestById_ReturnsBadRequest()
        {
            //Arrange
            var studentId = -1;

            var token = Guid.Empty;
            var validationResult = new ValidationResult<StudentDTO>()
            {
                CustomResultCode = 200,
                Errors = new List<string>(),
                ResultList = new List<StudentDTO>(),
                SingleResult = null
            };

            var studentRepositoryMock = new Mock<IStudentRepository>();
            var studentService = new StudentService(studentRepositoryMock.Object);

            //ACT
            var serviceResult = studentService.GetById(studentId);

            //Assert
            Assert.IsNotNull(serviceResult);
            Assert.IsInstanceOfType(serviceResult.Result, typeof(ValidationResult<StudentDTO>));
            Assert.AreEqual((int)HttpStatusCode.BadRequest, serviceResult.Result.CustomResultCode);
        }

        [TestMethod]
        public void When_Student_GetById_ReturnsNull_Then_ReturnsNotFound()
        {
            //Arrange
            var studentsToRetrieve = GetStudentDTOList();
            StudentDTO returnedStudent = null;
            var studentId = studentsToRetrieve.Last().StudentId;
            var token = Guid.Empty;
            var validationResult = new ValidationResult<StudentDTO>()
            {
                CustomResultCode = 200,
                Errors = new List<string>(),
                ResultList = new List<StudentDTO>(),
                SingleResult = null
            };

            var studentRepositoryMock = new Mock<IStudentRepository>();
            studentRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(returnedStudent));

            var studentService = new StudentService(studentRepositoryMock.Object);

            //ACT
            var serviceResult = studentService.GetById(studentId);

            //Assert
            Assert.IsNotNull(serviceResult);
            Assert.IsInstanceOfType(serviceResult.Result, typeof(ValidationResult<StudentDTO>));
            Assert.AreEqual((int)HttpStatusCode.NotFound, serviceResult.Result.CustomResultCode);
        }

        [TestMethod]
        public void When_Student_GetById_ThrowsException_ReturnsInternalErrorCode()
        {
            //Arrange
            var studentsToRetrieve = GetStudentDTOList();
            var studentId = studentsToRetrieve.Last().StudentId;
            var token = Guid.Empty;
            var validationResult = new ValidationResult<StudentDTO>()
            {
                CustomResultCode = 500,
                Errors = new List<string>() { "Some random text" },
                ResultList = new List<StudentDTO>(),
                SingleResult = null
            };

            var studentRepositoryMock = new Mock<IStudentRepository>();
            studentRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).Throws(new Exception());

            var studentService = new StudentService(studentRepositoryMock.Object);

            //ACT
            var serviceResult = studentService.GetById(studentId);

            //Assert
            Assert.IsNotNull(serviceResult);
            Assert.IsInstanceOfType(serviceResult.Result, typeof(ValidationResult<StudentDTO>));
            Assert.IsTrue(serviceResult.Result.Errors.Any());
            Assert.AreEqual(500, serviceResult.Result.CustomResultCode);
        }
        #endregion

        #region Create
        [TestMethod]
        public void When_Student_Create_IsValid_AndNoErrors_Returns_Converted_StudentDTO()
        {
            //Arrange
            var token = Guid.Empty;
            var validationResult = new ValidationResult<StudentDTO>()
            {
                CustomResultCode = 500,
                Errors = new List<string>() { "Some random error message" },
                ResultList = new List<StudentDTO>(),
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

            var studentRepositoryMock = new Mock<IStudentRepository>();
            var studentToRetrieve = studentCreateDTO.ConvertToStudentDTO();
            studentRepositoryMock.Setup(x => x.Add(It.IsAny<StudentDTO>())).Returns(Task.FromResult(studentToRetrieve));

            var studentService = new StudentService(studentRepositoryMock.Object);

            //ACT
            var serviceResult = studentService.Create(studentCreateDTO);

            //Assert
            Assert.IsNotNull(serviceResult);
            Assert.IsInstanceOfType(serviceResult.Result, typeof(ValidationResult<StudentDTO>));
            Assert.IsNotNull(serviceResult.Result.SingleResult);
            Assert.AreEqual(serviceResult.Result.SingleResult.StudentId, studentToRetrieve.StudentId);
        }

        [TestMethod]
        public void When_Student_Create_IsInValid_ByDocumentTypeId_AndErrors_Returns_Errors()
        {
            //Arrange
            var studentsToRetrieve = GetStudentDTOList();
            var token = Guid.Empty;
            var validationResult = new ValidationResult<StudentDTO>()
            {
                CustomResultCode = 500,
                Errors = new List<string>(),
                ResultList = new List<StudentDTO>(),
                SingleResult = null
            };

            var studentCreateDTO = new StudentCreateDTO()
            {
                DocumentTypeId = -1,
                BirthDate = DateTime.Now,
                DocumentNumber = "123456",
                Names = "Names",
                LastNames = "Last Names"
            };

            var studentRepositoryMock = new Mock<IStudentRepository>();
            var studentToRetrieve = studentCreateDTO.ConvertToStudentDTO();
            studentRepositoryMock.Setup(x => x.Add(It.IsAny<StudentDTO>())).Returns(Task.FromResult(studentToRetrieve));

            var studentService = new StudentService(studentRepositoryMock.Object);

            //ACT
            var serviceResult = studentService.Create(studentCreateDTO);

            //Assert
            Assert.IsNotNull(serviceResult);
            Assert.IsInstanceOfType(serviceResult.Result, typeof(ValidationResult<StudentDTO>));
            Assert.IsTrue(serviceResult.Result.Errors.Any());
        }

        [TestMethod]
        public void When_Student_Create_IsInValid_DocumentNumber_AndErrors_Returns_Errors()
        {
            //Arrange
            var studentsToRetrieve = GetStudentDTOList();
            var token = Guid.Empty;
            var validationResult = new ValidationResult<StudentDTO>()
            {
                CustomResultCode = 500,
                Errors = new List<string>(),
                ResultList = new List<StudentDTO>(),
                SingleResult = null
            };

            var studentCreateDTO = new StudentCreateDTO()
            {
                DocumentTypeId = -1,
                BirthDate = DateTime.Now,
                DocumentNumber = null,
                Names = "Names",
                LastNames = "Last Names"
            };

            var studentRepositoryMock = new Mock<IStudentRepository>();
            var studentToRetrieve = studentCreateDTO.ConvertToStudentDTO();
            studentRepositoryMock.Setup(x => x.Add(It.IsAny<StudentDTO>())).Returns(Task.FromResult(studentToRetrieve));

            var studentService = new StudentService(studentRepositoryMock.Object);

            //ACT
            var serviceResult = studentService.Create(studentCreateDTO);

            //Assert
            Assert.IsNotNull(serviceResult);
            Assert.IsInstanceOfType(serviceResult.Result, typeof(ValidationResult<StudentDTO>));
            Assert.IsTrue(serviceResult.Result.Errors.Any());
        }

        [TestMethod]
        public void When_Student_Create_IsInValid_BYyNullDocumentNumber_AndErrors_Returns_Errors()
        {
            //Arrange
            var studentsToRetrieve = GetStudentDTOList();
            var token = Guid.Empty;
            var validationResult = new ValidationResult<StudentDTO>()
            {
                CustomResultCode = 500,
                Errors = new List<string>(),
                ResultList = new List<StudentDTO>(),
                SingleResult = null
            };

            var studentCreateDTO = new StudentCreateDTO()
            {
                DocumentTypeId = 1,
                BirthDate = DateTime.Now,
                DocumentNumber = null,
                Names = "Names",
                LastNames = "Last Names"
            };

            var studentRepositoryMock = new Mock<IStudentRepository>();
            var studentToRetrieve = studentCreateDTO.ConvertToStudentDTO();
            studentRepositoryMock.Setup(x => x.Add(It.IsAny<StudentDTO>())).Returns(Task.FromResult(studentToRetrieve));

            var studentService = new StudentService(studentRepositoryMock.Object);

            //ACT
            var serviceResult = studentService.Create(studentCreateDTO);

            //Assert
            Assert.IsNotNull(serviceResult);
            Assert.IsInstanceOfType(serviceResult.Result, typeof(ValidationResult<StudentDTO>));
            Assert.IsTrue(serviceResult.Result.Errors.Any());
        }

        [TestMethod]
        public void When_Student_Create_IsInValid_ByShortDocumentNumber_AndErrors_Returns_Errors()
        {
            //Arrange
            var studentsToRetrieve = GetStudentDTOList();
            var token = Guid.Empty;
            var validationResult = new ValidationResult<StudentDTO>()
            {
                CustomResultCode = 500,
                Errors = new List<string>(),
                ResultList = new List<StudentDTO>(),
                SingleResult = null
            };

            var studentCreateDTO = new StudentCreateDTO()
            {
                DocumentTypeId = 1,
                BirthDate = DateTime.Now,
                DocumentNumber = "1",
                Names = "Names",
                LastNames = "Last Names"
            };

            var studentRepositoryMock = new Mock<IStudentRepository>();
            var studentToRetrieve = studentCreateDTO.ConvertToStudentDTO();
            studentRepositoryMock.Setup(x => x.Add(It.IsAny<StudentDTO>())).Returns(Task.FromResult(studentToRetrieve));

            var studentService = new StudentService(studentRepositoryMock.Object);

            //ACT
            var serviceResult = studentService.Create(studentCreateDTO);

            //Assert
            Assert.IsNotNull(serviceResult);
            Assert.IsInstanceOfType(serviceResult.Result, typeof(ValidationResult<StudentDTO>));
            Assert.IsTrue(serviceResult.Result.Errors.Any());
        }

        [TestMethod]
        public void When_Student_Create_IsInValid_ByToLongDocumentNumber_AndErrors_Returns_Errors()
        {
            //Arrange
            var studentsToRetrieve = GetStudentDTOList();
            var token = Guid.Empty;
            var validationResult = new ValidationResult<StudentDTO>()
            {
                CustomResultCode = 500,
                Errors = new List<string>(),
                ResultList = new List<StudentDTO>(),
                SingleResult = null
            };

            var studentCreateDTO = new StudentCreateDTO()
            {
                DocumentTypeId = 1,
                BirthDate = DateTime.Now,
                DocumentNumber = "123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890",
                Names = "Names",
                LastNames = "Last Names"
            };

            var studentRepositoryMock = new Mock<IStudentRepository>();
            var studentToRetrieve = studentCreateDTO.ConvertToStudentDTO();
            studentRepositoryMock.Setup(x => x.Add(It.IsAny<StudentDTO>())).Returns(Task.FromResult(studentToRetrieve));

            var studentService = new StudentService(studentRepositoryMock.Object);

            //ACT
            var serviceResult = studentService.Create(studentCreateDTO);

            //Assert
            Assert.IsNotNull(serviceResult);
            Assert.IsInstanceOfType(serviceResult.Result, typeof(ValidationResult<StudentDTO>));
            Assert.IsTrue(serviceResult.Result.Errors.Any());
        }

        [TestMethod]
        public void When_Student_Create_IsInValid_ByNullNames_AndErrors_Returns_Errors()
        {
            //Arrange
            var studentsToRetrieve = GetStudentDTOList();
            var token = Guid.Empty;
            var validationResult = new ValidationResult<StudentDTO>()
            {
                CustomResultCode = 500,
                Errors = new List<string>(),
                ResultList = new List<StudentDTO>(),
                SingleResult = null
            };

            var studentCreateDTO = new StudentCreateDTO()
            {
                DocumentTypeId = 1,
                BirthDate = DateTime.Now,
                DocumentNumber = "123456",
                Names = null,
                LastNames = "Last Names"
            };

            var studentRepositoryMock = new Mock<IStudentRepository>();
            var studentToRetrieve = studentCreateDTO.ConvertToStudentDTO();
            studentRepositoryMock.Setup(x => x.Add(It.IsAny<StudentDTO>())).Returns(Task.FromResult(studentToRetrieve));

            var studentService = new StudentService(studentRepositoryMock.Object);

            //ACT
            var serviceResult = studentService.Create(studentCreateDTO);

            //Assert
            Assert.IsNotNull(serviceResult);
            Assert.IsInstanceOfType(serviceResult.Result, typeof(ValidationResult<StudentDTO>));
            Assert.IsTrue(serviceResult.Result.Errors.Any());
        }

        [TestMethod]
        public void When_Student_Create_IsInValid_ByToShortNames_AndErrors_Returns_Errors()
        {
            //Arrange
            var studentsToRetrieve = GetStudentDTOList();
            var token = Guid.Empty;
            var validationResult = new ValidationResult<StudentDTO>()
            {
                CustomResultCode = 500,
                Errors = new List<string>(),
                ResultList = new List<StudentDTO>(),
                SingleResult = null
            };

            var studentCreateDTO = new StudentCreateDTO()
            {
                DocumentTypeId = 1,
                BirthDate = DateTime.Now,
                DocumentNumber = "123456",
                Names = "1",
                LastNames = "Last Names"
            };

            var studentRepositoryMock = new Mock<IStudentRepository>();
            var studentToRetrieve = studentCreateDTO.ConvertToStudentDTO();
            studentRepositoryMock.Setup(x => x.Add(It.IsAny<StudentDTO>())).Returns(Task.FromResult(studentToRetrieve));

            var studentService = new StudentService(studentRepositoryMock.Object);

            //ACT
            var serviceResult = studentService.Create(studentCreateDTO);

            //Assert
            Assert.IsNotNull(serviceResult);
            Assert.IsInstanceOfType(serviceResult.Result, typeof(ValidationResult<StudentDTO>));
            Assert.IsTrue(serviceResult.Result.Errors.Any());
        }

        [TestMethod]
        public void When_Student_Create_IsInValid_ByToLongNames_AndErrors_Returns_Errors()
        {
            //Arrange
            var studentsToRetrieve = GetStudentDTOList();
            var token = Guid.Empty;
            var validationResult = new ValidationResult<StudentDTO>()
            {
                CustomResultCode = 500,
                Errors = new List<string>(),
                ResultList = new List<StudentDTO>(),
                SingleResult = null
            };

            var studentCreateDTO = new StudentCreateDTO()
            {
                DocumentTypeId = 1,
                BirthDate = DateTime.Now,
                DocumentNumber = "123456",
                Names = "12345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890",
                LastNames = "Last Names"
            };

            var studentRepositoryMock = new Mock<IStudentRepository>();
            var studentToRetrieve = studentCreateDTO.ConvertToStudentDTO();
            studentRepositoryMock.Setup(x => x.Add(It.IsAny<StudentDTO>())).Returns(Task.FromResult(studentToRetrieve));

            var studentService = new StudentService(studentRepositoryMock.Object);

            //ACT
            var serviceResult = studentService.Create(studentCreateDTO);

            //Assert
            Assert.IsNotNull(serviceResult);
            Assert.IsInstanceOfType(serviceResult.Result, typeof(ValidationResult<StudentDTO>));
            Assert.IsTrue(serviceResult.Result.Errors.Any());
        }

        [TestMethod]
        public void When_Student_Create_IsInValid_ByNullLastNames_AndErrors_Returns_Errors()
        {
            //Arrange
            var studentsToRetrieve = GetStudentDTOList();
            var token = Guid.Empty;
            var validationResult = new ValidationResult<StudentDTO>()
            {
                CustomResultCode = 500,
                Errors = new List<string>(),
                ResultList = new List<StudentDTO>(),
                SingleResult = null
            };

            var studentCreateDTO = new StudentCreateDTO()
            {
                DocumentTypeId = 1,
                BirthDate = DateTime.Now,
                DocumentNumber = "123456",
                Names = "123456",
                LastNames = null
            };

            var studentRepositoryMock = new Mock<IStudentRepository>();
            var studentToRetrieve = studentCreateDTO.ConvertToStudentDTO();
            studentRepositoryMock.Setup(x => x.Add(It.IsAny<StudentDTO>())).Returns(Task.FromResult(studentToRetrieve));

            var studentService = new StudentService(studentRepositoryMock.Object);

            //ACT
            var serviceResult = studentService.Create(studentCreateDTO);

            //Assert
            Assert.IsNotNull(serviceResult);
            Assert.IsInstanceOfType(serviceResult.Result, typeof(ValidationResult<StudentDTO>));
            Assert.IsTrue(serviceResult.Result.Errors.Any());
        }

        [TestMethod]
        public void When_Student_Create_IsInValid_ByShortLastNames_AndErrors_Returns_Errors()
        {
            //Arrange
            var studentsToRetrieve = GetStudentDTOList();
            var token = Guid.Empty;
            var validationResult = new ValidationResult<StudentDTO>()
            {
                CustomResultCode = 500,
                Errors = new List<string>(),
                ResultList = new List<StudentDTO>(),
                SingleResult = null
            };

            var studentCreateDTO = new StudentCreateDTO()
            {
                DocumentTypeId = 1,
                BirthDate = DateTime.Now,
                DocumentNumber = "123456",
                Names = "123456",
                LastNames = "1"
            };

            var studentRepositoryMock = new Mock<IStudentRepository>();
            var studentToRetrieve = studentCreateDTO.ConvertToStudentDTO();
            studentRepositoryMock.Setup(x => x.Add(It.IsAny<StudentDTO>())).Returns(Task.FromResult(studentToRetrieve));

            var studentService = new StudentService(studentRepositoryMock.Object);

            //ACT
            var serviceResult = studentService.Create(studentCreateDTO);

            //Assert
            Assert.IsNotNull(serviceResult);
            Assert.IsInstanceOfType(serviceResult.Result, typeof(ValidationResult<StudentDTO>));
            Assert.IsTrue(serviceResult.Result.Errors.Any());
        }

        [TestMethod]
        public void When_Student_Create_ThrowsException_Returns_Errors_WithInternalServerError()
        {
            //Arrange
            var studentsToRetrieve = GetStudentDTOList();
            var token = Guid.Empty;
            var validationResult = new ValidationResult<StudentDTO>()
            {
                CustomResultCode = 500,
                Errors = new List<string>(),
                ResultList = new List<StudentDTO>(),
                SingleResult = null
            };

            var studentCreateDTO = new StudentCreateDTO()
            {
                DocumentTypeId = 1,
                BirthDate = DateTime.Now,
                DocumentNumber = "123456",
                Names = "123456",
                LastNames = "BlaBlaBla"
            };

            var studentRepositoryMock = new Mock<IStudentRepository>();
            var studentToRetrieve = studentCreateDTO.ConvertToStudentDTO();
            studentRepositoryMock.Setup(x => x.Add(It.IsAny<StudentDTO>())).Throws(new Exception());

            var studentService = new StudentService(studentRepositoryMock.Object);

            //ACT
            var serviceResult = studentService.Create(studentCreateDTO);

            //Assert
            Assert.IsNotNull(serviceResult);
            Assert.IsInstanceOfType(serviceResult.Result, typeof(ValidationResult<StudentDTO>));
            Assert.IsTrue(serviceResult.Result.Errors.Any());
            Assert.AreEqual((int)HttpStatusCode.InternalServerError, serviceResult.Result.CustomResultCode);
        }

        #endregion

        #region Update
        [TestMethod]
        public void When_Student_Update_IsValid_AndNoErrors_Returns_Converted_StudentDTO()
        {
            //Arrange
            var studentsToRetrieve = GetStudentDTOList();
            var studentToRetrieve = studentsToRetrieve.Last();
            var studentId = studentToRetrieve.StudentId;
            var token = Guid.Empty;
            var validationResult = new ValidationResult<StudentDTO>()
            {
                CustomResultCode = 500,
                Errors = new List<string>() { "Some random error message" },
                ResultList = new List<StudentDTO>(),
                SingleResult = null
            };

            var studentUpdateDTO = new StudentUpdateDTO()
            {
                DocumentTypeId = 1,
                BirthDate = DateTime.Now,
                DocumentNumber = "123456",
                Names = "Names",
                LastNames = "Last Names",
                Enabled = false
            };

            var studentRepositoryMock = new Mock<IStudentRepository>();
            studentRepositoryMock.Setup(x => x.UpdateAsync(studentId, It.IsAny<StudentUpdateDTO>())).Returns(Task.FromResult(true));

            var studentService = new StudentService(studentRepositoryMock.Object);

            //ACT
            var serviceResult = studentService.Update(studentId, studentUpdateDTO);

            //Assert
            Assert.IsNotNull(serviceResult);
            Assert.IsInstanceOfType(serviceResult.Result, typeof(ValidationResult<bool>));
        }
        #endregion

        #region Update
        [TestMethod]
        public void When_Student_Update_IsInvalidByNegativeDocumentTypeId_AndErrors_Returns_BadRequest()
        {
            //Arrange
            var studentsToRetrieve = GetStudentDTOList();
            var studentToRetrieve = studentsToRetrieve.Last();
            var studentId = studentToRetrieve.StudentId;
            var token = Guid.Empty;
            var validationResult = new ValidationResult<StudentDTO>()
            {
                CustomResultCode = 500,
                Errors = new List<string>() { "Some random error message" },
                ResultList = new List<StudentDTO>(),
                SingleResult = null
            };

            var studentUpdateDTO = new StudentUpdateDTO()
            {
                DocumentTypeId = -1,
                BirthDate = DateTime.Now,
                DocumentNumber = "123456",
                Names = "Names",
                LastNames = "Last Names",
                Enabled = false
            };

            var studentRepositoryMock = new Mock<IStudentRepository>();
            studentRepositoryMock.Setup(x => x.UpdateAsync(studentId, It.IsAny<StudentUpdateDTO>())).Returns(Task.FromResult(true));

            var studentService = new StudentService(studentRepositoryMock.Object);

            //ACT
            var serviceResult = studentService.Update(studentId, studentUpdateDTO);

            //Assert
            Assert.IsNotNull(serviceResult);
            Assert.IsInstanceOfType(serviceResult.Result, typeof(ValidationResult<bool>));
            Assert.IsTrue(serviceResult.Result.Errors.Any());
        }

        [TestMethod]
        public void When_Student_Update_IsInvalidByNullDocumentNumber_AndErrors_Returns_BadRequest()
        {
            //Arrange
            var studentsToRetrieve = GetStudentDTOList();
            var studentToRetrieve = studentsToRetrieve.Last();
            var studentId = studentToRetrieve.StudentId;
            var token = Guid.Empty;
            var validationResult = new ValidationResult<StudentDTO>()
            {
                CustomResultCode = 500,
                Errors = new List<string>() { "Some random error message" },
                ResultList = new List<StudentDTO>(),
                SingleResult = null
            };

            var studentUpdateDTO = new StudentUpdateDTO()
            {
                DocumentTypeId = 1,
                BirthDate = DateTime.Now,
                DocumentNumber = null,
                Names = "Names",
                LastNames = "Last Names",
                Enabled = false
            };

            var studentRepositoryMock = new Mock<IStudentRepository>();
            studentRepositoryMock.Setup(x => x.UpdateAsync(studentId, It.IsAny<StudentUpdateDTO>())).Returns(Task.FromResult(true));

            var studentService = new StudentService(studentRepositoryMock.Object);

            //ACT
            var serviceResult = studentService.Update(studentId, studentUpdateDTO);

            //Assert
            Assert.IsNotNull(serviceResult);
            Assert.IsInstanceOfType(serviceResult.Result, typeof(ValidationResult<bool>));
            Assert.IsTrue(serviceResult.Result.Errors.Any());
        }

        [TestMethod]
        public void When_Student_Update_IsInvalidByShortDocumentNumber_AndErrors_Returns_BadRequest()
        {
            //Arrange
            var studentsToRetrieve = GetStudentDTOList();
            var studentToRetrieve = studentsToRetrieve.Last();
            var studentId = studentToRetrieve.StudentId;
            var token = Guid.Empty;
            var validationResult = new ValidationResult<StudentDTO>()
            {
                CustomResultCode = 500,
                Errors = new List<string>() { "Some random error message" },
                ResultList = new List<StudentDTO>(),
                SingleResult = null
            };

            var studentUpdateDTO = new StudentUpdateDTO()
            {
                DocumentTypeId = 1,
                BirthDate = DateTime.Now,
                DocumentNumber = "1",
                Names = "Names",
                LastNames = "Last Names",
                Enabled = false
            };

            var studentRepositoryMock = new Mock<IStudentRepository>();
            studentRepositoryMock.Setup(x => x.UpdateAsync(studentId, It.IsAny<StudentUpdateDTO>())).Returns(Task.FromResult(true));

            var studentService = new StudentService(studentRepositoryMock.Object);

            //ACT
            var serviceResult = studentService.Update(studentId, studentUpdateDTO);

            //Assert
            Assert.IsNotNull(serviceResult);
            Assert.IsInstanceOfType(serviceResult.Result, typeof(ValidationResult<bool>));
            Assert.IsTrue(serviceResult.Result.Errors.Any());
        }

        [TestMethod]
        public void When_Student_Update_IsInvalidByLongDocumentNumber_AndErrors_Returns_BadRequest()
        {
            //Arrange
            var studentsToRetrieve = GetStudentDTOList();
            var studentToRetrieve = studentsToRetrieve.Last();
            var studentId = studentToRetrieve.StudentId;
            var token = Guid.Empty;
            var validationResult = new ValidationResult<StudentDTO>()
            {
                CustomResultCode = 500,
                Errors = new List<string>() { "Some random error message" },
                ResultList = new List<StudentDTO>(),
                SingleResult = null
            };

            var studentUpdateDTO = new StudentUpdateDTO()
            {
                DocumentTypeId = 1,
                BirthDate = DateTime.Now,
                DocumentNumber = "100000000000000000000000000000000",
                Names = "Names",
                LastNames = "Last Names",
                Enabled = false
            };

            var studentRepositoryMock = new Mock<IStudentRepository>();
            studentRepositoryMock.Setup(x => x.UpdateAsync(studentId, It.IsAny<StudentUpdateDTO>())).Returns(Task.FromResult(true));

            var studentService = new StudentService(studentRepositoryMock.Object);

            //ACT
            var serviceResult = studentService.Update(studentId, studentUpdateDTO);

            //Assert
            Assert.IsNotNull(serviceResult);
            Assert.IsInstanceOfType(serviceResult.Result, typeof(ValidationResult<bool>));
            Assert.IsTrue(serviceResult.Result.Errors.Any());
        }

        [TestMethod]
        public void When_Student_Update_IsInvalidByNullNames_AndErrors_Returns_BadRequest()
        {
            //Arrange
            var studentsToRetrieve = GetStudentDTOList();
            var studentToRetrieve = studentsToRetrieve.Last();
            var studentId = studentToRetrieve.StudentId;
            var token = Guid.Empty;
            var validationResult = new ValidationResult<StudentDTO>()
            {
                CustomResultCode = 500,
                Errors = new List<string>() { "Some random error message" },
                ResultList = new List<StudentDTO>(),
                SingleResult = null
            };

            var studentUpdateDTO = new StudentUpdateDTO()
            {
                DocumentTypeId = 1,
                BirthDate = DateTime.Now,
                DocumentNumber = "123456",
                Names = null,
                LastNames = "Last Names",
                Enabled = false
            };

            var studentRepositoryMock = new Mock<IStudentRepository>();
            studentRepositoryMock.Setup(x => x.UpdateAsync(studentId, It.IsAny<StudentUpdateDTO>())).Returns(Task.FromResult(true));

            var studentService = new StudentService(studentRepositoryMock.Object);

            //ACT
            var serviceResult = studentService.Update(studentId, studentUpdateDTO);

            //Assert
            Assert.IsNotNull(serviceResult);
            Assert.IsInstanceOfType(serviceResult.Result, typeof(ValidationResult<bool>));
            Assert.IsTrue(serviceResult.Result.Errors.Any());
        }

        [TestMethod]
        public void When_Student_Update_IsInvalidByShortNames_AndErrors_Returns_BadRequest()
        {
            //Arrange
            var studentsToRetrieve = GetStudentDTOList();
            var studentToRetrieve = studentsToRetrieve.Last();
            var studentId = studentToRetrieve.StudentId;
            var token = Guid.Empty;
            var validationResult = new ValidationResult<StudentDTO>()
            {
                CustomResultCode = 500,
                Errors = new List<string>() { "Some random error message" },
                ResultList = new List<StudentDTO>(),
                SingleResult = null
            };

            var studentUpdateDTO = new StudentUpdateDTO()
            {
                DocumentTypeId = 1,
                BirthDate = DateTime.Now,
                DocumentNumber = "123456",
                Names = "a",
                LastNames = "Last Names",
                Enabled = false
            };

            var studentRepositoryMock = new Mock<IStudentRepository>();
            studentRepositoryMock.Setup(x => x.UpdateAsync(studentId, It.IsAny<StudentUpdateDTO>())).Returns(Task.FromResult(true));

            var studentService = new StudentService(studentRepositoryMock.Object);

            //ACT
            var serviceResult = studentService.Update(studentId, studentUpdateDTO);

            //Assert
            Assert.IsNotNull(serviceResult);
            Assert.IsInstanceOfType(serviceResult.Result, typeof(ValidationResult<bool>));
            Assert.IsTrue(serviceResult.Result.Errors.Any());
        }
        [TestMethod]
        public void When_Student_Update_IsInvalidByTooLongNames_AndErrors_Returns_BadRequest()
        {
            //Arrange
            var studentsToRetrieve = GetStudentDTOList();
            var studentToRetrieve = studentsToRetrieve.Last();
            var studentId = studentToRetrieve.StudentId;
            var token = Guid.Empty;
            var validationResult = new ValidationResult<StudentDTO>()
            {
                CustomResultCode = 500,
                Errors = new List<string>() { "Some random error message" },
                ResultList = new List<StudentDTO>(),
                SingleResult = null
            };

            var studentUpdateDTO = new StudentUpdateDTO()
            {
                DocumentTypeId = 1,
                BirthDate = DateTime.Now,
                DocumentNumber = "123456",
                Names = "1234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890",
                LastNames = "Last Names",
                Enabled = false
            };

            var studentRepositoryMock = new Mock<IStudentRepository>();
            studentRepositoryMock.Setup(x => x.UpdateAsync(studentId, It.IsAny<StudentUpdateDTO>())).Returns(Task.FromResult(true));

            var studentService = new StudentService(studentRepositoryMock.Object);

            //ACT
            var serviceResult = studentService.Update(studentId, studentUpdateDTO);

            //Assert
            Assert.IsNotNull(serviceResult);
            Assert.IsInstanceOfType(serviceResult.Result, typeof(ValidationResult<bool>));
            Assert.IsTrue(serviceResult.Result.Errors.Any());
        }

        [TestMethod]
        public void When_Student_Update_IsInvalidByNullLastNames_AndErrors_Returns_BadRequest()
        {
            //Arrange
            var studentsToRetrieve = GetStudentDTOList();
            var studentToRetrieve = studentsToRetrieve.Last();
            var studentId = studentToRetrieve.StudentId;
            var token = Guid.Empty;
            var validationResult = new ValidationResult<StudentDTO>()
            {
                CustomResultCode = 500,
                Errors = new List<string>() { "Some random error message" },
                ResultList = new List<StudentDTO>(),
                SingleResult = null
            };

            var studentUpdateDTO = new StudentUpdateDTO()
            {
                DocumentTypeId = 1,
                BirthDate = DateTime.Now,
                DocumentNumber = "123456",
                Names = "123456",
                LastNames = null,
                Enabled = false
            };

            var studentRepositoryMock = new Mock<IStudentRepository>();
            studentRepositoryMock.Setup(x => x.UpdateAsync(studentId, It.IsAny<StudentUpdateDTO>())).Returns(Task.FromResult(true));

            var studentService = new StudentService(studentRepositoryMock.Object);

            //ACT
            var serviceResult = studentService.Update(studentId, studentUpdateDTO);

            //Assert
            Assert.IsNotNull(serviceResult);
            Assert.IsInstanceOfType(serviceResult.Result, typeof(ValidationResult<bool>));
            Assert.IsTrue(serviceResult.Result.Errors.Any());
        }
        [TestMethod]
        public void When_Student_Update_IsInvalidByShortLastNames_AndErrors_Returns_BadRequest()
        {
            //Arrange
            var studentsToRetrieve = GetStudentDTOList();
            var studentToRetrieve = studentsToRetrieve.Last();
            var studentId = studentToRetrieve.StudentId;
            var token = Guid.Empty;
            var validationResult = new ValidationResult<StudentDTO>()
            {
                CustomResultCode = 500,
                Errors = new List<string>() { "Some random error message" },
                ResultList = new List<StudentDTO>(),
                SingleResult = null
            };

            var studentUpdateDTO = new StudentUpdateDTO()
            {
                DocumentTypeId = 1,
                BirthDate = DateTime.Now,
                DocumentNumber = "123456",
                Names = "123456",
                LastNames = "1",
                Enabled = false
            };

            var studentRepositoryMock = new Mock<IStudentRepository>();
            studentRepositoryMock.Setup(x => x.UpdateAsync(studentId, It.IsAny<StudentUpdateDTO>())).Returns(Task.FromResult(true));

            var studentService = new StudentService(studentRepositoryMock.Object);

            //ACT
            var serviceResult = studentService.Update(studentId, studentUpdateDTO);

            //Assert
            Assert.IsNotNull(serviceResult);
            Assert.IsInstanceOfType(serviceResult.Result, typeof(ValidationResult<bool>));
            Assert.IsTrue(serviceResult.Result.Errors.Any());
        }
        [TestMethod]
        public void When_Student_Update_IsInvalidByTooLongLastNames_AndErrors_Returns_BadRequest()
        {
            //Arrange
            var studentsToRetrieve = GetStudentDTOList();
            var studentToRetrieve = studentsToRetrieve.Last();
            var studentId = studentToRetrieve.StudentId;
            var token = Guid.Empty;
            var validationResult = new ValidationResult<StudentDTO>()
            {
                CustomResultCode = 500,
                Errors = new List<string>() { "Some random error message" },
                ResultList = new List<StudentDTO>(),
                SingleResult = null
            };

            var studentUpdateDTO = new StudentUpdateDTO()
            {
                DocumentTypeId = 1,
                BirthDate = DateTime.Now,
                DocumentNumber = "123456",
                Names = "123456",
                LastNames = "12345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890",
                Enabled = false
            };

            var studentRepositoryMock = new Mock<IStudentRepository>();
            studentRepositoryMock.Setup(x => x.UpdateAsync(studentId, It.IsAny<StudentUpdateDTO>())).Returns(Task.FromResult(true));

            var studentService = new StudentService(studentRepositoryMock.Object);

            //ACT
            var serviceResult = studentService.Update(studentId, studentUpdateDTO);

            //Assert
            Assert.IsNotNull(serviceResult);
            Assert.IsInstanceOfType(serviceResult.Result, typeof(ValidationResult<bool>));
            Assert.IsTrue(serviceResult.Result.Errors.Any());
        }

        [TestMethod]
        public void When_Student_Update_ThrowsException_Returns_InternalServerError()
        {
            //Arrange
            var studentsToRetrieve = GetStudentDTOList();
            var studentToRetrieve = studentsToRetrieve.Last();
            var studentId = studentToRetrieve.StudentId;
            var token = Guid.Empty;
            var validationResult = new ValidationResult<StudentDTO>()
            {
                CustomResultCode = 500,
                Errors = new List<string>() { "Some random error message" },
                ResultList = new List<StudentDTO>(),
                SingleResult = null
            };

            var studentUpdateDTO = new StudentUpdateDTO()
            {
                DocumentTypeId = 1,
                BirthDate = DateTime.Now,
                DocumentNumber = "123456",
                Names = "123456",
                LastNames = "123456",
                Enabled = false
            };

            var studentRepositoryMock = new Mock<IStudentRepository>();
            studentRepositoryMock.Setup(x => x.UpdateAsync(studentId, It.IsAny<StudentUpdateDTO>())).Throws(new Exception());

            var studentService = new StudentService(studentRepositoryMock.Object);

            //ACT
            var serviceResult = studentService.Update(studentId, studentUpdateDTO);

            //Assert
            Assert.IsNotNull(serviceResult);
            Assert.IsInstanceOfType(serviceResult.Result, typeof(ValidationResult<bool>));
            Assert.IsTrue(serviceResult.Result.Errors.Any());
            Assert.AreEqual((int)HttpStatusCode.InternalServerError, serviceResult.Result.CustomResultCode);
        }
        #endregion

        #region Delete
        [TestMethod]
        public void When_Student_Delete_IsWithoutRecords_AndErrors_Returns_BadRequest()
        {
            //Arrange
            StudentDTO studentToRetrieve = null;
            var studentId = -1;
            var token = Guid.Empty;
            var validationResult = new ValidationResult<StudentDTO>()
            {
                CustomResultCode = 500,
                Errors = new List<string>() { "Some random error message" },
                ResultList = new List<StudentDTO>(),
                SingleResult = null
            };

            var studentRepositoryMock = new Mock<IStudentRepository>();
            studentRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(studentToRetrieve));
            studentRepositoryMock.Setup(x => x.DeleteAsync(It.IsAny<int>())).Returns(Task.FromResult(0));

            var studentService = new StudentService(studentRepositoryMock.Object);

            //ACT
            var serviceResult = studentService.Delete(studentId);

            //Assert
            Assert.IsNotNull(serviceResult);
            Assert.IsInstanceOfType(serviceResult.Result, typeof(ValidationResult<int>));
            Assert.IsTrue(serviceResult.Result.Errors.Any());
        }

        [TestMethod]
        public void When_Student_Delete_HasValidationIssues_ByBeingEnabled_Returns_NoContent()
        {
            //Arrange
            var studentsToRetrieve = GetStudentDTOList();
            var studentToRetrieve = studentsToRetrieve.Last();
            studentToRetrieve.Enabled = true;
            var studentId = 1;
            var token = Guid.Empty;
            var validationResult = new ValidationResult<StudentDTO>()
            {
                CustomResultCode = 200,
                Errors = new List<string>(),
                ResultList = new List<StudentDTO>(),
                SingleResult = null
            };

            var studentRepositoryMock = new Mock<IStudentRepository>();
            studentRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(studentToRetrieve));

            studentRepositoryMock.Setup(x => x.DeleteAsync(It.IsAny<int>())).Returns(Task.FromResult(0));

            var studentService = new StudentService(studentRepositoryMock.Object);

            //ACT
            var serviceResult = studentService.Delete(studentId);

            //Assert
            Assert.IsNotNull(serviceResult);
            Assert.IsInstanceOfType(serviceResult.Result, typeof(ValidationResult<int>));
            Assert.IsTrue(serviceResult.Result.Errors.Any());
        }

        [TestMethod]
        public void When_Student_Delete_IsOk_Returns_NoContent()
        {
            //Arrange
            var studentsToRetrieve = GetStudentDTOList();
            var studentToRetrieve = studentsToRetrieve.Last();
            studentToRetrieve.Enabled = false;
            var studentId = 1;
            var token = Guid.Empty;
            var validationResult = new ValidationResult<StudentDTO>()
            {
                CustomResultCode = 200,
                Errors = new List<string>(),
                ResultList = new List<StudentDTO>(),
                SingleResult = null
            };

            var studentRepositoryMock = new Mock<IStudentRepository>();
            studentRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(studentToRetrieve));

            studentRepositoryMock.Setup(x => x.DeleteAsync(It.IsAny<int>())).Returns(Task.FromResult(0));

            var studentService = new StudentService(studentRepositoryMock.Object);

            //ACT
            var serviceResult = studentService.Delete(studentId);

            //Assert
            Assert.IsNotNull(serviceResult);
            Assert.IsInstanceOfType(serviceResult.Result, typeof(ValidationResult<int>));
            
        }
        [TestMethod]
        public void When_Student_Delete_ThrowsException_Returns_InternalServerError()
        {
            //Arrange
            var studentsToRetrieve = GetStudentDTOList();
            var studentToRetrieve = studentsToRetrieve.Last();
            studentToRetrieve.Enabled = false;
            var studentId = 1;
            var token = Guid.Empty;
            var validationResult = new ValidationResult<StudentDTO>()
            {
                CustomResultCode = 200,
                Errors = new List<string>(),
                ResultList = new List<StudentDTO>(),
                SingleResult = null
            };

            var studentRepositoryMock = new Mock<IStudentRepository>();
            studentRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(studentToRetrieve));

            studentRepositoryMock.Setup(x => x.DeleteAsync(It.IsAny<int>())).Throws(new Exception());

            var studentService = new StudentService(studentRepositoryMock.Object);

            //ACT
            var serviceResult = studentService.Delete(studentId);

            //Assert
            Assert.IsNotNull(serviceResult);
            Assert.IsInstanceOfType(serviceResult.Result, typeof(ValidationResult<int>));
            Assert.IsTrue(serviceResult.Result.Errors.Any());
            Assert.AreEqual((int)HttpStatusCode.InternalServerError, serviceResult.Result.CustomResultCode);
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