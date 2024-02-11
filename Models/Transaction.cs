using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace retail_management.Models
{
    public class Transaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public required string customerName { get; set; }

        public string? contactNumber { get; set; }
        public DateTime transactionDate { get; set; }
        public decimal totalDiscount { get; set; }
    }
}
