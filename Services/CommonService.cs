using retail_management.Data;
using retail_management.Models;
using retail_management.Dtos;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Threading.Tasks;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace retail_management.Services
{
    public class CommonService : ICommonService
    {
        private readonly AppDbContext _db;

        public CommonService(AppDbContext db)
        {
            _db = db;
        }

        private bool VerifyPassword(string inputPassword, string storedPassword)
        {
            try
            {
                bool passwordMatch = BCrypt.Net.BCrypt.Verify(inputPassword, storedPassword);

                return passwordMatch;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<User> LoginAsync(LoginInputDto user)
        {
            try
            {

                var existingUser = await _db.Users.FirstOrDefaultAsync(u => u.email == user.email);

                if (existingUser != null && VerifyPassword(user.password, existingUser.password))
                {
                    return existingUser;
                }

                return null; 
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<Invoice> GenerateInvoiceAsync(InvoiceInputDto invoiceInput)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {

                   
                    var transactionEntity = new Transaction
                    {
                        customerName = invoiceInput.customerName,
                        contactNumber = invoiceInput.contactNumber,
                        transactionDate = DateTime.Now,
                        totalDiscount = invoiceInput.discount,
                    };

                   
                    await _db.Transactions.AddAsync(transactionEntity);
                    await _db.SaveChangesAsync();

                    foreach (var invoiceItem in invoiceInput.details)
                    {
                      
                        var product = await _db.Products.FindAsync(invoiceItem.productCode);

                        if (product != null)
                        {
                         
                            var transactionDetail = new TransactionDetail
                            {
                                transactionId = transactionEntity.id,
                                productId = product.id,
                                quantity = invoiceItem.qty,
                                subTotal = invoiceItem.total,
                                transaction = transactionEntity,
                                product = product
                            };

                          
                            await _db.TransactionDetails.AddAsync(transactionDetail);
                            await _db.SaveChangesAsync();

                        }
                    }

                  
                    var invoice = new Invoice
                    {
                        transactionId = transactionEntity.id,
                        totalAmount = invoiceInput.total,
                        balanceAmount = invoiceInput.netTotal,
                        discountPercentage = invoiceInput.percentageControl,
                        transaction = transactionEntity,
                    };

          
                    await _db.Invoices.AddAsync(invoice);
                    await _db.SaveChangesAsync();

               
                    var pdfPath = await GeneratePdfInvoiceAsync(invoice, invoiceInput);
             
                    invoice.filePath = pdfPath;
                    await _db.SaveChangesAsync();
                   
             
                    transaction.Commit();

                    return invoice;
                }
                catch (Exception ex)
                {
                  
                    transaction.Rollback();
                    return null;
                }
            }
        }

        private Task<string> GeneratePdfInvoiceAsync(Invoice invoice, InvoiceInputDto invoiceInput)
        {
            try
            {
                var pdfPath = $"Invoice_{invoice.id}.pdf";
                using (var stream = new FileStream(pdfPath, FileMode.Create))
                {
                    var writer = new PdfWriter(stream);
                    var pdf = new PdfDocument(writer);
                    var document = new Document(pdf);


                    document.Add(new Paragraph("MC COMPUTERS"));
                    document.Add(new Paragraph("Address: No 03, Colombo 07"));
                    document.Add(new Paragraph("Contact Number: +94761234565"));


                    document.Add(new Paragraph());


                    document.Add(new Paragraph($"Invoice ID: {invoice.id}"));
                    document.Add(new Paragraph($"Customer Name: {invoiceInput.customerName}"));


                    document.Add(new Paragraph());

                    document.Add(new Paragraph("Product Details:"));

                    Table table = new Table(6);
                    table.AddCell("Sl.No");
                    table.AddCell("Product Code");
                    table.AddCell("Description");
                    table.AddCell("Qty");
                    table.AddCell("Unit Price");
                    table.AddCell("Total");

                    for (int i = 0; i < invoiceInput.details.Count; i++)
                    {
                        var item = invoiceInput.details[i];
                        table.AddCell((i + 1).ToString());
                        table.AddCell(item.productCode.ToString());
                        table.AddCell(item.productName);
                        table.AddCell(item.qty.ToString());
                        table.AddCell(item.salesPrice.ToString());
                        table.AddCell(item.total.ToString());
                    }

                    document.Add(table);

                    document.Add(new Paragraph());


                    document.Add(new Paragraph($"Total: {invoiceInput.total}"));
                    document.Add(new Paragraph($"Discount Percentage(%): {invoiceInput.percentageControl}"));
                    document.Add(new Paragraph($"Discount: {invoiceInput.discount}"));
                    document.Add(new Paragraph($"Net Total: {invoiceInput.netTotal}"));


                    document.Close();
                }

                return Task.FromResult(pdfPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error generating PDF: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                return Task.FromResult<string>(null);
            }
        }

        public async Task<byte[]> GetInvoicePdfAsBlob(int invoiceId)
        {
            try
            {
                var invoice = await _db.Invoices.FindAsync(invoiceId);
                string filePath = invoice.filePath;

                if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
                {
                    return null; 
                }

                // Read the file as bytes
                byte[] fileBytes = await File.ReadAllBytesAsync(filePath);

                return fileBytes;
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return null;
            }
        }

        public async Task<List<Invoice>> GetAllInvoiceAsync()
        {
            try
            {
                return await _db.Invoices.Include(i => i.transaction)
                    .Where(i => i.isDeleted == false)
                    .OrderByDescending(i => i.id).ToListAsync();
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public async Task<Invoice> DeleteInvoiceAsync(int id)
        {
            try
            {
                var invoice = await _db.Invoices.FindAsync(id);

                if (invoice != null)
                {
                    invoice.isDeleted = true;

                    await _db.SaveChangesAsync();

                    return invoice;
                }

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            try
            {
                return await _db.Products.ToListAsync();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            try
            {
              
                return await _db.Products.FindAsync(id);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Product> AddProductAsync(ProductInputDto productInput)
        {
            try
            {
                var product = new Product
                {
                    productName = productInput.productName,
                    description = productInput.description,
                    price = productInput.price,
                };

                await _db.Products.AddAsync(product);
                await _db.SaveChangesAsync();

                return product;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        // product delete implementing
        //public async Task<Product> DeleteProductAsync(int id)
        //{
        //    return null;
        //}

    }
}
