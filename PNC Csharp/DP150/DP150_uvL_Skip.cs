using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using BSQH_Csharp_Library;

namespace PNC_Csharp
{
    public class DP150_uvL_Skip
    { 
        Form1 f1()
        {
            return (Form1)System.Windows.Forms.Application.OpenForms["Form1"];
        }

        DP150_Dual_Engineering_Mornitoring_Mode dp150_dual_mornitoring()
        {
            return (DP150_Dual_Engineering_Mornitoring_Mode)System.Windows.Forms.Application.OpenForms["DP150_Dual_Engineering_Mornitoring_Mode"];
        }

        Second_Model_Option_Form dp150_form()
        {
            return (Second_Model_Option_Form)System.Windows.Forms.Application.OpenForms["Second_Model_Option_Form"];
        }

        private int band;
        private int gray;

        public DP150_uvL_Skip(int band, int gray)
        {
            this.band = band;
            this.gray = gray;
        }

        public bool Is_Mode2_OC_Skip_Within_UVL()
        {
            XYLv Mode1_Measured = dp150_dual_mornitoring().Get_OCMode1_Measure(gray);
            XYLv Mode2_Measured = dp150_dual_mornitoring().Get_OCMode2_Measure(gray);
            f1().GB_Status_AppendText_Nextline("OC_Mode1_Measured B" + band.ToString() + "/" + gray.ToString() + " X/Y/LV : " + Mode1_Measured.double_X + "/" + Mode1_Measured.double_Y + "/" + Mode1_Measured.double_Lv, Color.Blue);
            f1().GB_Status_AppendText_Nextline("OC_Mode2_Measured B" + band.ToString() + "/" + gray.ToString() + " X/Y/LV : " + Mode2_Measured.double_X + "/" + Mode2_Measured.double_Y + "/" + Mode2_Measured.double_Lv, Color.Blue);

            return Is_Two_Measured_Are_Within_UVL_Spec(Mode1_Measured, Mode2_Measured);
        }

        private bool Is_Two_Measured_Are_Within_UVL_Spec(XYLv Mode1_Measured, XYLv Mode2_or_Mode3_Measured)
        {
            double Diff_Delta_L_Spec = dp150_form().OC_Mode2_Diff_Delta_L_Spec[band, gray];
            double UV_Distance_Limit = dp150_form().OC_Mode2_Diff_Delta_UV_Spec[band, gray];

            bool Delta_L_Spec_In = Compare_Delta_L(Mode1_Measured, Mode2_or_Mode3_Measured, Diff_Delta_L_Spec);
            bool Delta_UV_Spec_In = Compare_Delta_UV(Mode1_Measured, Mode2_or_Mode3_Measured, UV_Distance_Limit);

            if (Delta_L_Spec_In && Delta_UV_Spec_In)
                return true;
            else
                return false;
        }

        private bool Compare_Delta_UV(XYLv Mode1_Measured, XYLv Mode2_or_Mode3_Measured, double UV_Distance_Limit)
        {
            double UV_Distance = Color_Coordinate.Get_UV_Distance_From_XY(Mode1_Measured.double_X, Mode1_Measured.double_Y, Mode2_or_Mode3_Measured.double_X, Mode2_or_Mode3_Measured.double_Y);
            bool UV_Spec_In;

            if (UV_Distance < UV_Distance_Limit)
            {
                UV_Spec_In = true;
                f1().GB_Status_AppendText_Nextline("UV_Distance(" + UV_Distance.ToString() + ") < UV_Distance_Limit(" + UV_Distance_Limit.ToString() + "), UV_Spec_In = true)", Color.Green);
            }
            else
            {
                UV_Spec_In = false;
                f1().GB_Status_AppendText_Nextline("UV_Distance(" + UV_Distance.ToString() + ") >= UV_Distance_Limit(" + UV_Distance_Limit.ToString() + "), UV_Spec_In = false)", Color.Red);
            }

            return UV_Spec_In;
        }

        private bool Compare_Delta_L(XYLv Mode1_Measured, XYLv Mode2_or_Mode3_Measured, double Diff_Delta_L_Spec)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            double Diff_Measured_Lv = Mode1_Measured.double_Lv - Mode2_or_Mode3_Measured.double_Lv;
            double Delta_L = Math.Abs(Diff_Measured_Lv / (Mode1_Measured.double_Lv));

            bool Delta_L_Spec_In;

            if (Delta_L < Diff_Delta_L_Spec)
            {
                Delta_L_Spec_In = true;
                f1.GB_Status_AppendText_Nextline("Delta_L(" + Delta_L.ToString() + ") < Diff_Delta_L_Spec(" + Diff_Delta_L_Spec.ToString() + "), Delta_L_Spec_In = true)", Color.Green);
            }
            else
            {
                Delta_L_Spec_In = false;
                f1.GB_Status_AppendText_Nextline("Delta_L(" + Delta_L.ToString() + ") >= Diff_Delta_L_Spec(" + Diff_Delta_L_Spec.ToString() + "), Delta_L_Spec_In = false)", Color.Red);
            }

            return Delta_L_Spec_In;
        }
        
    }


}
