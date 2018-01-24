using System.ComponentModel.DataAnnotations;

namespace NetCore.Business.Authentication
{
    public class AuthenticationRefreshModel
    {
        [Required]
        public string Token { get; set; }

        [Required]
        public string RefreshToken { get; set; }
    }
}
