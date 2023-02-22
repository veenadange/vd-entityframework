using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EntityFrameworkRls.Models
{
   // [RlsPolicy]
    public class TaxReturn
    {
        public Guid TenantId { get; set; }
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; }
        public string TaxYear { get; set; }
    }
}
