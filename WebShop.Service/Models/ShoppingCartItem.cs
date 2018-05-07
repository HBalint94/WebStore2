using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Models;

namespace WebStore.Models
{
    public class ShoppingCartItem
    {
        public ShoppingCartItem() { }

        public ShoppingCartItem(int pmn,int q,string desc)
        {
            this.ProductModellNumber = pmn;
            this.Quantity = q;
            this.ProductDescription = desc;
        }
        public int ProductModellNumber{ get; set; }

        public string ProductDescription { get; set; }
        
        public int Quantity { get; set; }
    }
}
