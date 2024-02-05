using System.Net;
using Ballast.DTO;

namespace Ballast.Business.Validation
{
    public static class StudentValidation
    {
        public static ValidationResult<StudentDTO> ValidateCreate(this StudentCreateDTO student)
        {
            var validationResult = new ValidationResult<StudentDTO>();

            if (student.DocumentTypeId < 1 || student.DocumentTypeId > 4)
            {
                validationResult.Errors.Add("Please set a valid Document Type");
            }

            if (student.DocumentTypeId != (int)EDocumentType.ForeignId
                && student.DocumentTypeId != (int)EDocumentType.Passport)
            {
                if (!int.TryParse(student.DocumentNumber, out _))
                {
                    validationResult.Errors.Add("Document number cannot have letters if Document type is not ForeignId or Passport");
                }
            }

            if (string.IsNullOrWhiteSpace(student.DocumentNumber))
            {
                validationResult.Errors.Add("Document Number cannot be Empty");
            }
            else
            {
                if (student.DocumentNumber.Length < 5 || student.DocumentNumber.Length > 30)
                {
                    validationResult.Errors.Add("Document Number length should be between 5 and 30");
                }
            }
            if (string.IsNullOrWhiteSpace(student.Names))
            {
                validationResult.Errors.Add("Names cannot be Empty");
            }
            else
            {
                if (student.Names.Length < 2 || student.Names.Length > 100)
                {
                    validationResult.Errors.Add("Names length should be between 5 and 30");
                }
            }
            if (string.IsNullOrWhiteSpace(student.LastNames))
            {
                validationResult.Errors.Add("Last names cannot be Empty");
            }
            else
            {
                if (student.LastNames.Length < 2 || student.LastNames.Length > 100)
                {
                    validationResult.Errors.Add("Last names length should be between 5 and 30");
                }
            }

            if (validationResult.Errors.Count > 0)
            {
                validationResult.CustomResultCode = (int)HttpStatusCode.BadRequest;
            }
            return validationResult;
        }

        public static ValidationResult<bool> ValidateUpdate(this StudentUpdateDTO student)
        {
            var validationResult = new ValidationResult<bool>();

            if (student.DocumentTypeId < 0 || student.DocumentTypeId > 4)
            {
                validationResult.Errors.Add("Please set a valid Document Type");
            }

            if (student.DocumentTypeId != (int)EDocumentType.ForeignId
                && student.DocumentTypeId != (int)EDocumentType.Passport)
            {
                var dummyValue = 0;
                if (!int.TryParse(student.DocumentNumber, out dummyValue))
                {
                    validationResult.Errors.Add("Document number cannot have letters if Document type is not ForeignId or Passport");
                }
            }

            if (string.IsNullOrWhiteSpace(student.DocumentNumber))
            {
                validationResult.Errors.Add("Document Number cannot be Empty");
            }
            else
            {
                if (student.DocumentNumber.Length < 5 || student.DocumentNumber.Length > 30)
                {
                    validationResult.Errors.Add("Document Number length should be between 5 and 30");
                }
            }
            if (string.IsNullOrWhiteSpace(student.Names))
            {
                validationResult.Errors.Add("Names cannot be Empty");
            }
            else
            {
                if (student.Names.Length < 2 || student.Names.Length > 100)
                {
                    validationResult.Errors.Add("Names length should be between 5 and 30");
                }
            }
            if (string.IsNullOrWhiteSpace(student.LastNames))
            {
                validationResult.Errors.Add("Last names cannot be Empty");
            }
            else
            {
                if (student.LastNames.Length < 2 || student.LastNames.Length > 100)
                {
                    validationResult.Errors.Add("Last names length should be between 5 and 30");
                }
            }

            if (validationResult.Errors.Count > 0)
            {
                validationResult.CustomResultCode = (int)HttpStatusCode.BadRequest;
            }
            return validationResult;
        }

        public static ValidationResult<int> ValidateDelete(this StudentDTO student)
        {
            var validationResult = new ValidationResult<int>();

            if (student.Enabled)
            {
                validationResult.Errors.Add("Enabled students cannot be deleted");
            }

            if (validationResult.Errors.Count > 0)
            {
                validationResult.CustomResultCode = (int)HttpStatusCode.BadRequest;
            }
            return validationResult;
        }
    }
}
