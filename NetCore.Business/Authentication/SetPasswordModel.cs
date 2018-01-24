using System.ComponentModel.DataAnnotations;

namespace NetCore.Business.Authentication
{
    public class SetPasswordModel : AuthenticationModel
    {
        [Required]
        public string ResetCode { get; set; }
    }
}
