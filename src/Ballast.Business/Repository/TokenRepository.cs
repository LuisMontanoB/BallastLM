using Ballast.Business.Repository.Interfaces;
using Ballast.DTO;
using Microsoft.Extensions.Configuration;

namespace Ballast.Business.Repository
{
    public class TokenRepository : ITokenRepository
    {
        private readonly Data.Token _tokenDL;

        public TokenRepository(IConfiguration configuration)
        {
            _tokenDL = new Data.Token(configuration);
        }

        public async Task<bool> AddAsync(TokenDTO tokenDTO)
        {
            await _tokenDL.InsertAsync(tokenDTO);
            return await Task.FromResult(true);
        }

        public async Task<TokenDTO?> GetByToken(Guid tokenCode)
        {
            return await _tokenDL.GetByTokenCodeAsync(tokenCode);
        }
    }
}
