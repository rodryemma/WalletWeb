using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class ValidationHelper
    {
        public static DateTime ValidarFecha(string fechaStr, DateTime? fechaMinima = null)
        {
            if (DateTime.TryParse(fechaStr, out DateTime fechaValida))
            {
                return fechaValida;
            }

            return fechaMinima ?? DateTime.MinValue;
        }

        public static bool CompararEnum<TEnum>(string valor, TEnum enumComparar) where TEnum : struct, Enum
        {
            if (Enum.TryParse<TEnum>(valor, ignoreCase: true, out var tipo))
            {
                return tipo.Equals(enumComparar);
            }
            return false;
        }
    }
}
