using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace retail_management.Models
{
    public class Invoice
    {
        [Key]
        public int id { get; set; }

        public int transactionId { get; set; }

        public decimal totalAmount { get; set; }

        public decimal balanceAmount { get; set; }

        public Boolean isDeleted { get; set; }

        public decimal discountPercentage { get; set; }

        public string? filePath { get; set; }

        public required Transaction transaction { get; set; }
    }
}
