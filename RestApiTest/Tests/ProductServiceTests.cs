using Microsoft.EntityFrameworkCore;
using Moq;
using RestApi.Models;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace RestApiTest.Tests
{
    public class ProductServiceTest
    {
        private ProductService productService;
        private ProductContext context;
        private Mock<IProductGateway> mock;

        public ProductServiceTest()
        {
            var options = new DbContextOptionsBuilder<ProductContext>()
                .UseInMemoryDatabase(databaseName: "TestService")
                .Options;
            context = new ProductContext(options);
            mock = new Mock<IProductGateway>();
            productService = new ProductService(context, mock.Object);
        }

        [Fact]
        public void GetProductsMustReturnListOfProducts()
        {
            var result = productService.GetProducts();
            Assert.IsType<List<Product>>(result);
        }

        [Fact]
        public void GetProductsMustReturnSortedListOfProducts()
        {
            bool alphabetical = true;
            mock.Setup(x => x.GetPrice(It.IsAny<string>(), It.IsAny<float>())).Returns(100);
            context.Add(new Product { Description = "Speaker"});
            context.Add(new Product { Description = "Chapéu"});
            context.Add(new Product { Description = "Rubix"});
            context.SaveChanges();
            var result = productService.GetProducts();
            for(int i=0; i<result.Count-1;i++)
            {
                if (string.Compare(result[i].Description, result[i + 1].Description) > 0)
                {
                    alphabetical = false;
                    break;
                }
            }
            Assert.True(alphabetical);
        }

        [Fact]
        public void InsertProductMustReturnBoolAndString()
        {
            mock.Setup(x => x.GetPrice(It.IsAny<string>(), It.IsAny<float>())).Returns(100);
            Product product = new Product { Description = "Caneta", Cost = 50, Category = "Escritório" };
            (bool ok, string message) = productService.InsertProduct(product);
            Assert.IsType<bool>(ok);
            Assert.IsType<string>(message);
        }

        [Fact]
        public void InsertProductMustReturnTrueAndSucessMessage()
        {
            string sucessMessage = "O produto foi salvo com sucesso"; 
            mock.Setup(x => x.GetPrice(It.IsAny<string>(), It.IsAny<float>())).Returns(100);
            Product product = new Product { Description = "Liquidificador", Cost = 50, Category = "Eletrodomesticos" };
            (bool ok, string message) = productService.InsertProduct(product);
            Assert.True(ok);
            Assert.Equal(message,sucessMessage);
        }

        [Fact]
        public void InsertProductMustReturnFalseAndErrorMessage()
        {
            string sucessMessage = "O produto foi salvo com sucesso"; 
            Product product = new Product {Cost = 50, Category = "Eletrodomesticos" };
            (bool ok, string message) = productService.InsertProduct(product);
            Assert.False(ok);
            Assert.NotEqual(message,sucessMessage);
        }

        [Fact]
        public void InsertProductMustInsertProductOnDtabase()
        {
            var productsList = context.Products.ToList();
            mock.Setup(x => x.GetPrice(It.IsAny<string>(), It.IsAny<float>())).Returns(100);
            Product product = new Product { Description = "Liquidificador", Cost = 50, Category = "Eletrodomesticos" };
            (bool ok, string message) = productService.InsertProduct(product);
            var newProductsList = context.Products.ToList();
            Assert.Equal(productsList.Count + 1, newProductsList.Count);
        }

    }
}
//         [Fact]
//         public void IsSoftplanTest()
//         {
//             bool expected = true;
//             bool actual = productService.IsSoftplan("Eu sou um Softplayer");
//             Assert.Equal(expected, actual);
//             expected = false;
//             actual = productService.IsSoftplan("Eu não trabalho na Softplan");
//             Assert.Equal(expected, actual);
//         }
//         [Fact]
//         public void CheckParametersTest(){
//             // Inserção normal deve cadastrar
//             string expectedString = "O produto foi salvo com sucesso";
//             bool expecteBool = true;
//             (bool actualBool, string actualString) = productService.CheckParameters(new Product {Description= "Panela Tramontina",Cost= 100,Category= "Panelas"});
//             Assert.Equal(expectedString, actualString);            
//             Assert.Equal(expecteBool, actualBool);   
//             // Inserção sem descrição, deve retornar um erro
//             expectedString = "O preenchimento da descrição é obrigatório";
//             expecteBool = false;
//             (actualBool, actualString) = productService.CheckParameters(new Product {Cost= 20,Category= "Bebidas"});
//             Assert.Equal(expectedString, actualString);            
//             Assert.Equal(expecteBool, actualBool);   
//             // Inserção com descrição maior que limite, deve retornar um erro
//             expectedString = "O tamanho máximo da descrição é de 50 caracteres";
//             (actualBool, actualString) = productService.CheckParameters(new Product {Description= "Baralho de cartas da famosa e renomada marca Bycicle",Cost= 20,Category= "Brinquedos"});
//             Assert.Equal(expectedString, actualString);            
//             Assert.Equal(expecteBool, actualBool);   
//             // Inserção sem categoria, deve retornar um erro
//             expectedString = "O preenchimento da categoria é obrigatório";
//             (actualBool, actualString) = productService.CheckParameters(new Product {Description= "Panos",Cost= 20});
//             Assert.Equal(expectedString, actualString);            
//             Assert.Equal(expecteBool, actualBool);   
//             // Inserção com custo 0, deve retornar um erro
//             expectedString = "O preenchimento do preço de custo é obrigatório";
//             (actualBool, actualString) = productService.CheckParameters(new Product {Description= "Detergente",Cost= 0,Category= "Limpeza"});
//             Assert.Equal(expectedString, actualString);            
//             Assert.Equal(expecteBool, actualBool);   
//             // Inserção com custo negativo, deve retornar um erro
//             (actualBool, actualString) = productService.CheckParameters(new Product {Description= "Bala",Cost= -5,Category= "Comidas"});
//             Assert.Equal(expectedString, actualString);            
//             Assert.Equal(expecteBool, actualBool);   
//         }
//         [Fact]
//         public void InsertAndGetProduct()
//         {
            
//             List<Product> expectedList = new List<Product>();
//             expectedList.Add(new Product {Id=1,Description= "Panela Tramontina",Cost= 100,Category= "Panelas",Price= 100});
//             productService.InsertProduct(new Product {Description= "Panela Tramontina",Cost= 100,Category= "Panelas"});
//             var actualList = productService.GetProducts().ToList();
//             for(int i = 0; i < expectedList.Count; i++)
//             {
//                 Assert.Equal(expectedList[i].Id, actualList[i].Id);
//                 Assert.Equal(expectedList[i].Description, actualList[i].Description);
//                 Assert.Equal(expectedList[i].Category, actualList[i].Category);
//                 Assert.Equal(expectedList[i].Price, actualList[i].Price);
//             }
//         }
//     }
// }