using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NetCore.Data.Entities
{
    public class UserCountry
    {
        [ForeignKey("User")]
        public long UserId { get; set; }

        [ForeignKey("Country")]
        public long CountryId { get; set; }

        public virtual User User { get; set; }

        public virtual Country Country { get; set; }
    }
}
