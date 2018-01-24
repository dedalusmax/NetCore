using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using NetCore.Business.Models;
using System;
using System.Linq;
using System.Security.Claims;

namespace NetCore.Api.Config
{
    public class CurrentUserInfoFactory
    {
        public static readonly Func<IServiceProvider, CurrentUserInfo> Instance = provider =>
        {
            var user = provider.GetService<IHttpContextAccessor>()?.HttpContext?.User;

            if (user != null)
            {
                var userName = user.Identity?.Name;
                var userEmail = user.Claims?.SingleOrDefault(_ => _.Type == ClaimTypes.NameIdentifier)?.Value;
                var displayName = user.Claims?.SingleOrDefault(_ => _.Type == ClaimTypes.Name)?.Value;
                var userIdValue = user.Claims?.SingleOrDefault(_ => _.Type == TokenProvider.CLAIM_USERID)?.Value;
                var roles = user.Claims?.Where(_ => _.Type == ClaimTypes.Role)?.Select(_ => _.Value).ToArray();

                long userId;
                if (string.IsNullOrWhiteSpace(userIdValue) || !long.TryParse(userIdValue, out userId))
                    userId = 0;

                return new CurrentUserInfo(userId, userName, displayName, userEmail, roles);
            }

            return null;
        };
    }
}
