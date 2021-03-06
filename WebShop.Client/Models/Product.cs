﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebShop.Client.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public int ModellNumber { get; set; }
        public string Producer { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public int Price { get; set; }
        public int Inventory { get; set; }
        public Boolean Available { get; set; }
    }
}
