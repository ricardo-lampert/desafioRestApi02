using System.Collections.Generic;
using System.Linq;
using System;
using System.IO;
using System.Net;
using FluentValidation.Results;
using FluentValidation;

namespace RestApi.Models
{
    
    public class ProductGateway : IProductGateway
    {
        public virtual float GetPrice(string category,float cost)
        {
            string url = "http://localhost:5000/api01/price?category="+category+"&cost="+cost;
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