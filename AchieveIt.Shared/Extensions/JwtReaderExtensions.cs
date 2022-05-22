using Kirpichyov.FriendlyJwt.Contracts;

namespace AchieveIt.Shared.Extensions
{
    public static class JwtReaderExtensions
    {
        public static int GetUserId(this IJwtTokenReader jwtTokenReader)
        {
            return int.Parse(jwtTokenReader.UserId);
        }
    }
}