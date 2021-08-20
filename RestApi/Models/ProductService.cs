using System.Collections.Generic;
using System.Linq;
using System;

using System.Net.Http;
using System.IO;
using System.Net;

namespace RestApi.Models
{
    
    public class ProductService : IProductService
    {
        private readonly ProductContext productContext;

        public ProductService(ProductContext context)
        {
            productContext = context;
        }
        public IEnumerable<Product> GetProducts()
        {
            return productContext.Products.OrderBy(o=>o.Description).ToList();
        }
        public bool IsSoftplan(string description)
        {
            if(description!=null)
            {
                return description.Contains("Softplayer");;
            }
            return false;  
        }
        public (bool, string) CheckParameters(Product product)
        {
            if(product.Description==null)
                return (false,"O preenchimento da descrição é obrigatório");
            else if(product.Description.Length>50)
                return (false,"O tamanho máximo da descrição é de 50 caracteres");
            else if(product.Category==null)
                return (false,"O preenchimento da categoria é obrigatório");
            else if(product.Cost<=0)
                return (false,"O preenchimento do preço de custo é obrigatório");
            return (true,"O produto foi salvo com sucesso");
        }

        public void InsertProduct(Product product)
        {
            product.Date = DateTime.Now;
            product.Price = GetPrice(product);
            productContext.Add(product);
            productContext.SaveChanges();
        }

        public virtual float GetPrice(Product product)
        {
            string url = "http://localhost:5000/api01/price?category="+product.Category+"&cost="+product.Cost;
            var requisicaoWeb = WebRequest.CreateHttp(url);         
            requisicaoWeb.Method = "GET";
            requisicaoWeb.UserAgent = "RequisicaoWebDemo";
            float price = 0;
            using (var resposta = requisicaoWeb.GetResponse())
            {
                var streamDados = resposta.GetResponseStream();
                StreamReader reader = new StreamReader(streamDados);
                string response = reader.ReadToEnd().ToString();
                streamDados.Close();
                resposta.Close();
                price = float.Parse(response);
            }
            
            return price;
        }

    }
}