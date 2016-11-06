using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Z_Market.Models;

namespace Z_Market.ViewModels
{
    public class OrderView
    {
        public Customers Customer { get; set; }
        public ProductOrder ProductOrder { get; set; }
        public List<ProductOrder> Products { get; set; }
        
    }
}