using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;//Stream

namespace PNC_Csharp
{
    class Excel_Control
    {
        Microsoft.Office.Interop.Excel.Application oXL;
        Microsoft.Office.Interop.Excel._Workbook oWB;
        Microsoft.Office.Interop.Excel._Worksheet oSheet;
        string File_Address;//It only and must be set when this object is created;
        
        public Excel_Control(string File_Address,string sheetName)
        {
            oXL = new Microsoft.Office.Interop.Excel.Application();
            this.File_Address = File_Address;
            oWB = oXL.Workbooks.Open(File_Address);//it takes a lot of time, but only excuted one time 
            oSheet = String.IsNullOrEmpty(sheetName) ? (Microsoft.Office.Interop.Excel._Worksheet)oWB.ActiveSheet : (Microsoft.Office.Interop.Excel._Worksheet)oWB.Worksheets[sheetName];
        }
        ~Excel_Control()  // finalizer
        {
            Close_Excel_Connection();
        }
        public void Close_Excel_Connection()
        {
            if (oWB != null) oWB.Close();
        }

        public void UpdateExcelData<T>(int row, int col, T data)
        {
            try { oSheet.Cells[row, col] = data; }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); } 
        }

        public void Save_Data_to_Excel_File()//it takes some time
        {
            if (oWB != null) oWB.Save();
            else System.Windows.Forms.MessageBox.Show("oWB is null !");
        }
    }
}
