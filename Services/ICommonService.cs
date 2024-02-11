using retail_management.Dtos;
using retail_management.Models;

namespace retail_management.Services
{
    public interface ICommonService
    {

        // Login to system
        Task<User> LoginAsync(LoginInputDto user);

        // Generate invoice
        Task<Invoice> GenerateInvoiceAsync(InvoiceInputDto invoiceInput);
        Task<List<Invoice>> GetAllInvoiceAsync();

        Task<Invoice> DeleteInvoiceAsync(int id);

        Task<byte[]> GetInvoicePdfAsBlob(int id);


        // Get Products
        Task<List<Product>> GetAllProductsAsync();
        Task<Product> GetProductByIdAsync(int id);
        Task<Product> AddProductAsync(ProductInputDto productInput);
        //Task<Product> DeleteProductAsync(int id);


    }
}
