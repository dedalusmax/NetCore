using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace NetCore.Api.Config
{
    public static class TokenHelper
    {
        public const string FALLBACK_ISSUER = "APP_TEST";
        public const string FALLBACK_AUDIENCE = "APP_TEST_AUDIENCE";
        public const int FALLBACK_EXPIRATION_IN_MINUTES = 60;
        public const int FALLBACK_EXPIRATION_IN_DAYS = 30;
        public const string FALBACK_APPLICATION_SECRET = "APP_TEST_APPLICATION_SECRET";

        public static string GetTokenIssuer(IConfiguration configuration)
        {
            var issuerConfig = configuration["Issuer"];
            return string.IsNullOrWhiteSpace(issuerConfig) ? FALLBACK_ISSUER : issuerConfig;
        }

        public static string GetTokenAudience(IConfiguration configuration)
        {
            var audienceConfig = configuration["Audience"];
            return string.IsNullOrWhiteSpace(audienceConfig) ? FALLBACK_AUDIENCE : audienceConfig;
        }

        public static TimeSpan GetTokenExpirationShort(IConfiguration configuration)
        {
            var expirationInMinutesConfig = configuration["ExpirationInMinutes"];
            int parse;
            var expirationInMinutes = int.TryParse(expirationInMinutesConfig, out parse) ? parse : FALLBACK_EXPIRATION_IN_MINUTES;
            return TimeSpan.FromMinutes(expirationInMinutes);
        }

        public static TimeSpan GetTokenExpirationLong(IConfiguration configuration)
        {
            var expirationInDaysConfig = configuration["ExpirationInDays"];
            int parse;
            var expirationInDays = int.TryParse(expirationInDaysConfig, out parse) ? parse : FALLBACK_EXPIRATION_IN_DAYS;
            return TimeSpan.FromDays(expirationInDays);
        }

        public static SymmetricSecurityKey GetTokenSigningKey(IConfiguration configuration)
        {
            var applicationSecretConfig = configuration["ApplicationSecret"];
            var applicationSecret = string.IsNullOrWhiteSpace(applicationSecretConfig) ? FALBACK_APPLICATION_SECRET : applicationSecretConfig;
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(applicationSecret));
        }

        public static SigningCredentials GetTokenSigningCredentials(IConfiguration configuration)
        {
            var key = GetTokenSigningKey(configuration);
            return new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        }

        public static TokenValidationParameters GetTokenValidationParameters(IConfiguration configuration, bool validateLifetime = true)
        {
            return new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = GetTokenSigningKey(configuration),
                ValidateIssuer = true,
                ValidIssuer = GetTokenIssuer(configuration),
                ValidateAudience = true,
                ValidAudience = GetTokenAudience(configuration),
                ValidateLifetime = validateLifetime,
                ClockSkew = TimeSpan.FromSeconds(1)
            };
        }
    }

}
