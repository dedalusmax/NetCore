using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NetCore.Data.Entities
{
    public class User
    {
        public User()
        {
            Countries = new HashSet<UserCountry>();
        }

        [Key]
        public long Id { get; set; }

        [ForeignKey("Role")]
        public long? RoleId { get; set; }

        [Required, MaxLength(255)]
        public string UserName { get; set; }

        [Required, MaxLength(255)]
        public string Email { get; set; }

        [MaxLength(255)]
        public string DisplayName { get; set; }

        [MaxLength(255)]
        public string PasswordHash { get; set; }

        [MaxLength(255)]
        public string PasswordSalt { get; set; }

        [MaxLength(255)]
        public string PasswordResetCode { get; set; }

        public DateTime? LastPasswordReset { get; set; }

        public virtual Role Role { get; set; }

        public virtual ICollection<UserCountry> Countries { get; set; }
    }

}
