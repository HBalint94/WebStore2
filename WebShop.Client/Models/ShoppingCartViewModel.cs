﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebShop.Client.Models
{ 
    public class ShoppingCartViewModel
    {

        public List<ShoppingCartItem> ShoppingCartItems { get; set; }

        public int ShoppingCartTotal { get; set; }

    }
}
