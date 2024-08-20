using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF.Exception
{
    internal class ReservationOverrideException : ApplicationException
    {
        public ReservationOverrideException(string message = "The reservation override another one.") : base(message) { }
    }
}
