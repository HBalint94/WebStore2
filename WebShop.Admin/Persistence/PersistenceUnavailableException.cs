using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShop.Admin.Persistence
{
    public class PersistenceUnavailableException : Exception
    {
        public PersistenceUnavailableException(String message) : base(message) { }

        public PersistenceUnavailableException(Exception innerException) : base("Exception occurred.", innerException) { }
    }
}
