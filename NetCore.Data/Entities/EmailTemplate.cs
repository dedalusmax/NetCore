using NetCore.Data.Common;
using System.ComponentModel.DataAnnotations;

namespace NetCore.Data.Entities
{
    public class EmailTemplate
    {
        [Key]
        public long Id { get; set; }

        public EmailTemplateType Type { get; set; }

        [Required, MaxLength(255)]
        public string Subject { get; set; }

        [Required, MaxLength(4500)]
        public string Body { get; set; }
    }
}
