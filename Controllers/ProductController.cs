using Microsoft.AspNetCore.Mvc;
using retail_management.Dtos;
using retail_management.Models;
using retail_management.Services;

namespace retail_management.Controllers
{
    [ApiController]
    [Route("api/product")]
    public class ProductController: ControllerBase
    {
        private readonly ICommonService _commonService;
        public ProductController(ICommonService commonService)
        {
            _commonService = commonService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProductsAsync()
        {
            var products = await _commonService.GetAllProductsAsync();
            if (products == null)
            {
                return StatusCode(StatusCodes.Status204NoContent, "No products in database.");
            }

            return StatusCode(StatusCodes.Status200OK, products);
        }

        [HttpGet("id")]
        public async Task<IActionResult> GetProductByIdAsync(int id)
        {
            Product product = await _commonService.GetProductByIdAsync(id);

            if (product == null)
            {
                return StatusCode(StatusCodes.Status204NoContent, $"No product found for id: {id}");
            }

            return StatusCode(StatusCodes.Status200OK, product);
        }

        [HttpPost]
        public async Task<IActionResult> AddProductAsync(ProductInputDto productInput)
        {
            var products = await _commonService.AddProductAsync(productInput);

            if (products == null)
            {
                return StatusCode(StatusCodes.Status204NoContent, "Products not added!.");
            }

            return StatusCode(StatusCodes.Status200OK, products);
        }

        // Product delete end point

        //[HttpDelete]
        //public async Task<IActionResult> DeleteProductAsync(int id)
        //{
        //    var product = await _commonService.DeleteProductAsync(id);
        //    if (product == null)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, "Error occured");
        //    }

        //    return StatusCode(StatusCodes.Status200OK, product);
        //}

    }
}
