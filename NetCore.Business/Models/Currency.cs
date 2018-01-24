using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NetCore.Business.Models
{
    public class Currency
    {
        public long Id { get; set; }

        [Required, StringLength(255)]
        public string DisplayName { get; set; }

        [Required]
        public decimal ToUSD { get; set; }

        [Required]
        public decimal FromUSD { get; set; }
    }

    public class CurrencyWithSettings : Currency
    {
        public decimal Type365Threshold { get; set; }
    }
}
