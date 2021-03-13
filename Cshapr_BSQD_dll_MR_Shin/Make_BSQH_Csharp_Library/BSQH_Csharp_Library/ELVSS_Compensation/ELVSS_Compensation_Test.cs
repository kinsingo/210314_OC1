using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace BSQH_Csharp_Library
{

    [TestFixture]
    class ELVSS_Compensation_Test
    {
        [Test]
        public void Get_Average_XYLv_After_Remove_Min_Max_Exception_Test()
        {
            ELVSS_Compensation elvss_compensation_obj = new ELVSS_Compensation();

            XYLv[] myinput = new XYLv[2];
           var ex = Assert.Throws<Exception>(() => elvss_compensation_obj.Get_Average_XYLv_After_Remove_Min_Max(myinput));
           Assert.That(ex.Message, Is.EqualTo("Input Array Length Should be equal to or bigger than 3"));
        }

        [Test]
        public void Get_Average_XYLv_After_Remove_Min_Max_Case1()
        {
            ELVSS_Compensation elvss_compensation_obj = new ELVSS_Compensation();

            XYLv[] myinput = new XYLv[5];
            myinput[0] = new XYLv(0.3038, 0.3172, 893.8708);
            myinput[1] = new XYLv(0.3033, 0.3169, 898.9791);
            myinput[2] = new XYLv(0.3041, 0.3171, 892.0173);
            myinput[3] = new XYLv(0.3036, 0.3168, 896.2209);
            myinput[4] = new XYLv(0.3039, 0.3165, 897.1231);

            XYLv output  = elvss_compensation_obj.Get_Average_XYLv_After_Remove_Min_Max(myinput);

            Assert.AreEqual(Math.Round(output.double_X,5), 0.30377);
            Assert.AreEqual(Math.Round(output.double_Y,5), 0.31693);
            Assert.AreEqual(Math.Round(output.double_Lv,4), 895.7383);
        }

        [Test]
        public void Get_Average_XYLv_After_Remove_Min_Max_Case2()
        {
            ELVSS_Compensation elvss_compensation_obj = new ELVSS_Compensation();

            XYLv[] myinput = new XYLv[3];
            myinput[0] = new XYLv(0.3033, 0.3169, 898.9791);
            myinput[1] = new XYLv(0.3041, 0.3171, 892.0173);
            myinput[2] = new XYLv(0.3036, 0.3168, 896.2209);


            XYLv output = elvss_compensation_obj.Get_Average_XYLv_After_Remove_Min_Max(myinput);
            Assert.AreEqual(Math.Round(output.double_X,4), 0.3036);
            Assert.AreEqual(Math.Round(output.double_Y,4), 0.3169);
            Assert.AreEqual(Math.Round(output.double_Lv,4), 896.2209);
        }


        //-----------
        [TestCase(new double[5] {933.8466,931.2597,929.9858,926.9485,926.387},
            new double[6] {-2.9,-2.8,-2.7,-2.6,-2.5,-2.4},
            new double[6] {874.3778,869.9155,864.2216,857.2672,849.4106,840.4617})]
        public void Find_ELVSS_PassTest1(double[] First_Five_Lv, double[] ELVSS, double[] Lv)
        {
            ELVSS_Compensation elvss_compensation_obj = new ELVSS_Compensation();
            double output_elvss = elvss_compensation_obj.FindELVSS(First_Five_Lv, ELVSS, Lv);

            Assert.AreEqual(-2.8, output_elvss);
            Assert.AreEqual(true, elvss_compensation_obj.Is_ELVSS_Found());
            
        }

        [TestCase(new double[5] { 933.8466, 931.2597, 929.9858, 926.9485, 926.387 },
       new double[6] { -2.9, -2.8, -2.7, -2.6, -2.5, -2.4 },
       new double[6] { 874.3778, 869.9155, 864.2216, 863.2216, 849.4106, 840.4617 })]
        public void Find_ELVSS_PassTest2(double[] First_Five_Lv, double[] ELVSS, double[] Lv)
        {
            ELVSS_Compensation elvss_compensation_obj = new ELVSS_Compensation();
            double output_elvss = elvss_compensation_obj.FindELVSS(First_Five_Lv, ELVSS, Lv);
         
            Assert.AreEqual(-2.8, output_elvss);
            Assert.AreEqual(false, elvss_compensation_obj.Is_ELVSS_Found());
        }

        [TestCase(new double[4] { 931.2597, 929.9858, 926.9485, 926.387 },
      new double[6] { -2.9, -2.8, -2.7, -2.6, -2.5, -2.4 },
      new double[6] { 874.3778, 869.9155, 864.2216, 863.2216, 849.4106, 840.4617 })]
        [TestCase(new double[6] { 933.8466, 933.8466, 931.2597, 929.9858, 926.9485, 926.387 },
      new double[6] { -2.9, -2.8, -2.7, -2.6, -2.5, -2.4 },
      new double[6] { 874.3778, 869.9155, 864.2216, 863.2216, 849.4106, 840.4617 })]
        public void Find_ELVSS_FailingTest_For_First_Five_Lv_Length(double[] First_Five_Lv, double[] ELVSS, double[] Lv)
        {
            ELVSS_Compensation elvss_compensation_obj = new ELVSS_Compensation();

            var ex = Assert.Throws<Exception>(() => elvss_compensation_obj.FindELVSS(First_Five_Lv, ELVSS, Lv));
            Assert.AreEqual(ex.Message.Trim(), ("Input_Array(Frist_Five_Lv) Length should be equal to 5").Trim());
        }

        [TestCase(new double[5] { 933.8466, 931.2597, 929.9858, 926.9485, 926.387 },
new double[5] { -2.8, -2.7, -2.6, -2.5, -2.4 },
new double[6] { 874.3778, 869.9155, 864.2216, 863.2216, 849.4106, 840.4617 })]

        [TestCase(new double[5] { 933.8466, 931.2597, 929.9858, 926.9485, 926.387 },
       new double[6] { -2.9, -2.8, -2.7, -2.6, -2.5, -2.4 },
       new double[5] { 869.9155, 864.2216, 863.2216, 849.4106, 840.4617 })]

        [TestCase(new double[5] { 933.8466, 931.2597, 929.9858, 926.9485, 926.387 },
       new double[6] { -2.9, -2.8, -2.7, -2.6, -2.5, -2.4 },
       new double[7] { 874.3778, 874.3778, 869.9155, 864.2216, 863.2216, 849.4106, 840.4617 })]

        [TestCase(new double[5] { 933.8466, 931.2597, 929.9858, 926.9485, 926.387 },
       new double[7] { -2.9, - 2.9, -2.8, -2.7, -2.6, -2.5, -2.4 },
       new double[6] { 874.3778, 869.9155, 864.2216, 863.2216, 849.4106, 840.4617 })]
        public void Find_ELVSS_FailingTest_For_ELVSS_and_Lv_Length(double[] First_Five_Lv, double[] ELVSS, double[] Lv)
        {
            ELVSS_Compensation elvss_compensation_obj = new ELVSS_Compensation();
            
            var ex = Assert.Throws<Exception>(() => elvss_compensation_obj.FindELVSS(First_Five_Lv, ELVSS, Lv));
            Assert.AreEqual(ex.Message.Trim(), ("Input_Array(ELVSS & Lv) Length should be equal to " + elvss_compensation_obj.Get_ELVSSArrayLength()).Trim());
        }


        [TestCase(-3.0, -6.1)]
        [TestCase(-3.0, -6.0)]
        [TestCase(-3.0, -5.9)]
        [TestCase(-3.0, -5.8)]
        [TestCase(-3.0, -5.7)]
        [TestCase(-3.0, -5.6)]
        [TestCase(-3.0, -5.5)]
        public void Is_ELVSS_Min_Max_Volatge_Valid_Test_1(double ELVSS_Voltage_Max, double ELVSS_Voltage_Min)
        {
            ELVSS_Compensation elvss_compensation_obj = new ELVSS_Compensation();

            var ex = Assert.Throws<Exception>(() => elvss_compensation_obj.Is_ELVSS_Min_Max_Volatge_Valid(ELVSS_Voltage_Max , ELVSS_Voltage_Min));

            double minimum = elvss_compensation_obj.Get_First_ELVSS_Voltage() + elvss_compensation_obj.ELVSS_Minimum_Step() * elvss_compensation_obj.Get_ELVSSArrayLength();
            string ExpectedErrorMessage = "ELVSS_Voltage_Min(" + ELVSS_Voltage_Min + ") should be bigger than " + minimum;

            Assert.AreEqual(ex.Message.Trim(), ExpectedErrorMessage.Trim());
        }

        [TestCase(-0.7, -3.0)]
        [TestCase(-0.8, -3.0)]
        [TestCase(-0.9, -3.0)]
        [TestCase(-1.0, -3.0)]
        [TestCase(-1.1, -3.0)]
        [TestCase(-1.2, -3.0)]
        [TestCase(-1.3, -3.0)]
        [TestCase(-1.4, -3.0)]
        public void Is_ELVSS_Min_Max_Volatge_Valid_Test_2(double ELVSS_Voltage_Max, double ELVSS_Voltage_Min)
        {
            ELVSS_Compensation elvss_compensation_obj = new ELVSS_Compensation();

            var ex = Assert.Throws<Exception>(() => elvss_compensation_obj.Is_ELVSS_Min_Max_Volatge_Valid(ELVSS_Voltage_Max, ELVSS_Voltage_Min));

            double maximum = elvss_compensation_obj.Get_Last_ELVSS_Voltage() - elvss_compensation_obj.ELVSS_Minimum_Step() * elvss_compensation_obj.Get_ELVSSArrayLength();
            string ExpectedErrorMessage = "ELVSS_Voltage_Max(" + ELVSS_Voltage_Max + ") should be smaller than " + maximum;

            Assert.AreEqual(ex.Message.Trim(), ExpectedErrorMessage.Trim());
        }


        [TestCase(-2.95, -3.0)]
        [TestCase(-3.0, -3.0)]
        [TestCase(-3.1, -3.0)]
        [TestCase(-3.2, -3.0)]

        public void Is_ELVSS_Min_Max_Volatge_Valid_Test_3(double ELVSS_Voltage_Max, double ELVSS_Voltage_Min)
        {
            ELVSS_Compensation elvss_compensation_obj = new ELVSS_Compensation();

            var ex = Assert.Throws<Exception>(() => elvss_compensation_obj.Is_ELVSS_Min_Max_Volatge_Valid(ELVSS_Voltage_Max, ELVSS_Voltage_Min));

            string ExpectedErrorMessage = "The Gap Between ELVSS_Voltage_Max and ELVSS_Voltage_Min should be bigger than " + elvss_compensation_obj.ELVSS_Minimum_Step();

            Assert.AreEqual(ex.Message.Trim(), ExpectedErrorMessage.Trim());
        }
    }
}
