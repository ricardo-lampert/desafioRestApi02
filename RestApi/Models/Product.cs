using System;
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
    }
}

