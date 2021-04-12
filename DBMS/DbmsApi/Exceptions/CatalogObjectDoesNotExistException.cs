using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbmsApi.Exceptions
{
    public class CatalogObjectDoesNotExistException : Exception
    {
        public CatalogObjectDoesNotExistException() { }

        public CatalogObjectDoesNotExistException(string message) : base(message) { }

        public CatalogObjectDoesNotExistException(string message, Exception innerException) : base(message, innerException) { }
    }
}
