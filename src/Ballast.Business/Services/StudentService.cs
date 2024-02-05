using System.Net;
using Ballast.Business.Repository.Interfaces;
using Ballast.Business.Services.Interfaces;
using Ballast.Business.Validation;
using Ballast.DTO;

namespace Ballast.Business.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;

        public StudentService(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public async Task<ValidationResult<StudentDTO>> Create(StudentCreateDTO studentCreateDTO)
        {
            var validationResult = studentCreateDTO.ValidateCreate();

            if (validationResult.Errors.Any())
            {
                return validationResult;
            }
            try
            {
                var createdStudent = await _studentRepository.Add(studentCreateDTO.ConvertToStudentDTO());
                validationResult.SingleResult = createdStudent;
            }
            catch (Exception ex)
            {
                validationResult.CustomResultCode = (int)HttpStatusCode.InternalServerError;
                validationResult.Errors.Add(ex.Message);
            }

            return validationResult;
        }
        public async Task<ValidationResult<int>> Delete(int studentId)
        {
            var validationResult = new ValidationResult<int>();

            var currentStudentRecord = await _studentRepository.GetByIdAsync(studentId);
            if (currentStudentRecord == null)
            {
                validationResult.Errors.Add("Student does not exists");
                validationResult.CustomResultCode = (int)HttpStatusCode.BadRequest;
                return validationResult;
            }

            validationResult = currentStudentRecord.ValidateDelete();
            if (validationResult.Errors.Any())
            {
                return validationResult;
            }
            try
            {
                var affectedRowCount = await _studentRepository.DeleteAsync(currentStudentRecord.StudentId);
                validationResult.SingleResult = affectedRowCount;
            }
            catch (Exception ex)
            {
                validationResult.CustomResultCode = (int)HttpStatusCode.InternalServerError;
                validationResult.Errors.Add(ex.Message);
            }

            return validationResult;
        }

        public async Task<ValidationResult<StudentDTO>> GetAll(int pageNumber, int pageSize)
        {
            var validationResult = new ValidationResult<StudentDTO>();
            try
            {
                var studentList = await _studentRepository.GetAll(pageNumber, pageSize);
                validationResult.ResultList = studentList;
                validationResult.CustomResultCode = (int)HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                validationResult.CustomResultCode = (int)HttpStatusCode.InternalServerError;
                validationResult.Errors.Add(ex.Message);
            }
            return validationResult;
        }

        public async Task<ValidationResult<StudentDTO>> GetById(int id)
        {
            var validationResult = new ValidationResult<StudentDTO>();
            if (id < 1)
            {
                validationResult.Errors.Add("Student Id cannot be less than '1' One");
                validationResult.CustomResultCode = (int)HttpStatusCode.BadRequest;
                return validationResult;
            }
            try
            {
                var student = await _studentRepository.GetByIdAsync(id);
                if (student == null)
                {
                    validationResult.CustomResultCode = (int)HttpStatusCode.NotFound;
                    return validationResult;
                }
                validationResult.SingleResult = student;
                validationResult.CustomResultCode = (int)HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                validationResult.CustomResultCode = (int)HttpStatusCode.InternalServerError;
                validationResult.Errors.Add(ex.Message);
            }
            return validationResult;
        }

        public async Task<ValidationResult<bool>> Update(int studentId, StudentUpdateDTO student)
        {
            var validationResult = student.ValidateUpdate();
            if (validationResult.Errors.Any())
            {
                return validationResult;
            }
            try
            {
                var updatedRecordResult = await _studentRepository.UpdateAsync(studentId, student);
                validationResult.SingleResult = updatedRecordResult;
            }
            catch (Exception ex)
            {
                validationResult.CustomResultCode = (int)HttpStatusCode.InternalServerError;
                validationResult.Errors.Add(ex.Message);
            }

            return validationResult;
        }
    }
}
