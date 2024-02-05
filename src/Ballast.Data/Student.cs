using System.Data;
using Ballast.DTO;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Ballast.Data
{
    public class Student
    {
        private readonly string ROConnectionString;
        private readonly string RWConnectionString;

        public Student(IConfiguration configuration)
        {
            var readOnlyConnectionString = configuration.GetConnectionString("ROConnectionString") ?? throw new ArgumentException("ROConnectionString");
            ROConnectionString = readOnlyConnectionString;

            var readAndWriteConnectionString = configuration.GetConnectionString("RWConnectionString") ?? throw new ArgumentException("RWConnectionString");
            RWConnectionString = readAndWriteConnectionString;
        }

        #region CREATE
        public async Task<StudentDTO> InsertAsync(StudentDTO studentDTO)
        {
            StudentDTO newRecord = null;

            using (var connection = new SqlConnection(RWConnectionString))
            {
                using (var command = new SqlCommand("SP_INS_Student", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddRange(new SqlParameter[]
                    {
                        new SqlParameter()
                        {
                            ParameterName = "@PARAM_DocumentTypeId",
                            SqlDbType = SqlDbType.Int,
                            Value = studentDTO.DocumentTypeId,
                            Direction = ParameterDirection.Input
                        },
                        new SqlParameter()
                        {
                            ParameterName = "@PARAM_DocumentNumber",
                            SqlDbType = SqlDbType.NVarChar,
                            Value = studentDTO.DocumentNumber,
                            Direction = ParameterDirection.Input
                        },
                        new SqlParameter()
                        {
                            ParameterName = "@PARAM_Names",
                            SqlDbType = SqlDbType.NVarChar,
                            Value = studentDTO.Names,
                            Direction = ParameterDirection.Input
                        },
                        new SqlParameter()
                        {
                            ParameterName = "@PARAM_LastNames",
                            SqlDbType = SqlDbType.NVarChar,
                            Value = studentDTO.LastNames,
                            Direction = ParameterDirection.Input
                        },
                        new SqlParameter()
                        {
                            ParameterName = "@PARAM_BirthDay",
                            SqlDbType = SqlDbType.Date,
                            Value = studentDTO.BirthDate,
                            Direction = ParameterDirection.Input
                        },
                        new SqlParameter()
                        {
                            ParameterName = "@PARAM_Enabled",
                            SqlDbType = SqlDbType.Bit,
                            Value = studentDTO.Enabled,
                            Direction = ParameterDirection.Input
                        }
                    });

                    connection.Open();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            newRecord = new StudentDTO()
                            {
                                StudentId = reader.GetInt32("StudentId"),
                                DocumentTypeId = reader.GetInt32("DocumentTypeId"),
                                Names = reader.GetString("Names"),
                                LastNames = reader.GetString("LastNames"),
                                DocumentNumber = reader.GetString("DocumentNumber"),
                                BirthDate = reader.GetDateTime("BirthDate"),
                                Enabled = reader.GetBoolean("Enabled")
                            };
                        }
                    }
                }

            }
            return newRecord;
        }
        #endregion

        #region RETRIEVE
        public async Task<StudentDTO?> GetByIdAsync(int studentId)
        {
            var result = GenericGetAsync(studentId);
            if (result == null)
            {
                return null;
            }
            return await Task.FromResult(result.Result.FirstOrDefault());
        }

        public Task<List<StudentDTO>> GetAllAsync(int pageNumber = 1, int pageSize = 50)
        {
            return GenericGetAsync(null, pageNumber, pageSize);
        }

        public async Task<List<StudentDTO>> GenericGetAsync(int? studentId = null, int pageNumber = 1, int pageSize = 50)
        {
            var studentList = new List<StudentDTO>();

            using (var connection = new SqlConnection(ROConnectionString))
            {
                using (var command = new SqlCommand("dbo.SP_SEL_Student", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    if (studentId.HasValue)
                    {
                        command.Parameters.Add(
                            new SqlParameter()
                            {
                                ParameterName = "@PARAM_StudentId",
                                SqlDbType = SqlDbType.Int,
                                Value = studentId.Value,
                                Direction = ParameterDirection.Input
                            }
                        );
                    }
                    else
                    {
                        command.Parameters.AddRange(new SqlParameter[]
                        {
                        new SqlParameter()
                        {
                            ParameterName = "@PARAM_PageNumber",
                            SqlDbType = SqlDbType.Int,
                            Value = pageNumber,
                            Direction = ParameterDirection.Input
                        },
                        new SqlParameter()
                        {
                            ParameterName = "@PARAM_PageSize",
                            SqlDbType = SqlDbType.Int,
                            Value = pageSize,
                            Direction = ParameterDirection.Input
                        }
                        });
                    }
                    connection.Open();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            studentList.Add(new StudentDTO()
                            {
                                StudentId = reader.GetInt32("StudentId"),
                                DocumentTypeId = reader.GetInt32("DocumentTypeId"),
                                Names = reader.GetString("Names"),
                                LastNames = reader.GetString("LastNames"),
                                DocumentNumber = reader.GetString("DocumentNumber"),
                                BirthDate = reader.GetDateTime("BirthDate"),
                                Enabled = reader.GetBoolean("Enabled")
                            });
                        }
                    }
                }

            }
            return studentList;
        }
        #endregion

        #region UPDATE
        public async Task<bool> UpdateAsync(StudentDTO studentDTO)
        {
            using (var connection = new SqlConnection(RWConnectionString))
            {
                using (var command = new SqlCommand("SP_UPD_Student", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddRange(new SqlParameter[]
                    {
                        new SqlParameter()
                        {
                            ParameterName = "@PARAM_StudentId",
                            SqlDbType = SqlDbType.Int,
                            Value = studentDTO.StudentId,
                            Direction = ParameterDirection.Input
                        },
                        new SqlParameter()
                        {
                            ParameterName = "@PARAM_DocumentTypeId",
                            SqlDbType = SqlDbType.Int,
                            Value = studentDTO.DocumentTypeId,
                            Direction = ParameterDirection.Input
                        },
                        new SqlParameter()
                        {
                            ParameterName = "@PARAM_DocumentNumber",
                            SqlDbType = SqlDbType.NVarChar,
                            Value = studentDTO.DocumentNumber,
                            Direction = ParameterDirection.Input
                        },
                        new SqlParameter()
                        {
                            ParameterName = "@PARAM_Names",
                            SqlDbType = SqlDbType.NVarChar,
                            Value = studentDTO.Names,
                            Direction = ParameterDirection.Input
                        },
                        new SqlParameter()
                        {
                            ParameterName = "@PARAM_LastNames",
                            SqlDbType = SqlDbType.NVarChar,
                            Value = studentDTO.LastNames,
                            Direction = ParameterDirection.Input
                        },
                        new SqlParameter()
                        {
                            ParameterName = "@PARAM_BirthDay",
                            SqlDbType = SqlDbType.Date,
                            Value = studentDTO.BirthDate,
                            Direction = ParameterDirection.Input
                        },
                        new SqlParameter()
                        {
                            ParameterName = "@PARAM_Enabled",
                            SqlDbType = SqlDbType.Bit,
                            Value = studentDTO.Enabled,
                            Direction = ParameterDirection.Input
                        }
                    });

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            return await Task.FromResult(true);
        }
        #endregion

        #region DELETE
        public async Task<int> DeleteAsync(int studentId)
        {
            using (var connection = new SqlConnection(RWConnectionString))
            {
                using (var command = new SqlCommand("SP_DEL_Student", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddRange(new SqlParameter[]
                    {
                        new SqlParameter()
                        {
                            ParameterName = "@PARAM_StudentId",
                            SqlDbType = SqlDbType.Int,
                            Value = studentId,
                            Direction = ParameterDirection.Input
                        }
                    });

                    connection.Open();
                    return await command.ExecuteNonQueryAsync();
                }
            }
        }
        #endregion

    }
}
