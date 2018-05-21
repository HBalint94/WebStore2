using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebStore.Models
{
    public class RentViewModel : CustomerViewModel
    { 
        //Termékek
        public List<ShoppingCartItem> ShoppingCartItems { get; set; }

        // Teljes ár
        [DataType(DataType.Currency)]
        public int TotalPrice { get; set; }
        
    }
}
