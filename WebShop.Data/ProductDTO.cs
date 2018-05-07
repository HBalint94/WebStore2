using System;
using System.Collections.Generic;
using System.Text;

namespace WebShop.Data
{
    public class ProductDTO
    {
        public int Id { get; set; }

        public int ModellNumber { get; set; }

        public String Producer { get; set; }

        public int CategoryId { get; set; }

        public int Price { get; set; }

        public int Inventory { get; set; }

        public Boolean Available { get; set; }

        public String Description { get; set; }

        /// <summary>
		/// Egyenlőségvizsgálat.
		/// </summary>
		public override Boolean Equals(Object obj)
        {
            return (obj is ProductDTO dto) && ModellNumber == dto.ModellNumber;
        }
    }
}
