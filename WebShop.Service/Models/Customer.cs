using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebStore.Models
{
    public class Customer : IdentityUser<int>
    {
        public string Name { get; set; }
        public string Address { get; set; }

    }
}
