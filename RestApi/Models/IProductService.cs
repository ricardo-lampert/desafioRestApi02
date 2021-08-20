using System.Collections.Generic;

namespace RestApi.Models
{
    public interface IProductService
    {
        IEnumerable<Product> GetProducts();
        void InsertProduct(Product product);
        bool IsSoftplan(string description);
        (bool, string) CheckParameters(Product product);
    }
}
