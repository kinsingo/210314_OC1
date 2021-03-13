using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PNC_Csharp
{
    public static class Class_Extensions
    {
        //string's extension methods
        public static int Hex_string_To_Dec_int(this string s)
        {
            return Convert.ToInt32(s, 16);
        }

        //string's extension methods
        public static int Dec_string_To_Dec_int(this string s)
        {
            return Convert.ToInt32(s);
        }

        //int's extension methods
        public static string Dec_int_To_Dec_String(this int i)
        {
            return i.ToString();
        }

        //int's extension methods
        public static string Dec_int_To_Hex_String(this int i)
        {
            return i.ToString("X2");
        }
    }
}
