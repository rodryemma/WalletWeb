using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common
{
    internal class MathHelper
    {
        public static T Dividir<T>(T numerator, T denominator) where T : struct
        {
            dynamic num = numerator;
            dynamic den = denominator;

            if (den == 0)
                return default;

            return (T)(num / den);
        }
    }
}
