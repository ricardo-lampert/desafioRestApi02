using RestApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
namespace RestApi.Controllers
{
    [Route("api02")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService productService;
        public ProductController(ProductContext context, IProductService productService)
        {
            this.productService = productService;
        }
        [HttpGet]
        public ActionResult<IEnumerable<Product>> GetProducts()
        {
            return Ok(productService.GetProducts());
        }
        [HttpPost("insert")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<string> InsertProduct(Product product)
        {
            if (productService.IsSoftplan(product.Description))
            {
                product.Category = "Softplan";
            }
            (bool status, string message) = productService.CheckParameters(product);
            if (status)
            {
                productService.InsertProduct(product);
                return Ok(message);
            }
            
            return BadRequest(message);
        }
    }
}
