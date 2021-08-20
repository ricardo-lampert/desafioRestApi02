using RestApi.Controllers;
using RestApi.Models;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace RestApiTest.ControlerTests
{
    public class ProductControllerTest
    {
        private ProductController productController;
        private ProductContext context;

        public class FakeProductService : ProductService
        {
            public FakeProductService(ProductContext productContext) : base(productContext){}
            public override float GetPrice(Product product)
            {
                return 100;
            }
        }

        public ProductControllerTest()
        {
            var options = new DbContextOptionsBuilder<ProductContext>()
                .UseInMemoryDatabase(databaseName: "ProductListTestControler")
                .Options;
            context = new ProductContext(options);
            var productService = new FakeProductService(context);
            this.productController = new ProductController(context,productService);
        }

        [Fact]
        public void InsertAndGetProductsTest()
        {
            // Inserção normal deve cadastrar
            string expected = "O produto foi salvo com sucesso";
            var result = productController.InsertProduct(new Product {Description= "Panela Tramontina",Cost= 100,Category= "Panelas"}).Result as OkObjectResult;
            Assert.IsType<OkObjectResult>(result);
            var actual = Assert.IsType<string>(result.Value);
            Assert.Equal(expected, actual);            
            // Inserção com Softplayer na descrição, deve alterar a categoria para softplan e cadastrar
            result = productController.InsertProduct(new Product {Description= "Softplayer bonus",Cost= 400,Category= "Funcionario"}).Result as OkObjectResult;
            Assert.IsType<OkObjectResult>(result);
            actual = Assert.IsType<string>(result.Value);
            Assert.Equal(expected, actual);
            // Inserção com Softplayer na descrição, deve inserir a categoria para softplan e cadastrar
            result = productController.InsertProduct(new Product {Description= "Softplayer salario",Cost= 2000}).Result as OkObjectResult;
            Assert.IsType<OkObjectResult>(result);
            actual = Assert.IsType<string>(result.Value);
            Assert.Equal(expected, actual);
            // Inserção normal deve cadastrar (para verificar a ordem de exibição depois)
            result = productController.InsertProduct(new Product {Description= "Rubix",Cost= 20,Category= "Brinquedos"}).Result as OkObjectResult;
            Assert.IsType<OkObjectResult>(result);
            actual = Assert.IsType<string>(result.Value);
            Assert.Equal(expected, actual);
            // Inserção sem descrição, deve retornar um erro
            expected = "O preenchimento da descrição é obrigatório";
            var resultErro = productController.InsertProduct(new Product {Cost= 20,Category= "Bebidas"}).Result as BadRequestObjectResult;
            Assert.IsType<BadRequestObjectResult>(resultErro);
            actual = Assert.IsType<string>(resultErro.Value);
            Assert.Equal(expected, actual);
            // Inserção com descrição maior que limite, deve retornar um erro
            expected = "O tamanho máximo da descrição é de 50 caracteres";
            resultErro = productController.InsertProduct(new Product {Description= "Baralho de cartas da famosa e renomada marca Bycicle",Cost= 20,Category= "Brinquedos"}).Result as BadRequestObjectResult;
            Assert.IsType<BadRequestObjectResult>(resultErro);
            actual = Assert.IsType<string>(resultErro.Value);
            Assert.Equal(expected, actual);
            // Inserção sem categoria, deve retornar um erro
            expected = "O preenchimento da categoria é obrigatório";
            resultErro = productController.InsertProduct(new Product {Description= "Panos",Cost= 20}).Result as BadRequestObjectResult;
            Assert.IsType<BadRequestObjectResult>(resultErro);
            actual = Assert.IsType<string>(resultErro.Value);
            Assert.Equal(expected, actual);
            // Inserção com custo 0, deve retornar um erro
            expected = "O preenchimento do preço de custo é obrigatório";
            resultErro = productController.InsertProduct(new Product {Description= "Detergente",Cost= 0,Category= "Limpeza"}).Result as BadRequestObjectResult;
            Assert.IsType<BadRequestObjectResult>(resultErro);
            actual = Assert.IsType<string>(resultErro.Value);
            Assert.Equal(expected, actual);
            // Inserção com custo negativo, deve retornar um erro
            resultErro = productController.InsertProduct(new Product {Description= "Bala",Cost= -5,Category= "Comidas"}).Result as BadRequestObjectResult;
            Assert.IsType<BadRequestObjectResult>(resultErro);
            actual = Assert.IsType<string>(resultErro.Value);
            Assert.Equal(expected, actual);
        
        
            List<Product> expectedList = new List<Product>();
            expectedList.Add(new Product {Id=1,Description= "Panela Tramontina",Cost= 100,Category= "Panelas",Price= 100});
            expectedList.Add(new Product {Id=4,Description= "Rubix",Cost= 20,Category= "Brinquedos",Price= 100});
            expectedList.Add(new Product {Id=2,Description= "Softplayer bonus",Cost= 400,Category= "Softplan",Price= 100});
            expectedList.Add(new Product {Id=3,Description= "Softplayer salario",Cost= 2000,Category= "Softplan",Price= 100});

            var resultList = productController.GetProducts().Result as OkObjectResult;
            Assert.IsType<OkObjectResult>(resultList);
            var actualList = Assert.IsType<List<Product>>(resultList.Value);
            Assert.Equal(expectedList.Count, actualList.Count);
            for (int i = 0; i < expectedList.Count; i++)
            {
                Assert.Equal(expectedList[i].Id, actualList[i].Id);
                Assert.Equal(expectedList[i].Description, actualList[i].Description);
                Assert.Equal(expectedList[i].Category, actualList[i].Category);
                Assert.Equal(expectedList[i].Price, actualList[i].Price);
            }        
        }

    }
}