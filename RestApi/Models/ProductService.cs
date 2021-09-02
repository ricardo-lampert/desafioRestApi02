using System.Collections.Generic;
using System.Linq;
using System;
using System.IO;
using System.Net;
using FluentValidation.Results;

namespace RestApi.Models
{
    
    public class ProductService : IProductService
    {
        private readonly ProductContext context;

        private IProductGateway gateway;
        private Product.Validator validator;
        private Product product;

        public ProductService(ProductContext productContext, IProductGateway productGateway)
        {
            
            validator = new Product.Validator();
            product = new Product();
            
            gateway = productGateway;
            context = productContext;
        }

        public List<Product> GetProducts()
        {
            return context.Products.OrderBy(o=>o.Description).ToList();
        }

        public (bool,string) InsertProduct (Product product)
        {
            product = product.UpdateCategory(product);
            ValidationResult results = validator.Validate(product);
            if(!results.IsValid) 
            {
                return (false,results.Errors[0].ErrorMessage);
            }
            product.Date = DateTime.Now;
            product.Price = gateway.GetPrice(product.Category,product.Cost);
            context.Add(product);
            context.SaveChanges();
            return (true,"O produto foi salvo com sucesso");
        }
    }
}