using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebStore.Models
{
    public class Rent
    {
        
        [Key]
        public int Id { get; set; } 
        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        public int TotalPrice { get; set; }
        public Boolean Performed { get; set; }
    }
}
