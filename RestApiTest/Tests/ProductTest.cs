using Microsoft.EntityFrameworkCore;
using Moq;
using RestApi.Models;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace RestApiTest.Tests
{
    public class ProductTest
    {
        private Product product;
        private Product.Validator validator;

        public ProductTest()
        {
            validator = new Product.Validator();
            product = new Product();
        }

        [Fact]
        public void ValidatorMustReturnValid()
        {
            Product product = new Product { Description = "Lambreta", Cost = 5000, Category = "Veículo" };
            var results = validator.Validate(product);
            Assert.True(results.IsValid);
        }

        [Fact]
        public void ValidatorMustReturnNotValidBecauseDescriptionNull()
        {
            string expectedError = "O preenchimento da descrição é obrigatório";
            Product product = new Product {Cost = 5000, Category = "Veículo" };
            var results = validator.Validate(product);
            Assert.False(results.IsValid);
            Assert.Equal(expectedError, results.Errors[0].ErrorMessage);
        }

        [Fact]
        public void ValidatorMustReturnNotValidBecauseCategoryNull()
        {
            string expectedError = "O preenchimento da categoria é obrigatório";
            Product product = new Product { Description = "Lambreta", Cost = 5000};
            var results = validator.Validate(product);
            Assert.False(results.IsValid);
            Assert.Equal(expectedError, results.Errors[0].ErrorMessage);
        }

        [Fact]
        public void ValidatorMustReturnNotValidBecauseCostEmpty()
        {
            string expectedError = "O preenchimento do preço de custo é obrigatório";
            Product product = new Product { Description = "Lambreta", Category = "Veículo" };
            var results = validator.Validate(product);
            Assert.False(results.IsValid);
            Assert.Equal(expectedError, results.Errors[0].ErrorMessage);
        }

        [Fact]
        public void ValidatorMustReturnNotValidBecauseDescriptionTooLarge()
        {
            string expectedError = "O tamanho máximo da descrição é de 50 caracteres";
            Product product = new Product { Description = "LambretaLambretaLambretaLambretaLambretaLambretaLambreta", Cost = 5000, Category = "Veículo" };
            var results = validator.Validate(product);
            Assert.False(results.IsValid);
            Assert.Equal(expectedError, results.Errors[0].ErrorMessage);
        }

        [Fact]
        public void IsSoftplanMustReturnTrue()
        {
            string description = "Esse é um produto Softplayer";
            var isSoftplan = product.IsSoftplan(description);
            Assert.True(isSoftplan);
        }

        [Fact]
        public void IsSoftplanMustReturnFalse()
        {
            string description = "Esse não é um produto da Softplan";
            var isSoftplan = product.IsSoftplan(description);
            Assert.False(isSoftplan);
        }

        [Fact]
        public void UpdateCategoryMustReturnSameProduct()
        {
            string expectCategory = "Veículo";
            Product product = new Product { Description = "Lambreta que não é da sofplan", Category = "Veículo" };
            var result = product.UpdateCategory(product);
            Assert.Equal(expectCategory, result.Category);
        }

        [Fact]
        public void UpdateCategoryMustReturnUpdatedProduct()
        {
            string expectCategory = "Softplan";
            Product product = new Product { Description = "Lambreta de um Softplayer", Category = "Veículo" };
            var result = product.UpdateCategory(product);
            Assert.Equal(expectCategory, result.Category);
        }
    }
}
