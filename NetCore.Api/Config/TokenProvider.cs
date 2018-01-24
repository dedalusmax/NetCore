using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NetCore.Business.Authentication;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NetCore.Api.Config
{
    public class TokenProvider : ITokenProvider
    {
        public const string CLAIM_USERID = "app:userid";

        private const int TOKEN_EXPIRATION_MINUTES = 5;

        private readonly string _issuer;
        private readonly string _audience;
        private readonly TimeSpan _expirationShort;
        private readonly TimeSpan _expirationLong;
        private readonly SigningCredentials _signingCredentials;
        private readonly TokenValidationParameters _validationParametersWithoutLifetime;

        public TokenProvider(IConfiguration configuration)
        {
            _issuer = TokenHelper.GetTokenIssuer(configuration);
            _audience = TokenHelper.GetTokenAudience(configuration);
            _expirationShort = TokenHelper.GetTokenExpirationShort(configuration);
            _expirationLong = TokenHelper.GetTokenExpirationLong(configuration);
            _signingCredentials = TokenHelper.GetTokenSigningCredentials(configuration);
            _validationParametersWithoutLifetime = TokenHelper.GetTokenValidationParameters(configuration, validateLifetime: false);
        }

        public async Task<string> CreateTokenAsync(TokenData tokenData)
        {
            var claims = new List<Claim>
            {
                new Claim(CLAIM_USERID, tokenData.UserId.ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, tokenData.UserName),
                new Claim(ClaimTypes.Name, tokenData.DisplayName)
            };
            if (!string.IsNullOrEmpty(tokenData.Role))
                claims.Add(new Claim(ClaimTypes.Role, tokenData.Role));

            var expiration = TimeSpan.FromMinutes(TOKEN_EXPIRATION_MINUTES);

            return await CreateTokenFromClaimsAndExpirationAsync(claims, expiration);
        }

        public async Task<string> CreateRefreshTokenAsync(TokenData tokenData)
        {
            var claims = new List<Claim> { new Claim(CLAIM_USERID, tokenData.UserId.ToString()) };
            var expiration = _expirationLong;

            return await CreateTokenFromClaimsAndExpirationAsync(claims, expiration);
        }

        public IEnumerable<Claim> ReadClaims(string token)
        {
            var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
            return jwtToken?.Claims;
        }

        public TokenData GetTokenData(string token)
        {
            var tokenData = new TokenData();
            var handler = new JwtSecurityTokenHandler();

            SecurityToken securityToken;
            handler.ValidateToken(token, _validationParametersWithoutLifetime, out securityToken);

            if (securityToken is JwtSecurityToken jwtSecurityToken)
            {
                var userIdValue = jwtSecurityToken.Claims.SingleOrDefault(_ => _.Type == CLAIM_USERID)?.Value;
                var issuedAtValue = jwtSecurityToken.Claims.SingleOrDefault(_ => _.Type == JwtRegisteredClaimNames.Iat)?.Value;

                tokenData.UserId = long.TryParse(userIdValue, out long longParse) ? longParse : 0;
                tokenData.UserName = jwtSecurityToken.Claims.SingleOrDefault(_ => _.Type == JwtRegisteredClaimNames.Sub)?.Value;
                tokenData.DisplayName = jwtSecurityToken.Claims.SingleOrDefault(_ => _.Type == ClaimTypes.Name)?.Value;
                tokenData.Role = jwtSecurityToken.Claims.SingleOrDefault(_ => _.Type == ClaimTypes.Role)?.Value;
                tokenData.IssuedAt = long.TryParse(issuedAtValue, out longParse) ? (DateTime?)DateTimeOffset.FromUnixTimeSeconds(longParse).DateTime : null;
            }

            return tokenData;
        }

        private async Task<string> CreateTokenFromClaimsAndExpirationAsync(ICollection<Claim> claims, TimeSpan expiration)
        {
            var now = DateTime.UtcNow;

            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, await Task.FromResult(Guid.NewGuid().ToString())));
            claims.Add(new Claim(JwtRegisteredClaimNames.Iat, new DateTimeOffset(now).ToUniversalTime().ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64));

            var jwt = new JwtSecurityToken(_issuer, _audience, claims, now, now.Add(expiration), _signingCredentials);
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return encodedJwt;
        }
    }

}
