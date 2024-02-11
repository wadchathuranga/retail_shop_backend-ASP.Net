using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace retail_management.Models
{
    public class TransactionDetail
    {
        [Key]
        public int id { get; set; }

        public int transactionId { get; set; }

        public int productId { get; set; }

        public int quantity { get; set; }

        public decimal discount { get; set; }

        public decimal subTotal { get; set; }

        public required Transaction transaction { get; set; }
        public required Product product { get; set; }
    }
}
