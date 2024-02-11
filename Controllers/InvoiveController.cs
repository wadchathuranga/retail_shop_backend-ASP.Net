using Microsoft.AspNetCore.Mvc;
using retail_management.Dtos;
using retail_management.Services;

namespace retail_management.Controllers
{
    [ApiController]
    [Route("api/invoice")]
    public class InvoiceController: ControllerBase
    {
        private readonly ICommonService _commonService;
        public InvoiceController(ICommonService commonService)
        {
            _commonService = commonService;
        }

        [HttpPost]
        public async Task<IActionResult> GenerateInvoiceAsync([FromBody] InvoiceInputDto invoice)
        {
            var dbInvoice = await _commonService.GenerateInvoiceAsync(invoice);
        
            if (dbInvoice == null)
            {
                
                return StatusCode(StatusCodes.Status500InternalServerError, $"invoice could not be generated.");
            }
            return StatusCode(StatusCodes.Status200OK, dbInvoice);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllInvoicesAsync()
        {
            var invoices = await _commonService.GetAllInvoiceAsync();
            if (invoices == null)
            {
                return StatusCode(StatusCodes.Status204NoContent, "No invoices in database.");
            }

            return StatusCode(StatusCodes.Status200OK, invoices);
        }

        [HttpGet, Route("print")]
        public async Task<IActionResult> GetInvoicePdfAsBlob(int id)
        {
            try
            {
                var fileContent = await _commonService.GetInvoicePdfAsBlob(id);

                if (fileContent == null)
                {
                    return StatusCode(StatusCodes.Status204NoContent, "No invoices in the database.");
                }


                Response.Headers.Add("Content-Type", "application/pdf");
                Response.Headers.Add("Content-Disposition", $"attachment; filename=invoice_{id}.pdf");

  
                return File(fileContent, "application/pdf");
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteInvoiceAsync(int id)
        {
            var invoices = await _commonService.DeleteInvoiceAsync(id);
            if (invoices == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error occured");
            }

            return StatusCode(StatusCodes.Status200OK, invoices);
        }
    }
}
