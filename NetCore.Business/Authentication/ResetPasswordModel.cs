using System.ComponentModel.DataAnnotations;

namespace NetCore.Business.Authentication
{
    public class ResetPasswordModel
    {
        [Required]
        public string Username { get; set; }
    }
}
