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
using System.Media;
using BSQH_Csharp_Library;

namespace PNC_Csharp
{

    public class DP213_Main_OC_Flow : DP213_forms_accessor
    {
        DP213_CMDS_Write_Read_Update_Variables cmds;
        DP213_OC_Values_Storage storage;
        DP213_OC_Current_Variables_Structure vars;
        DP213_CRC_Check crcs;
        public static Init_Gray_LUT init_gray_lut;
        public static InterpolationFomulaFactory interpolation_fomula_obj;

        public DP213_Main_OC_Flow()
        {
            cmds = DP213_CMDS_Write_Read_Update_Variables.getInstance();
            storage = DP213_OC_Values_Storage.getInstance();
            vars = DP213_OC_Current_Variables_Structure.getInstance();
            crcs = DP213_CRC_Check.getInstance(dp213_form().Get_OC_Mode_Set(OC_Mode.Mode1));
        }

        public void Start_OC()
        {
            //DP213_OC_Modes's Constructor
            //0. Storage data initializing (datas = 0)
            //1. Construct OC_Set_Mode1/2/3/4/5/6
            //2. DP213_Dual_Engineering_Mode_Show();
            //3. Set_Mornitoring_Mode_Background_Color() according to OC_Set_Mode's Current_Gamma_Set;
            //4. Set m.All_Band_Gray_Gammas[,,] by_reading_from_gridviews
            //5. Measure/Loopcount/ExtensionApplied Clear
            DP213_OC_Modes DP213_Objects = new DP213_OC_Modes();
            OC_Initialize();
            Optic_Compensation(DP213_Objects);
            Show_CMOTP_GMOTP_CRC_Check_Result(dp213_form().Get_OC_Mode_Set(OC_Mode.Mode1));
            OC_Finalize(DP213_Objects);
            Show_Logs();
        }

        private void Show_Logs()
        {
            if (f1().radioButton_Debug_Status_Mode.Checked && dp213_form().checkBox_Initial_RVreg1B_or_RGB_Algorithm_Apply.Checked)
            {
                DP213_Intial_Algorithm_RGBVreg1_CPP init_algorithm_storag = DP213_Intial_Algorithm_RGBVreg1_CPP.getInstance();
                
                f1().GB_Status_AppendText_Nextline("--init_algorithm_storage--", Color.Blue);
                init_algorithm_storag.Show_RGB_Datas(Color.Blue);
            }

            if (dp213_form().checkBox_OC_Mode23_UVL_Check.Checked)
            {
                f1().SB_Show();
                f1().SB_Clear();
            }

            if(dp213_form().radioButton_Difference_Offset.Checked)
            {
                f1().GB_Status_AppendText_Nextline(SJHLogger.Difference_Offset_Log12.ToString(), Color.Red);
                f1().GB_Status_AppendText_Nextline(SJHLogger.Difference_Offset_Log23.ToString(), Color.Blue);
            }
        }

        private void Show_CMOTP_GMOTP_CRC_Check_Result(Gamma_Set OC_Mode1_GammaSet)
        {
            crcs.Show_Changed_Flags_Status();
            crcs.Show_CMOTP_GammaOTP_Result(OC_Mode1_GammaSet);
        }

        private void OC_Initialize()
        {
            f1().SB_Clear();
            f1().OC_Timer_Start();

            cmds.Vreg1_Text_Clear();
            cmds.ELVSS_Vinit2_Text_Clear();
            cmds.VREF4095_VREF0_Clear();

            vars.Optic_Compensation_Stop = false;
            vars.Optic_Compensation_Succeed = false;

            if (dp213_form().checkBox_OC_File_Refresh.Checked)
                dp213_mornitoring().OC_File_Refresh();

            crcs.Initialize_Vriables();

            Read_and_Update_All_REF0_REF4095_Vreg1_Data_and_Voltages();//It must be needed
            dp213_form().Read_DBV();

            dp213_form().groupBox_OC_Main_Setting.Enabled = false;
            dp213_form().groupBox_DP213_Mode_Set_Selection.Enabled = false;
            dp213_mornitoring().Dual_RadioButton_All_Enable(false);

            string OC_Mode1_LUT_Path = Directory.GetCurrentDirectory() + "\\DP213" + "\\Initial_RGB_Gray_LUT" + "\\OC_Mode1_Gray_LUT.csv";
            string OC_Mode4_LUT_Path = Directory.GetCurrentDirectory() + "\\DP213" + "\\Initial_RGB_Gray_LUT" + "\\OC_Mode4_Gray_LUT.csv";
            string OC_Mode5_LUT_Path = Directory.GetCurrentDirectory() + "\\DP213" + "\\Initial_RGB_Gray_LUT" + "\\OC_Mode5_Gray_LUT.csv";
            string OC_Mode6_LUT_Path = Directory.GetCurrentDirectory() + "\\DP213" + "\\Initial_RGB_Gray_LUT" + "\\OC_Mode6_Gray_LUT.csv";
            init_gray_lut = new Init_Gray_LUT(OC_Mode1_LUT_Path, OC_Mode4_LUT_Path, OC_Mode5_LUT_Path, OC_Mode6_LUT_Path);
            interpolation_fomula_obj = new InterpolationFomulaFactory();
            
            SJHLogger.Clear();
        }

        private void OC_Finalize(DP213_OC_Modes DP213_Objects)
        {
            int band = 1;
            DP213_Objects.Get_DP213_object(OC_Mode.Mode1).Show_Gradation_Pattern_At_Selected_Band(band);
            f1().OC_Timer_Stop();

            dp213_form().groupBox_OC_Main_Setting.Enabled = true;
            dp213_form().groupBox_DP213_Mode_Set_Selection.Enabled = true;
            dp213_mornitoring().Dual_RadioButton_All_Enable(true);

            if (IsAll_Mode1234_and_Band0_to_Band11_Compensated())
            {
                f1().GB_Status_AppendText_Nextline("Band0 ~ Band11 have been compensated", Color.Blue);
                f1().GB_Status_AppendText_Nextline("Save this new Gray LUT", Color.Blue);
                init_gray_lut.Save_Updated_Matched_Grays();
            }

            SoundPlayer simpleSound = new SoundPlayer(Directory.GetCurrentDirectory() + "\\wavs\\OK_Sound.wav");
            simpleSound.PlayLooping();
            if (System.Windows.Forms.MessageBox.Show("OC Finishied") == DialogResult.OK)
            {
                simpleSound.Stop();
            }
        }

        private bool IsAll_Mode1234_and_Band0_to_Band11_Compensated()
        {
            return (vars.Optic_Compensation_Stop == false && dp213_form().checkBox_Band0.Checked && dp213_form().checkBox_Band1.Checked
                && dp213_form().checkBox_Band2.Checked && dp213_form().checkBox_Band3.Checked
                && dp213_form().checkBox_Band4.Checked && dp213_form().checkBox_Band5.Checked
                && dp213_form().checkBox_Band6.Checked && dp213_form().checkBox_Band7.Checked
                && dp213_form().checkBox_Band8.Checked && dp213_form().checkBox_Band9.Checked
                && dp213_form().checkBox_Band10.Checked && dp213_form().checkBox_Band11.Checked
                && dp213_form().checkBox_Mode_4_Skip.Checked == false
                && dp213_form().checkBox_Mode_5_Skip.Checked == false
                && dp213_form().checkBox_Mode_6_Skip.Checked == false);
        }

        private void Read_and_Update_All_REF0_REF4095_Vreg1_Data_and_Voltages()
        {
            cmds.Read_and_Update_REF0_REF4095_and_Textboxes(); //(1.1) Read REF0/REF4095 For Vreg1
            cmds.Read_and_Update_AOD_REF0_REF4095();//(1.2) Read AOD REF0/REF4095
            cmds.Read_Dec_Vreg1_and_Save_to_Textbox();//(2) update Vreg1 for HBM/Normal/AOD
            
            if(dp213_form().radioButton_AM1_Original_Value.Checked || dp213_form().radioButton_AM0_Original_Value.Checked)
            {
                Set_AM1_or_AM0_As_Read_Value(Gamma_Set.Set1);
                Set_AM1_or_AM0_As_Read_Value(Gamma_Set.Set2);
                Set_AM1_or_AM0_As_Read_Value(Gamma_Set.Set3);
                Set_AM1_or_AM0_As_Read_Value(Gamma_Set.Set4);
                Set_AM1_or_AM0_As_Read_Value(Gamma_Set.Set5);
                Set_AM1_or_AM0_As_Read_Value(Gamma_Set.Set6);
            }
            else
            {
                storage.Set_All_Band_Set_AM1_As_Same_Values(new RGB(0));// Set All AM1 = 0x00
                storage.Set_All_Band_Set_AM0_As_Same_Values(new RGB(0));// Set All AM0 = 0x00
            }
        }

    private void Set_AM1_or_AM0_As_Read_Value(Gamma_Set Set)
    {
        for (int band = 0; band < DP213_Static.Max_Band_Amount; band++)
        {
            byte[] output = ModelFactory.Get_DP213_Instance().Get_Read_Gamma_AM1_AM0_CMD(Set, band);
            f1().MX_OTP_Read(output);
            byte[] parameters = f1().Get_Read_Byte_Array(output[2]);

            if (dp213_form().radioButton_AM1_Original_Value.Checked)
            {
                RGB AM1 = ModelFactory.Get_DP213_Instance().Get_AM1(parameters);
                storage.Set_Band_Set_AM1(Set, band, AM1);
            }

            if (dp213_form().radioButton_AM0_Original_Value.Checked)
            {
                RGB AM0 = ModelFactory.Get_DP213_Instance().Get_AM0(parameters);
                storage.Set_Band_Set_AM0(Set, band, AM0);
            }
        }
    }

        
        private DP213_Optic_Compensation Get_Selected_OCMode_For_AM0_AM1_REF0_ELVSS(DP213_OC_Modes DP213_Objects)
        {
            DP213_Optic_Compensation Selected_OCMode_For_AM0_AM1_REF0_ELVSS;

            if (dp213_form().radioButton_AM0_AM1_REF0_ELVSS_OCMode_1.Checked)
                Selected_OCMode_For_AM0_AM1_REF0_ELVSS = DP213_Objects.Get_DP213_object(OC_Mode.Mode1);
            else if (dp213_form().radioButton_AM0_AM1_REF0_ELVSS_OCMode_2.Checked)
                Selected_OCMode_For_AM0_AM1_REF0_ELVSS = DP213_Objects.Get_DP213_object(OC_Mode.Mode2);
            else
                Selected_OCMode_For_AM0_AM1_REF0_ELVSS = DP213_Objects.Get_DP213_object(OC_Mode.Mode3);

            return Selected_OCMode_For_AM0_AM1_REF0_ELVSS;
        }


        private void Optic_Compensation(DP213_OC_Modes DP213_Objects)
        {
            vars.Is_HBM_Gray255_Compensated = false;

            DP213_Optic_Compensation Selected_OCMode_For_AM0_AM1_REF0_ELVSS = Get_Selected_OCMode_For_AM0_AM1_REF0_ELVSS(DP213_Objects);
            if (Is_Conduct_ELVSS_and_Vinit2_OC()) Selected_OCMode_For_AM0_AM1_REF0_ELVSS.ELVSS_and_Vinit2_Compensation();
            if (Is_Conduct_VREF0_OC()) Selected_OCMode_For_AM0_AM1_REF0_ELVSS.VREF0_Compensation();

            if (Is_Conduct_Black_OC()) Selected_OCMode_For_AM0_AM1_REF0_ELVSS.Black_Compensation();
            if (Is_Conduct_AM1_OC()) Selected_OCMode_For_AM0_AM1_REF0_ELVSS.AM1_Compensation();

            DP213_Optic_Compensation OC_Set_Mode1 = DP213_Objects.Get_DP213_object(OC_Mode.Mode1);
            if (Is_AOD_Band_Selected()) OC_Set_Mode1.AOD_Compensation();
            if (Is_Any_Normal_Band_Selected()) Main_Compensation(DP213_Objects);
        }

        private void Main_Compensation(DP213_OC_Modes DP213_Objects)
        {
            dp213_mornitoring().Dual_Mode_GridView_Measure_Loopcount_ExtentionApplied_Clear_Without_AOD_Area();
            
            DP213_Main_Compensation Main_OC = new DP213_Main_Compensation(DP213_Objects);
            Main_OC.Mode123_Compensation();
            
            if(dp213_form().checkBox_Mode_4_Skip.Checked == false) Main_OC.Mode4_Compensation();
            if (dp213_form().checkBox_Mode_5_Skip.Checked == false) Main_OC.Mode5_Compensation();
            if (dp213_form().checkBox_Mode_6_Skip.Checked == false) Main_OC.Mode6_Compensation();
        }

        private bool Is_Conduct_ELVSS_and_Vinit2_OC()
        {
            return (dp213_form().checkBox_ELVSS_and_Vinit2_Comp.Checked && vars.Optic_Compensation_Stop == false);
        }

        private bool Is_Conduct_VREF0_OC()
        {
            return (dp213_form().checkBox_VREF0_Comp.Checked && vars.Optic_Compensation_Stop == false);
        }

        private bool Is_Conduct_Black_OC()
        {
            return (dp213_form().radioButton_Black_Compensation.Checked && vars.Optic_Compensation_Stop == false);
        }


        private bool Is_Conduct_AM1_OC()
        {
            return (dp213_form().radioButton_AM1_Comp.Checked && vars.Optic_Compensation_Stop == false);
        }

        private bool Is_AOD_Band_Selected()
        {
            return (dp213_form().checkBox_AOD0.Checked || dp213_form().checkBox_AOD1.Checked || dp213_form().checkBox_AOD2.Checked);
        }


        private bool Is_Any_Normal_Band_Selected()
        {
            return (dp213_form().checkBox_Band0.Checked || dp213_form().checkBox_Band1.Checked || dp213_form().checkBox_Band2.Checked || dp213_form().checkBox_Band3.Checked || dp213_form().checkBox_Band4.Checked || dp213_form().checkBox_Band5.Checked
              || dp213_form().checkBox_Band6.Checked || dp213_form().checkBox_Band7.Checked || dp213_form().checkBox_Band8.Checked || dp213_form().checkBox_Band9.Checked || dp213_form().checkBox_Band10.Checked || dp213_form().checkBox_Band11.Checked);
        }

    }
}
