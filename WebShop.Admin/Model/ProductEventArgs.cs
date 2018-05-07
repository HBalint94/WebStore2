using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShop.Admin.Model
{
    public class ProductEventArgs : EventArgs
    {
        public int PorductModellNumber { get; set; }
    }
}
