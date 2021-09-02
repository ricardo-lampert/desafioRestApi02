using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Moq;
using RestApi.Controllers;
using RestApi.Models;
using System.Linq;
using Xunit;

namespace RestApiTest.Tests
{
    public class ProductControllerTest
    {

        private ProductController productController;
        private ProductContext context;
        private Mock<IProductGateway> mock;

        public ProductControllerTest()
        {
            var options = new DbContextOptionsBuilder<ProductContext>()
                .UseInMemoryDatabase(databaseName: "TestControler")
                .Options;
            context = new ProductContext(options);
            mock = new Mock<IProductGateway>();
            var productService = new ProductService(context, mock.Object);
            productController = new ProductController(context, productService);
        }

        [Fact]
        public void GetProductsMustReturnOk()
        {
            var result = productController.GetProducts().Result as OkObjectResult;
            Assert.IsType<OkObjectResult>(result);
        }


        [Fact]
        public void InsertProductMustReturnOk()
        {
            mock.Setup(x => x.GetPrice(It.IsAny<string>(), It.IsAny<float>())).Returns(100);
            Product product = new Product { Description = "Panela Tramontina", Cost = 100, Category = "Panelas" };
            var result = productController.InsertProduct(product).Result as OkObjectResult;
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void InsertProductMustReturnBadRequest()
        {
            Product product = new Product { Cost = 100, Category = "Joias" };
            var result = productController.InsertProduct(product).Result as BadRequestObjectResult;
            Assert.IsType<BadRequestObjectResult>(result);
        }

    }
}