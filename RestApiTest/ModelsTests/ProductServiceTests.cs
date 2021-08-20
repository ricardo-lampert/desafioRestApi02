using RestApi.Controllers;
using RestApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Xunit;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace RestApiTest.ModelsTests
{
    public class ProductServiceTest
    {
        private ProductService productService;
        private ProductContext context;
        public class FakeProductService : ProductService
        {
            public FakeProductService(ProductContext productContext) : base(productContext){}
            public override float GetPrice(Product product)
            {
                return 100;
            }
        }
        public ProductServiceTest()
        {
            var options = new DbContextOptionsBuilder<ProductContext>()
                .UseInMemoryDatabase(databaseName: "ProductListTestService")
                .Options;
            context = new ProductContext(options);
            productService = new FakeProductService(context);
        }
        [Fact]
        public void IsSoftplanTest()
        {
            bool expected = true;
            bool actual = productService.IsSoftplan("Eu sou um Softplayer");
            Assert.Equal(expected, actual);
            expected = false;
            actual = productService.IsSoftplan("Eu não trabalho na Softplan");
            Assert.Equal(expected, actual);
        }
        [Fact]
        public void CheckParametersTest(){
            // Inserção normal deve cadastrar
            string expectedString = "O produto foi salvo com sucesso";
            bool expecteBool = true;
            (bool actualBool, string actualString) = productService.CheckParameters(new Product {Description= "Panela Tramontina",Cost= 100,Category= "Panelas"});
            Assert.Equal(expectedString, actualString);            
            Assert.Equal(expecteBool, actualBool);   
            // Inserção sem descrição, deve retornar um erro
            expectedString = "O preenchimento da descrição é obrigatório";
            expecteBool = false;
            (actualBool, actualString) = productService.CheckParameters(new Product {Cost= 20,Category= "Bebidas"});
            Assert.Equal(expectedString, actualString);            
            Assert.Equal(expecteBool, actualBool);   
            // Inserção com descrição maior que limite, deve retornar um erro
            expectedString = "O tamanho máximo da descrição é de 50 caracteres";
            (actualBool, actualString) = productService.CheckParameters(new Product {Description= "Baralho de cartas da famosa e renomada marca Bycicle",Cost= 20,Category= "Brinquedos"});
            Assert.Equal(expectedString, actualString);            
            Assert.Equal(expecteBool, actualBool);   
            // Inserção sem categoria, deve retornar um erro
            expectedString = "O preenchimento da categoria é obrigatório";
            (actualBool, actualString) = productService.CheckParameters(new Product {Description= "Panos",Cost= 20});
            Assert.Equal(expectedString, actualString);            
            Assert.Equal(expecteBool, actualBool);   
            // Inserção com custo 0, deve retornar um erro
            expectedString = "O preenchimento do preço de custo é obrigatório";
            (actualBool, actualString) = productService.CheckParameters(new Product {Description= "Detergente",Cost= 0,Category= "Limpeza"});
            Assert.Equal(expectedString, actualString);            
            Assert.Equal(expecteBool, actualBool);   
            // Inserção com custo negativo, deve retornar um erro
            (actualBool, actualString) = productService.CheckParameters(new Product {Description= "Bala",Cost= -5,Category= "Comidas"});
            Assert.Equal(expectedString, actualString);            
            Assert.Equal(expecteBool, actualBool);   
        }
        [Fact]
        public void InsertAndGetProduct()
        {
            
            List<Product> expectedList = new List<Product>();
            expectedList.Add(new Product {Id=1,Description= "Panela Tramontina",Cost= 100,Category= "Panelas",Price= 100});
            productService.InsertProduct(new Product {Description= "Panela Tramontina",Cost= 100,Category= "Panelas"});
            var actualList = productService.GetProducts().ToList();
            for(int i = 0; i < expectedList.Count; i++)
            {
                Assert.Equal(expectedList[i].Id, actualList[i].Id);
                Assert.Equal(expectedList[i].Description, actualList[i].Description);
                Assert.Equal(expectedList[i].Category, actualList[i].Category);
                Assert.Equal(expectedList[i].Price, actualList[i].Price);
            }
        }
    }
}