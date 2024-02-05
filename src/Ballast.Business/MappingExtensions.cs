using Ballast.DTO;

namespace Ballast.Business
{
    public static class MappingExtensions
    {
        public static StudentDTO ConvertToStudentDTO(this StudentCreateDTO source)
        {
            return new StudentDTO
            {
                DocumentTypeId = source.DocumentTypeId,
                DocumentNumber = source.DocumentNumber,
                Names = source.Names,
                LastNames = source.LastNames,
                BirthDate = source.BirthDate,
                Enabled = true
            };
        }

        public static StudentDTO ConvertToStudentDTO(this StudentUpdateDTO source, int studentId)
        {
            return new StudentDTO
            {
                StudentId = studentId,
                DocumentTypeId = source.DocumentTypeId,
                DocumentNumber = source.DocumentNumber,
                Names = source.Names,
                LastNames = source.LastNames,
                BirthDate = source.BirthDate,
                Enabled = source.Enabled
            };
        }

        public static UserDTO ConvertToUserDTO(this UserCreateDTO source)
        {
            return new UserDTO
            {
                UserName = source.UserName,
                PasswordHash = source.Password
            };
        }
    }
}
