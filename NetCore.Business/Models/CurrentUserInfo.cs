using System;
using System.Linq;

namespace NetCore.Business.Models
{
    public class CurrentUserInfo
    {
        public long Id { get; }
        public string UserName { get; }
        public string UserEmail { get; }
        public string DisplayName { get; }
        public string[] Roles { get; }

        public CurrentUserInfo(long id, string userName, string displayName, string userEmail, string[] roles)
        {
            Id = id;
            UserName = userName;
            DisplayName = displayName;
            UserEmail = userEmail;
            Roles = roles;
        }

        public bool IsInRole(string role)
        {
            return this.Roles != null ? Array.IndexOf(this.Roles, role) >= 0 : false;
        }

        public bool IsInAnyRole(string[] roles)
        {
            return this.Roles != null ? this.Roles.Intersect(roles).Any() : false;
        }
    }
}
