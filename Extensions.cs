using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComManagement
{
    public static class Extensions
    {
        public static float ToFloat(this string input)
        {
            float tmp;
            float.TryParse(input, out tmp);
            return tmp;
        }
        public static double ToDouble(this string input)
        {
            double tmp;
            double.TryParse(input, out tmp);
            return tmp;
        }
    }
}
