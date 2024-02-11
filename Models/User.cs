using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace retail_management.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public required string name { get; set; }
        public required string contactNumber { get; set; }
        public required string email { get; set; }
        public required string password { get; set; }
        public bool Status { get; set; }
        public required string role { get; set; }
    }
}
