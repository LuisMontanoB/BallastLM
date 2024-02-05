using Ballast.DTO;

namespace Ballast.Business.Repository.Interfaces
{
    public interface ITokenRepository
    {
        Task<bool> AddAsync(TokenDTO tokenDTO);
        Task<TokenDTO?> GetByToken(Guid tokenCode);
    }
}
