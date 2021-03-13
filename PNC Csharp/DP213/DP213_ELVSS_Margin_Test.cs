using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Timers;
using System.Threading.Tasks;
using System.IO;
using BSQH_Csharp_Library;

namespace PNC_Csharp
{
    public class DP213_ELVSS_Margin_Test : DP213_forms_accessor
    {
        DP213_OC_Current_Variables_Structure vars;
        DP213_CMDS_Write_Read_Update_Variables cmds;
        int Margin_Tested_band;

        public DP213_ELVSS_Margin_Test()
        {
            vars = DP213_OC_Current_Variables_Structure.getInstance();
            cmds = DP213_CMDS_Write_Read_Update_Variables.getInstance();
            Margin_Tested_band = dp213_form().Get_Margin_Tested_ELVSS_Band();
        }

        private void ELVSS_Margin_Test(Gamma_Set Set, int band)
        {
            for (double ELVSS_Voltage = -6.0; ((ELVSS_Voltage <= -0.8) && (vars.Optic_Compensation_Stop == false)); ELVSS_Voltage += 0.1)
            {
                cmds.Set_Voltage_ELVSS_and_and_Update_Textboxes(Set, band, ELVSS_Voltage);
                cmds.Send_ELVSS_CMD(Set);
                Thread.Sleep(50);
                f1().CA_Measure_For_ELVSS(ELVSS_Voltage.ToString());
            }
        }


        public void Set_and_Send_ELVSS_CMD(double ELVSS_Voltage, Gamma_Set Set, int band)
        {
            cmds.Set_Voltage_ELVSS_and_and_Update_Textboxes(Set, band, ELVSS_Voltage);
            cmds.Send_ELVSS_CMD(Set);
            Thread.Sleep(50);
        }



        public void ELVSS_Margin_Test_Start()
        {
            dp213_form().ELVSS_Margin_Test_Initialize();
            vars.Optic_Compensation_Stop = false;
            dp213_form().DP213_DBV_Setting(Margin_Tested_band);//DBV Setting

            //W (Set1)
            f1().dataGridView2.Rows.Add("W", "Set1", "-", "-");
            dp213_form().Set_Condition_Mipi_Script_Send(Gamma_Set.Set1);
            f1().PTN_update(255, 255, 255);
            Thread.Sleep(300);
            ELVSS_Margin_Test(Gamma_Set.Set1, Margin_Tested_band);

            //W (Set2)
            f1().dataGridView2.Rows.Add("W", "Set2", "-", "-");
            dp213_form().Set_Condition_Mipi_Script_Send(Gamma_Set.Set2);
            f1().PTN_update(255, 255, 255);
            Thread.Sleep(300);
            ELVSS_Margin_Test(Gamma_Set.Set2, Margin_Tested_band);

            //W (Set3)
            f1().dataGridView2.Rows.Add("W", "Set3", "-", "-");
            dp213_form().Set_Condition_Mipi_Script_Send(Gamma_Set.Set3);
            f1().PTN_update(255, 255, 255);
            Thread.Sleep(300);
            ELVSS_Margin_Test(Gamma_Set.Set3, Margin_Tested_band);
        }

    }
}
