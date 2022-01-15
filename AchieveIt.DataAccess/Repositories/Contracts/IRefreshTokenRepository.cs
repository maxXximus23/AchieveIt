using System;
using System.Threading.Tasks;
using AchieveIt.DataAccess.Entities;

namespace AchieveIt.DataAccess.Repositories.Contracts
{
    public interface IRefreshTokenRepository
    {
        public void AddToken(RefreshToken refreshToken);

        public void DeleteToken(RefreshToken refreshToken);
        public Task<RefreshToken> GetToken(Guid refreshTokenId);
    }
}