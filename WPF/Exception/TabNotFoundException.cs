using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF.Exception
{
    public class TabNotFoundException : ApplicationException
    {
        public TabNotFoundException(string message = "Tab not found!") : base(message) { }
    }
}
