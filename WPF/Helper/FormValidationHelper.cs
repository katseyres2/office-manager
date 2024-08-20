using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WPF.Constant;

namespace WPF.Helper
{
    public class FormValidationHelper
    {
        public static bool IsEmpty(string value)
        {
            return value.Length == 0;
        }

        public static bool MaxLengthOverflow(string value)
        {
            return value.Length > FormConstant.maxLength;
        }
    }
}
