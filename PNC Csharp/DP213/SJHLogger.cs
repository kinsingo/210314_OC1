using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PNC_Csharp
{
    static public class SJHLogger
    {
        public static StringBuilder Difference_Offset_Log12 = new StringBuilder();
        public static StringBuilder Difference_Offset_Log23 = new StringBuilder();
        public static void Clear()
        {
            Difference_Offset_Log12.Clear();
            Difference_Offset_Log23.Clear();
        }
    }
}
