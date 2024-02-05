using System.Data;
using Ballast.DTO;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Ballast.Data
{
    public class User
    {
        private readonly string ROConnectionString;
        private readonly string RWConnectionString;

        public User(IConfiguration configuration)
        {
            var readOnlyConnectionString = configuration.GetConnectionString("ROConnectionString") ?? throw new ArgumentException("ROConnectionString");
            ROConnectionString = readOnlyConnectionString;

            var readAndWriteConnectionString = configuration.GetConnectionString("RWConnectionString") ?? throw new ArgumentException("RWConnectionString");
            RWConnectionString = readAndWriteConnectionString;
        }

        #region CREATE
        public async Task<int> InsertAsync(UserDTO userDTO)
        {
            using (var connection = new SqlConnection(RWConnectionString))
            {
                using (var command = new SqlCommand("SP_INS_User", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddRange(new SqlParameter[]
                    {
                        new SqlParameter()
                        {
                            ParameterName = "@PARAM_UserName",
                            SqlDbType = SqlDbType.NVarChar,
                            Value = userDTO.UserName,
                            Direction = ParameterDirection.Input
                        },
                        new SqlParameter()
                        {
                            ParameterName = "@PARAM_PasswordHash",
                            SqlDbType = SqlDbType.NVarChar,
                            Value = userDTO.PasswordHash,
                            Direction = ParameterDirection.Input
                        }
                    });

                    connection.Open();
                    return await command.ExecuteNonQueryAsync();
                }
            }
        }
        #endregion

        #region RETRIEVE
        public async Task<UserDTO?> GetByUserNameAsync(string userName)
        {
            UserDTO userDTO = null;

            using (var connection = new SqlConnection(ROConnectionString))
            {
                using (var command = new SqlCommand("dbo.SP_SEL_User", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddRange(new SqlParameter[]
                    {
                        new SqlParameter()
                        {
                            ParameterName = "@PARAM_UserName",
                            SqlDbType = SqlDbType.NVarChar,
                            Value = userName,
                            Direction = ParameterDirection.Input
                        }
                    });

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            userDTO = new UserDTO()
                            {
                                UserId = reader.GetInt32("UserId"),
                                UserName = reader.GetString("UserName"),
                                PasswordHash = reader.GetString("PasswordHash"),
                            };
                        }
                    }
                }

            }
            return await Task.FromResult(userDTO);
        }
        public async Task<UserGetDTO?> GetByUserIdAsync(int userId)
        {
            UserGetDTO userDTO = null;

            using (var connection = new SqlConnection(ROConnectionString))
            {
                using (var command = new SqlCommand("dbo.SP_SEL_User", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddRange(new SqlParameter[]
                    {
                        new SqlParameter()
                        {
                            ParameterName = "@PARAM_UserId",
                            SqlDbType = SqlDbType.Int,
                            Value = userId,
                            Direction = ParameterDirection.Input
                        }
                    });

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            userDTO = new UserGetDTO()
                            {
                                UserId = reader.GetInt32("UserId"),
                                UserName = reader.GetString("UserName"),
                            };
                        }
                    }
                }
            }
            return await Task.FromResult(userDTO);
        }
        #endregion

        #region UPDATE
        public async Task<int> ChangePasswordAsync(UserChangePasswordDTO userChangePasswordDTO)
        {
            using (var connection = new SqlConnection(ROConnectionString))
            {
                using (var command = new SqlCommand("dbo.SP_UPD_User_ChangePassword", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddRange(
                    [
                        new SqlParameter()
                        {
                            ParameterName = "@PARAM_UserId",
                            SqlDbType = SqlDbType.Int,
                            Value = userChangePasswordDTO.UserId,
                            Direction = ParameterDirection.Input
                        },
                        new SqlParameter()
                        {
                            ParameterName = "@PARAM_PasswordHash",
                            SqlDbType = SqlDbType.NVarChar,
                            Value = userChangePasswordDTO.NewPassword,
                            Direction = ParameterDirection.Input
                        }
                    ]);

                    connection.Open();
                    var affectedRowCount = command.ExecuteNonQuery();
                    return await Task.FromResult(affectedRowCount);
                }
            }
        }
        #endregion
    }
}
