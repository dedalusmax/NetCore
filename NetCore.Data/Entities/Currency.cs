using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetCore.Data.Entities
{
    public class Currency
    {

        [Key]
        public long Id { get; set; }

        [Required, MaxLength(255)]
        public string DisplayName { get; set; }

        [Required]
        public decimal ToUSD { get; set; }

        [Required]
        public decimal FromUSD { get; set; }

        [ForeignKey("CurrencySettings")]
        public long? CurrencySettingsId { get; set; }

        //public virtual CurrencySettings CurrencySettings { get; set; }
    }
}