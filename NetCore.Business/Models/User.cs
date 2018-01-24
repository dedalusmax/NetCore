using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NetCore.Business.Models
{
    public class User : UserBase
    {
        public long Id { get; set; }
    }

    public class UserBase
    {
        public UserBase()
        {
            CountryIds = new HashSet<long>();
            DivisionIds = new HashSet<long>();
        }

        [Required]
        public long? RoleId { get; set; }

        public long? ParentUserId { get; set; }

        [Required, StringLength(255)]
        public string UserName { get; set; }

        [Required, StringLength(255)]
        public string Email { get; set; }

        [StringLength(255)]
        public string DisplayName { get; set; }

        public ICollection<long> CountryIds { get; set; }

        public ICollection<long> DivisionIds { get; set; }
    }
}
