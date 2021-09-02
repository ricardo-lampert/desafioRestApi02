using System;
using FluentValidation;

namespace RestApi.Models
{
    public class Product
    {
        public long Id { get; set; }
        public string Description { get; set; }
        public float Cost { get; set; }
        public string Category { get; set; }
        public DateTime Date { get; set; }
        public float Price { set; get; }
    
        public class Validator : AbstractValidator<Product> 
        {
            public Validator()
            {
                RuleFor(Product => Product.Description).NotNull().WithMessage("O preenchimento da descrição é obrigatório");
                RuleFor(Product => Product.Category).NotNull().WithMessage("O preenchimento da categoria é obrigatório");
                RuleFor(Product => Product.Cost).NotEmpty().WithMessage("O preenchimento do preço de custo é obrigatório");
                RuleFor(Product => Product.Description).MaximumLength(50).WithMessage("O tamanho máximo da descrição é de 50 caracteres");
            }
        }

        public bool IsSoftplan(string description)
        {
            if(description!=null)
            {
                return description.Contains("Softplayer");;
            }
            return false;  
        }

        public Product UpdateCategory(Product product)
        {
            if(IsSoftplan(product.Description))
            {
                product.Category = "Softplan";
            }
            return product;
        }
        
    }


}

