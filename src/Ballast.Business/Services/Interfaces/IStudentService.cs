using Ballast.Business.Validation;
using Ballast.DTO;

namespace Ballast.Business.Services.Interfaces
{
    public interface IStudentService
    {
        Task<ValidationResult<StudentDTO>> Create(StudentCreateDTO student);

        Task<ValidationResult<StudentDTO>> GetAll(int pageNumber, int pageSize);

        Task<ValidationResult<StudentDTO>> GetById(int id);

        Task<ValidationResult<bool>> Update(int studentId, StudentUpdateDTO student);

        Task<ValidationResult<int>> Delete(int studentId);
    }
}
