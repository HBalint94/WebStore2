using System;
using System.Collections.Generic;
using System.Text;

namespace WebShop.Data
{
    public class CategoryDTO
    {
        public int Id { get; set; }

        public String Name { get; set; }

        /// <summary>
        /// Egyenlőségvizsgálat.
        /// </summary>
        public override Boolean Equals(Object obj)
        {
            return (obj is CategoryDTO dto) && Id == dto.Id;
        }

        /// <summary>
        /// Szöveggé alakítás.
        /// </summary>
        public override String ToString()
        {
            return Name;
        }



    }
}
