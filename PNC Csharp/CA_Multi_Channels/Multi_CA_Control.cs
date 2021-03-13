
using System.Windows.Forms;
using BSQH_Csharp_Library;
using CASDK2;

namespace PNC_Csharp.CA_Multi_Channels
{
    //asd
    public interface I_CA_Control
    {
        int Get_Probe_Count();
        bool connect_CA();
        XYLv[] Get_Multi_MeasuredData();
        void Zero_Cal();
        void Set_SyncMode(int syncmode, double freq = 60.0);
        void Set_MeasruemnetMode(int MeasuremntMode);
        void Set_White_Channel(int ca_ch);

        bool[,] Get_Connected_Probe_Channels();
    }


    interface I_CA310_Control
    {
    }

    interface I_CA410_Control
    {
        
    }

    abstract class Base_Multi_CA_Control
    {
        //others
        protected Form1 f1()
        {
            return (Form1)System.Windows.Forms.Application.OpenForms["Form1"];
        }

        protected bool GetErrorMessage(int errornum)
        {
            string errormessage = "";
            if (errornum != 0)
            {
                GlobalFunctions.CASDK2_GetLocalizedErrorMsgFromErrorCode(0, errornum, ref errormessage);
                MessageBox.Show(errormessage);
                return false;
            }
            return true;
        }
    }



    
}
