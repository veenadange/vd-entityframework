using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EntityFrameworkRls.Models
{
    //[RlsPolicy]
    public class Client
    {
        public Guid TenantId { get; set; }
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
    }
}
