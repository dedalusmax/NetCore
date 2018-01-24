using System.ComponentModel.DataAnnotations;

namespace NetCore.Business.Authentication
{
    public class AuthenticationModel
    {
        [Required]
        public string UserName { get; set; }

        [Required, MinLength(5)]
        public string Password { get; set; }
    }
}
