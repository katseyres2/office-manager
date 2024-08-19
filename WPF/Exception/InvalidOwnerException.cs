using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF.Exception
{
    public class InvalidOwnerException : ApplicationException
    {
        public InvalidOwnerException(string message = "Invalid owner!") : base(message) { }
    }
}
