using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NetCore.Business.Models
{
    public class CountryBase
    {
        [Required, StringLength(255)]
        public string Name { get; set; }

        [Required]
        public long CurrencyId { get; set; }
    }

    public class Country : CountryBase
    {
        public long Id { get; set; }

        public decimal Type365Threshold { get; set; }
    }
}
