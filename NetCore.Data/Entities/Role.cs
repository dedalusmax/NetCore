using NetCore.Data.Common;
using System.ComponentModel.DataAnnotations;

namespace NetCore.Data.Entities
{
    public class Role
    {
        public const string Administrator_NAME = "Administrator";
        public const string Administrator_DISPLAY_NAME = "Administrator";
        public const RoleRelationCount Administrator_COUNTRY_SPECIFIC = RoleRelationCount.None;
        public const RoleRelationCount Administrator_DIVISION_SPECIFIC = RoleRelationCount.None;
        public const string Administrator_PARENT_ROLE_NAME = null;

        [Key]
        public long Id { get; set; }

        //[ForeignKey("ParentRole")]
        //public long? ParentRoleId { get; set; }

        [Required, MaxLength(255)]
        public string Name { get; set; }

        [Required, MaxLength(255)]
        public string DisplayName { get; set; }

        //public RoleRelationCount CountrySpecific { get; set; }

        //public RoleRelationCount DivisionSpecific { get; set; }

        //public virtual Role ParentRole { get; set; }
    }

}
