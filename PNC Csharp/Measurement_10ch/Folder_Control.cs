using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
////using References
using SectionLib;
using System.IO.MemoryMappedFiles;
using System.IO;

using System.Globalization;
using Microsoft.VisualBasic;

using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using BSQH_Csharp_Library;

namespace PNC_Csharp.Measurement_10ch
{
    static public class Folder_Control
    {
        static public string Make_new_folder(DateTime Start_Time)
        {
            string sDirPath;

            sDirPath = Directory.GetCurrentDirectory() + "\\Optic_Measurement_Data(10ch)\\" + Start_Time.ToString(@"yyyy_MM_dd_HH_mm", new CultureInfo("en-US"));

            DirectoryInfo di = new DirectoryInfo(sDirPath);

            if (di.Exists == false)
            {
                di.Create();
            }

            return sDirPath;
        }
    }
}
