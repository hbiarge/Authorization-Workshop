using System;
using System.Text;

namespace Products.Api.Models
{
    public class Product
    {
        public string ProductNumber { get; set; }
        
        public string OwnerId { get; set; }

        public string ProductName { get; set; }
    }
}
