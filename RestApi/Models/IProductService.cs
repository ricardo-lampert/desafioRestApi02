using System.Collections.Generic;
using FluentValidation.Results;

namespace RestApi.Models
{
    public interface IProductService
    {
        List<Product> GetProducts();
        (bool,string) InsertProduct(Product product);
    }
}
