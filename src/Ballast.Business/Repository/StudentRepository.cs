using Ballast.Business.Repository.Interfaces;
using Ballast.DTO;
using Microsoft.Extensions.Configuration;

namespace Ballast.Business.Repository
{
    public class StudentRepository : IStudentRepository
    {
        private readonly Data.Student _studentDL;

        public StudentRepository(IConfiguration configuration)
        {
            _studentDL = new Data.Student(configuration);
        }

        public async Task<StudentDTO> Add(StudentDTO entity)
        {
            return await _studentDL.InsertAsync(entity);
        }

        public async Task<int> DeleteAsync(int studentId)
        {
            return await _studentDL.DeleteAsync(studentId);
        }

        public async Task<List<StudentDTO>> GetAll(int pageNumber, int pageSize)
        {
            return await _studentDL.GetAllAsync(pageNumber, pageSize);
        }

        public async Task<StudentDTO?> GetByIdAsync(int id)
        {
            return await _studentDL.GetByIdAsync(id);
        }

        public async Task<bool> UpdateAsync(int studentId, StudentUpdateDTO studentUpdateDTO)
        {
            return await _studentDL.UpdateAsync(studentUpdateDTO.ConvertToStudentDTO(studentId));
        }
    }
}
