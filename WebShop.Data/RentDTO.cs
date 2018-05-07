using System;
using System.Collections.Generic;
using System.Text;

namespace WebShop.Data
{
    public class RentDTO
    {
        public int Id { get; set; }

        public String UserName { get; set; }

        public String Address { get; set; }

        public String PhoneNumber { get; set; }

        public String Email { get; set; }

        public int TotalPrice { get; set; }

        public Boolean Performed { get; set; }

        public override Boolean Equals(Object obj)
        {
            return (obj is RentDTO dto) && Id == dto.Id;
        }
    }
}
