using System.Threading.Tasks;

namespace NetCore.Business.Authentication
{
    public interface ITokenProvider
    {
        Task<string> CreateTokenAsync(TokenData tokenData);

        Task<string> CreateRefreshTokenAsync(TokenData tokenData);

        TokenData GetTokenData(string token);
    }
}
