namespace retail_management.Dtos
{
    public class ProductInputDto
    {
        public required string productName { get; set; }
        public string? description { get; set; }
        public decimal price { get; set; }
    }
}
