using RestApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System;
using FluentValidation.Results;

namespace RestApi.Controllers
{
    static class Constants
    {
        public const string route = "api02";
        public const string insertEndpoint = "insert";
    }

    [Route(Constants.route)]
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
        [HttpPost(Constants.insertEndpoint)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<string> InsertProduct(Product product)
        {
            (bool ok,string message) = productService.InsertProduct(product);
            if(ok)
            {
                return Ok(message);
            }
            return BadRequest(message);

        }
    }
}
