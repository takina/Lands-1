using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Lands.Helpers
{
    //Con esta clase, validamos el email con una expresión regular
    public static class RegexUtilities
    {
        public static bool IsValidEmail(string email)
        {
            //Esta expresión regular, valida si el correo tiene un fortmato válido. EJ: dshjds@gmail.com
            var expresion = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
            if (Regex.IsMatch(email, expresion))
            {
                if (Regex.Replace(email, expresion, String.Empty).Length == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

        }
    }
}
    
