using System;
using System.Collections.Generic;
using System.Text;

namespace EasyCode.EventBusService.Models
{
    public class ProductRequest
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
    }

    public class ProductResponse
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public double Price { get; set; }
    }
}
