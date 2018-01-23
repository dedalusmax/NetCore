using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NetCore.Data.Entities
{
    public class Country
    {
        [Key]
        public long Id { get; set; }

        [Required, MaxLength(255)]
        public string Name { get; set; }

        [ForeignKey("Currency")]
        public long? CurrencyId { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public virtual Currency Currency { get; set; }
    }
}
