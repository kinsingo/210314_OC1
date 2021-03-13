using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices; // (Dll 사용위해 필요)
using BSQH_Csharp_Library;

namespace PNC_Csharp
{
    public partial class CPP_Dll_Test_Form : Form
    {

        [DllImport("BSQH_dll_MR_Shin.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool Sub_Compensation(ref int R_Gamma, ref int G_Gamma, ref int B_Gamma, double Diff_X, double Diff_Y, double Diff_Lv
, double Limit_X, double Limit_Y, double Limit_Lv, int Gamma_Register_Limit, ref bool Gamma_Out_Of_Register_Limit, ref bool Within_Spec_Limit);

        [DllImport("BSQH_dll_MR_Shin.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void Vreg1_Compensation(ref int R_Gamma, ref int Vreg1, ref int B_Gamma, double Diff_X, double Diff_Y, double Diff_Lv
, double Limit_X, double Limit_Y, double Limit_Lv, ref bool Vreg1_Comp_Finish);

        [DllImport("BSQH_dll_MR_Shin.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void Black_Compensation(ref int Black_Register, double Target_Lv, double Measured_Lv, ref bool Black_Comp_Finish);


        private static CPP_Dll_Test_Form Instance;
        public static CPP_Dll_Test_Form getInstance()
        {
            if (Instance == null)
                Instance = new CPP_Dll_Test_Form();

            return Instance;
        }

        public static bool IsIstanceNull()
        {
            if (Instance == null)
                return true;
            else
                return false;
        }

        public static void DeleteInstance()
        {
            Instance = null;
        }
        private CPP_Dll_Test_Form()
        {
            InitializeComponent();
        }

        private void CPP_Dll_Test_Form_Load(object sender, EventArgs e)
        {
            Param_Update_From_Textbox();

            //광보 관련 Parameter
            Gamma_Register_Limit = 511;

        }


        private void button_Hide_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }

        //Gamma
        RGB Gamma = new RGB();
        RGB Prev_Gamma = new RGB();
        RGB Diff_Gamma = new RGB();

        //Taget / Measure / Diff / Limit
        XYLv Target = new XYLv();
        XYLv Measure = new XYLv();
        XYLv Diff = new XYLv();
        XYLv Limit = new XYLv();

        //광보 관련 Parameter
        int Gamma_Register_Limit;
        bool Gamma_Out_Of_Register_Limit;
        bool Optic_Compensation_Succeed;
        bool Within_Spec_Limit;


        private void button_Sub_Comp_One_Time_Click(object sender, EventArgs e)
        {
            //BSQA Param Update From Textbox (Prev_Gamma = Gamma)
            Param_Update_From_Textbox();
        
            //----Dll (gamma Change) Test----
            Sub_Compensation(ref Gamma.int_R, ref Gamma.int_G, ref Gamma.int_B, Diff.double_X, Diff.double_Y, Diff.double_Lv, Limit.double_X, Limit.double_Y, Limit.double_Lv,
                Gamma_Register_Limit, ref Gamma_Out_Of_Register_Limit, ref Within_Spec_Limit);

            //Gamma - Prev_Gamma (operator -)
            Diff_Gamma = Gamma - Prev_Gamma;

            //Measurement Changing Test (By Changed(Diff) Gamma Value)
            Test_Gamma_RGB_Measurement_Changed(Diff_Gamma, ref Measure);
            Gamma_Textbox_update();
        }

         private void button_Sub_Comp_Many_Times_Click(object sender, EventArgs e)
        {
            int loop_count = 0; //= Convert.ToInt16(textBox_loop_count.Text);
            int loop_count_max = Convert.ToInt16(textBox_Max_Loop.Text);

            Optic_Compensation_Succeed = false;
            while (Optic_Compensation_Succeed == false)
            {
                textBox_loop_count.Text = (++loop_count).ToString();
                Application.DoEvents();
                
                if (loop_count == loop_count_max)
                {
                    Optic_Compensation_Succeed = false;
                    textBox_Bool_Finish_Status.Text = Optic_Compensation_Succeed.ToString();
                    break;
                }

                button_Sub_Comp_One_Time_Click(sender, e);

                if (Gamma_Out_Of_Register_Limit == true)
                {
                    if (Within_Spec_Limit)
                    {
                        Optic_Compensation_Succeed = true;
                        textBox_Bool_Finish_Status.Text = Optic_Compensation_Succeed.ToString();
                        break;
                    }
                    Optic_Compensation_Succeed = false;
                    textBox_Bool_Finish_Status.Text = Optic_Compensation_Succeed.ToString();
                    System.Windows.Forms.MessageBox.Show("Gamma R/G/B is out of Limit");
                    break;
                }   
            }
        }



        private void button_Vreg1_Comp_One_Time_Click(object sender, EventArgs e)
        {
        }

        private void button_Black_Compensation_Click(object sender, EventArgs e)
        {
        }

    
        //Diff_Gamma = Next_Gamma - Prev_Gamma
        private void Test_Gamma_RGB_Measurement_Changed(RGB Diff_Gamma, ref XYLv Measure)
        {
            // R 처리
            if (Diff_Gamma.int_R >= 1)
            {
                Measure.double_X = Measure.double_X + 0.002 * Diff_Gamma.int_R;
                Measure.double_Y = Measure.double_Y + 0.001 * Diff_Gamma.int_R;
                Measure.double_Lv = Measure.double_Lv + 1 * Diff_Gamma.int_R;
            }
            else if (Diff_Gamma.int_R <= -1)
            {
                Measure.double_X = Measure.double_X - 0.002 * Math.Abs(Diff_Gamma.int_R);
                Measure.double_Y = Measure.double_Y - 0.001 * Math.Abs(Diff_Gamma.int_R);
                Measure.double_Lv = Measure.double_Lv - 1 * Math.Abs(Diff_Gamma.int_R);
            }

            // G 처리
            if (Diff_Gamma.int_G >= 1)
            {
                Measure.double_X = Measure.double_X + 0.001 * Diff_Gamma.int_G;
                Measure.double_Y = Measure.double_Y + 0.002 * Diff_Gamma.int_G;
                Measure.double_Lv = Measure.double_Lv + 1.5 * Diff_Gamma.int_G;
            }
            else if (Diff_Gamma.int_G <= -1)
            {
                Measure.double_X = Measure.double_X - 0.001 * Math.Abs(Diff_Gamma.int_G);
                Measure.double_Y = Measure.double_Y - 0.002 * Math.Abs(Diff_Gamma.int_G);
                Measure.double_Lv = Measure.double_Lv - 1.5 * Math.Abs(Diff_Gamma.int_G);
            }

            // B 처리
            if (Diff_Gamma.int_B >= 1)
            {
                Measure.double_X = Measure.double_X - 0.001 * Diff_Gamma.int_B;
                Measure.double_Y = Measure.double_Y - 0.001 * Diff_Gamma.int_B;
                Measure.double_Lv = Measure.double_Lv + 0.5 * Diff_Gamma.int_B;
            }
            else if (Diff_Gamma.int_B <= -1)
            {
                Measure.double_X = Measure.double_X + 0.001 * Math.Abs(Diff_Gamma.int_B);
                Measure.double_Y = Measure.double_Y + 0.001 * Math.Abs(Diff_Gamma.int_B);
                Measure.double_Lv = Measure.double_Lv - 0.5 * Math.Abs(Diff_Gamma.int_B);
            }

            Measure_Textbox_update();

            //Diff Value/Textbox Update
            Diff = Measure - Target;
            DIff_Textbox_update();
        }

        private void DIff_Textbox_update()
        {
            Diff.String_Update_From_Double();
            textBox_Diff_X.Text = Diff.X;
            textBox_Diff_Y.Text = Diff.Y;
            textBox_Diff_Lv.Text = Diff.Lv;
        }

        private void Measure_Textbox_update()
        {
            Measure.String_Update_From_Double();
            textBox_Measured_X.Text = Measure.X;
            textBox_Measured_Y.Text = Measure.Y;
            textBox_Measured_Lv.Text = Measure.Lv;
        }

        private void Target_Textbox_update()
        {
            Target.String_Update_From_Double();
            textBox_target_X.Text = Target.X;
            textBox_target_Y.Text = Target.Y;
            textBox_target_Lv.Text = Target.Lv;
        }

        private void Limit_Textbox_update()
        {
            Limit.String_Update_From_Double();
            textBox_Limit_X.Text = Limit.X;
            textBox_Limit_Y.Text = Limit.Y;
            textBox_Limit_Lv.Text = Limit.Lv;
        }
        private void Gamma_Textbox_update()
        {
            Gamma.String_Update_From_int();
            textBox_Gamma_R.Text = Gamma.R;
            textBox_Gamma_G.Text = Gamma.G;
            textBox_Gamma_B.Text = Gamma.B;
        }


   

        private void Param_Update_From_Textbox()
        {
            //Target
            Target.double_X = Convert.ToDouble(textBox_target_X.Text);
            Target.double_Y = Convert.ToDouble(textBox_target_Y.Text);
            Target.double_Lv = Convert.ToDouble(textBox_target_Lv.Text);
            Target.String_Update_From_Double();

            //Measure
            Measure.double_X = Convert.ToDouble(textBox_Measured_X.Text);
            Measure.double_Y = Convert.ToDouble(textBox_Measured_Y.Text);
            Measure.double_Lv = Convert.ToDouble(textBox_Measured_Lv.Text);
            Measure.String_Update_From_Double();

            //Difference(Diff = Measure(F) - Target(T))
            Diff = Measure - Target;

            //Limit
            Limit.double_X = Convert.ToDouble(this.textBox_Limit_X.Text);
            Limit.double_Y = Convert.ToDouble(this.textBox_Limit_Y.Text);
            Limit.double_Lv = Convert.ToDouble(this.textBox_Limit_Lv.Text);
            Limit.String_Update_From_Double();

            //Gamma (Int Value)
            Gamma.int_R = Convert.ToInt16(textBox_Gamma_R.Text);
            Gamma.int_G = Convert.ToInt16(textBox_Gamma_G.Text);
            Gamma.int_B = Convert.ToInt16(textBox_Gamma_B.Text);
            Prev_Gamma.int_R = Gamma.int_R;
            Prev_Gamma.int_G = Gamma.int_G;
            Prev_Gamma.int_B = Gamma.int_B;
        }


    }
}
