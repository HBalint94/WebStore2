using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebStore.Models
{
    
    public class RentProductConnection
    {
        [Key]
        public int Id { get; set; }
        public int ProductModellNumber { get; set; }
        public int CountProduct { get; set; }
        [ForeignKey("Rent")]
        public int RentId { get; set; }
    }
}
