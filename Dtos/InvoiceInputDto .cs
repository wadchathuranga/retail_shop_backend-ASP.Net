namespace retail_management.Dtos
{

    public class InvoiceItemDto
    {
        public int productCode { get; set; }

        public required string productName { get; set; }
        public int qty { get; set; }

        public decimal salesPrice { get; set; }

        public decimal total { get; set; }
    }
    public class InvoiceInputDto
    {
        public required string customerName { get; set; }

        public string? contactNumber { get; set; }
        public decimal discount { get; set; }
        public decimal netTotal { get; set; }
        public decimal total { get; set; }
        public decimal percentageControl { get; set; }
        public required List<InvoiceItemDto> details { get; set; }
    }
}
