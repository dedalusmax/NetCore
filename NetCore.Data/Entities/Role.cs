using System.ComponentModel.DataAnnotations;

namespace NetCore.Data.Entities
{
    public class Role
    {
        //public const string SalesRep_NAME = "SalesRep";
        //public const string SalesRep_DISPLAY_NAME = "Sales rep";
        //public const RoleRelationCount SalesRep_COUNTRY_SPECIFIC = RoleRelationCount.Single;
        //public const RoleRelationCount SalesRep_DIVISION_SPECIFIC = RoleRelationCount.Single;
        //public const string SalesRep_PARENT_ROLE_NAME = RSM_NAME;

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
