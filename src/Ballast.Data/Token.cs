using System.Data;
using Ballast.DTO;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Ballast.Data
{
    public class Token
    {
        private readonly string ROConnectionString;
        private readonly string RWConnectionString;

        public Token(IConfiguration configuration)
        {
            var readOnlyConnectionString = configuration.GetConnectionString("ROConnectionString") ?? throw new ArgumentException("ROConnectionString");
            ROConnectionString = readOnlyConnectionString;

            var readAndWriteConnectionString = configuration.GetConnectionString("RWConnectionString") ?? throw new ArgumentException("RWConnectionString");
            RWConnectionString = readAndWriteConnectionString;
        }

        #region CREATE
        public async Task<int> InsertAsync(TokenDTO tokenDTO)
        {
            using (var connection = new SqlConnection(RWConnectionString))
            {
                using (var command = new SqlCommand("SP_INS_Token", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddRange(
                    [
                        new SqlParameter()
                        {
                            ParameterName = "@PARAM_UserId",
                            SqlDbType = SqlDbType.NVarChar,
                            Value = tokenDTO.UserId,
                            Direction = ParameterDirection.Input
                        },
                        new SqlParameter()
                        {
                            ParameterName = "@PARAM_TokenCode",
                            SqlDbType = SqlDbType.UniqueIdentifier,
                            Value = tokenDTO.TokenCode,
                            Direction = ParameterDirection.Input
                        },
                        new SqlParameter()
                        {
                            ParameterName = "@PARAM_ExpiresIn",
                            SqlDbType = SqlDbType.DateTime,
                            Value = tokenDTO.ExpiresIn,
                            Direction = ParameterDirection.Input
                        }
                    ]);
                    connection.Open();
                    return await command.ExecuteNonQueryAsync();
                }
            }
        }
        #endregion

        #region RETRIEVE
        public async Task<TokenDTO?> GetByTokenCodeAsync(Guid tokenCode)
        {
            TokenDTO tokenDTO = null;

            using (var connection = new SqlConnection(ROConnectionString))
            {
                using (var command = new SqlCommand("dbo.SP_SEL_Token", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddRange(new SqlParameter[]
                    {
                        new SqlParameter()
                        {
                            ParameterName = "@PARAM_TokenCode",
                            SqlDbType = SqlDbType.UniqueIdentifier,
                            Value = tokenCode,
                            Direction = ParameterDirection.Input
                        }
                    });

                    connection.Open();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            tokenDTO = new TokenDTO()
                            {
                                UserTokenId = reader.GetInt32("UserTokenId"),
                                UserId = reader.GetInt32("UserId"),
                                TokenCode = reader.GetGuid("TokenCode"),
                                ExpiresIn = reader.GetDateTime("ExpiresIn"),
                            };
                        }
                    }
                }
            }
            return await Task.FromResult(tokenDTO);
        }
        #endregion
    }
}
