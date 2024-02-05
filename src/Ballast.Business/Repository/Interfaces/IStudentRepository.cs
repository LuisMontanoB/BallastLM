using Ballast.DTO;

namespace Ballast.Business.Repository.Interfaces
{
    public interface IStudentRepository
    {
        Task<StudentDTO?> GetByIdAsync(int id);
        Task<List<StudentDTO>> GetAll(int pageNumber, int pageSize);
        Task<StudentDTO> Add(StudentDTO entity);
        Task<int> DeleteAsync(int studentId);
        Task<bool> UpdateAsync(int studentId, StudentUpdateDTO studentUpdateDTO);
    }
}
