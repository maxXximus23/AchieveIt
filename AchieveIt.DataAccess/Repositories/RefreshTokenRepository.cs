using System;
using System.Threading.Tasks;
using AchieveIt.DataAccess.Entities;
using AchieveIt.DataAccess.Repositories.Contracts;
using AchieveIt.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace AchieveIt.DataAccess.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly DatabaseContext _databaseContext;

        public RefreshTokenRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public void AddToken(RefreshToken refreshToken)
        {
            _databaseContext.RefreshTokens.Add(refreshToken);
        }

        public void DeleteToken(RefreshToken refreshToken)
        {
            _databaseContext.RefreshTokens.Remove(refreshToken);
        }

        public async Task<RefreshToken> GetToken(Guid refreshTokenId)
        {
            return await _databaseContext.RefreshTokens.FirstOrDefaultAsync(
                refreshToken => refreshToken.Id == refreshTokenId) 
                   ?? throw new NotFoundException(nameof(RefreshToken), refreshTokenId.ToString());
        }
    }
}