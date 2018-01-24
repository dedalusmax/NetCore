using NetCore.Data.Entities;
using Microsoft.AspNetCore.Authorization;

namespace NetCore.Api.Config
{
    public class AuthorizeAdminAttribute : AuthorizeAttribute
    {
        public AuthorizeAdminAttribute()
            : base()
        {
            Roles = $"{Role.Administrator_NAME}";
        }
    }

    //public class AuthorizeDirectorAttribute : AuthorizeAttribute
    //{
    //    public AuthorizeDirectorAttribute()
    //        : base()
    //    {
    //        Roles = $"{Role.Administrator_NAME}, {Role.CountryFD_NAME}, {Role.DivisionalFD_NAME}, {Role.BUD_NAME}";
    //    }
    //}

    //public class AuthorizeManagerAttribute : AuthorizeAttribute
    //{
    //    public AuthorizeManagerAttribute()
    //        : base()
    //    {
    //        Roles = $"{Role.Administrator_NAME}, {Role.CountryFD_NAME}, {Role.DivisionalFD_NAME}, {Role.BUD_NAME}, {Role.FlexRFM_NAME}, {Role.RSM_NAME}";
    //    }
    //}
}
