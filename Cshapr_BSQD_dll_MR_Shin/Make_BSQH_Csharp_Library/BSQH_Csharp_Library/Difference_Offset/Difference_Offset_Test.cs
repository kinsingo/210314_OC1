using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace BSQH_Csharp_Library
{
    [TestFixture]
    class Difference_Offset_Test
    {
        [TestCase(100, 102, 100.5, 101.5, 200, 201, 0.02, false)] //Case1 : True / Case2 : False / Case3 : False
        [TestCase(100, 99, 100.5, 102.6, 200, 199, 0.02, false)]//Case1 : False / Case2 : True / Case3 : False
        [TestCase(100, 101, 100.5, 102.6, 200, 199, 0.02, false)]//Case1 : False / Case2 : False / Case3 : True
        [TestCase(100, 102, 100.5, 102.6, 200, 201, 0.02, true)] //Case1 : False / Case2 : False / Case3 : False (O)
        public void IsNotOffsetSkip(int Prev_Register_PrevMode, int Cur_Register_PrevMode, double Prev_Lv_CurMode, double Cur_Lv_CurMode, int Prev_Register_CurMode, int Cur_Register_CurMode, double Delta_L_Limit,bool expected)
        {
            bool result = Difference_Offset.IsNotOffsetSkip(Prev_Register_PrevMode, Cur_Register_PrevMode, Prev_Lv_CurMode, Cur_Lv_CurMode, Prev_Register_CurMode, Cur_Register_CurMode, Delta_L_Limit);
            Assert.That(result == expected);
        }

        [TestCase(100.5, 101.5, 0.02, true)]//Within Delta_L_Limit(O)
        [TestCase(101.5, 100.5, 0.02, true)]//Within Delta_L_Limit(O)
        [TestCase(100.5, 102.6, 0.02, false)]//Out of Delta_L_Limit
        [TestCase(102.6, 100.5, 0.02, false)]//Out of Delta_L_Limit
        [TestCase(100.5, 102.6, 0.03, true)]//Within Delta_L_Limit(O)
        [TestCase(102.6, 100.5, 0.03, true)]//Within Delta_L_Limit(O)
        public void OffsetSkip_Case1(double Prev_Lv_CurMode, double Cur_Lv_CurMode, double Delta_L_Limit,bool expected)
        {
            bool result = Difference_Offset.OffsetSkip_Case1(Prev_Lv_CurMode, Cur_Lv_CurMode, Delta_L_Limit);
            Assert.That(result == expected);
        }

        [TestCase(100, 101, 200, 201, true)]//같은방향 같은 Register차이(O)
        [TestCase(100, 99, 200, 199, true)]//같은방향 같은 Register차이(O)
        [TestCase(100, 102, 200, 201, false)]//같은방향 다른 Register차이
        [TestCase(100, 98, 200, 199, false)]//같은방향 다른 Register차이
        [TestCase(100, 101, 200, 199, false)]//다른방향 같은 Register차이
        [TestCase(100, 99, 200, 201, false)]//다른방향 같은 Register차이
        [TestCase(100, 101, 200, 198, false)]//다른방향 다른 Register차이
        [TestCase(100, 99, 200, 202, false)]//다른방향 다른 Register차이

        public void OffsetSkip_Case2(int Prev_Register_PrevMode, int Cur_Register_PrevMode, int Prev_Register_CurMode, int Cur_Register_CurMode,bool expected)
        {
            bool result = Difference_Offset.OffsetSkip_Case2(Prev_Register_PrevMode,  Cur_Register_PrevMode,  Prev_Register_CurMode,  Cur_Register_CurMode);
            Assert.That(result == expected);
        }

        [TestCase(100, 101, 200, 201, false)]//같은방향 같은 Register차이
        [TestCase(100, 99, 200, 199, false)]//같은방향 같은 Register차이
        [TestCase(100, 102, 200, 201, false)]//같은방향 다른 Register차이
        [TestCase(100, 98, 200, 199, false)]//같은방향 다른 Register차이
        [TestCase(100, 101, 200, 200, false)]//하나 0 차이 (다른방향 X)
        [TestCase(100, 100, 200, 199, false)]//하나 0 차이 (다른방향 X)
        [TestCase(100, 101, 200, 199, true)]//다른방향 같은 Register차이(O)
        [TestCase(100, 99, 200, 201, true)]//다른방향 같은 Register차이(O)
        [TestCase(100, 101, 200, 198, true)]//다른방향 다른 Register차이(O)
        [TestCase(100, 99, 200, 202, true)]//다른방향 다른 Register차이(O)
        public void OffsetSkip_Case3(int Prev_Register_PrevMode, int Cur_Register_PrevMode, int Prev_Register_CurMode, int Cur_Register_CurMode, bool expected)
        {
            bool result = Difference_Offset.OffsetSkip_Case3(Prev_Register_PrevMode ,Cur_Register_PrevMode, Prev_Register_CurMode,  Cur_Register_CurMode);
            Assert.That(result == expected);
        }

        [TestCase(100, 50, 300, 310, 101, 51, 300, 311, true)]
        [TestCase(100, 50, 300, 310, 101, 49, 300, 309, true)]
        [TestCase(100, 50, 300, 310, 99, 51, 300, 311, true)]
        [TestCase(100, 50, 300, 310, 99, 49, 300, 311, true)]
        [TestCase(100, 50, 300, 310, 99, 49, 300, 309, true)]
        [TestCase(100, 50, 300, 310, 100, 49, 300, 309, false)]
        [TestCase(100, 50, 300, 310, 99, 50, 300, 309, false)]
        [TestCase(100, 50, 300, 310, 99, 49, 200, 210, false)]
        public void Applied_Condition(double Prev_Lv_PrevMode, double Cur_Lv_PrevMode, int Prev_Register_PrevMode, int Cur_Register_PrevMode
            , double Prev_Lv_CurMode, double Cur_Lv_CurMode, int Prev_Register_CurMode, int Cur_Register_CurMode, bool expected)
        {
            bool result = Difference_Offset.IsToBeApplied(Prev_Lv_PrevMode, Cur_Lv_PrevMode, Prev_Register_PrevMode, Cur_Register_PrevMode
             , Prev_Lv_CurMode, Cur_Lv_CurMode, Prev_Register_CurMode, Cur_Register_CurMode);

            Assert.AreEqual(result, expected);
        }

        [TestCase(100, 50, 300, 310, 101, 51, 300, 311, true)]
        [TestCase(100, 50, 300, 310, 99, 51, 300, 311, false)]
        [TestCase(100, 50, 300, 310, 101, 49, 300, 311, false)]
        [TestCase(100, 50, 300, 310, 101, 51, 300, 309, false)]
        public void IsCase1(double Prev_Lv_PrevMode, double Cur_Lv_PrevMode, int Prev_Register_PrevMode, int Cur_Register_PrevMode
            , double Prev_Lv_CurMode, double Cur_Lv_CurMode, int Prev_Register_CurMode, int Cur_Register_CurMode, bool expected)
        {
            bool result = Difference_Offset.IsCase1(Prev_Lv_PrevMode, Cur_Lv_PrevMode, Prev_Register_PrevMode, Cur_Register_PrevMode
             , Prev_Lv_CurMode, Cur_Lv_CurMode, Prev_Register_CurMode, Cur_Register_CurMode);

            Assert.AreEqual(result, expected);
        }


        [TestCase(100, 50, 300, 310, 101, 49, 300, 309, true)]
        [TestCase(100, 50, 300, 310, 99, 49, 300, 309, false)]
        [TestCase(100, 50, 300, 310, 101, 51, 300, 309, false)]
        [TestCase(100, 50, 300, 310, 101, 49, 300, 311, false)]
        public void IsCase2(double Prev_Lv_PrevMode, double Cur_Lv_PrevMode, int Prev_Register_PrevMode, int Cur_Register_PrevMode
    , double Prev_Lv_CurMode, double Cur_Lv_CurMode, int Prev_Register_CurMode, int Cur_Register_CurMode, bool expected)
        {
            bool result = Difference_Offset.IsCase2(Prev_Lv_PrevMode, Cur_Lv_PrevMode, Prev_Register_PrevMode, Cur_Register_PrevMode
             , Prev_Lv_CurMode, Cur_Lv_CurMode, Prev_Register_CurMode, Cur_Register_CurMode);

            Assert.AreEqual(result, expected);
        }


        [TestCase(100, 50, 300, 310, 99, 51, 300, 311, true)]
        [TestCase(100, 50, 300, 310, 101, 51, 300, 311, false)]
        [TestCase(100, 50, 300, 310, 99, 49, 300, 311, false)]
        [TestCase(100, 50, 300, 310, 99, 51, 300, 309, false)]
        public void IsCase3(double Prev_Lv_PrevMode, double Cur_Lv_PrevMode, int Prev_Register_PrevMode, int Cur_Register_PrevMode
, double Prev_Lv_CurMode, double Cur_Lv_CurMode, int Prev_Register_CurMode, int Cur_Register_CurMode, bool expected)
        {
            bool result = Difference_Offset.IsCase3(Prev_Lv_PrevMode, Cur_Lv_PrevMode, Prev_Register_PrevMode, Cur_Register_PrevMode
             , Prev_Lv_CurMode, Cur_Lv_CurMode, Prev_Register_CurMode, Cur_Register_CurMode);

            Assert.AreEqual(result, expected);
        }

        [TestCase(100, 50, 300, 310, 99, 49, 300, 311, true)]
        [TestCase(100, 50, 300, 310, 101, 49, 300, 311, false)]
        [TestCase(100, 50, 300, 310, 99, 51, 300, 311, false)]
        [TestCase(100, 50, 300, 310, 99, 49, 300, 309, false)]
        public void IsCase4(double Prev_Lv_PrevMode, double Cur_Lv_PrevMode, int Prev_Register_PrevMode, int Cur_Register_PrevMode
, double Prev_Lv_CurMode, double Cur_Lv_CurMode, int Prev_Register_CurMode, int Cur_Register_CurMode, bool expected)
        {
            bool result = Difference_Offset.IsCase4(Prev_Lv_PrevMode, Cur_Lv_PrevMode, Prev_Register_PrevMode, Cur_Register_PrevMode
             , Prev_Lv_CurMode, Cur_Lv_CurMode, Prev_Register_CurMode, Cur_Register_CurMode);

            Assert.AreEqual(result, expected);
        }

        [TestCase(100, 50, 300, 310, 99, 49, 300, 309, true)]
        [TestCase(100, 50, 300, 310, 101, 49, 300, 309, false)]
        [TestCase(100, 50, 300, 310, 99, 51, 300, 309, false)]
        [TestCase(100, 50, 300, 310, 99, 49, 300, 311, false)]
        public void IsCase5(double Prev_Lv_PrevMode, double Cur_Lv_PrevMode, int Prev_Register_PrevMode, int Cur_Register_PrevMode
, double Prev_Lv_CurMode, double Cur_Lv_CurMode, int Prev_Register_CurMode, int Cur_Register_CurMode, bool expected)
        {
            bool result = Difference_Offset.IsCase5(Prev_Lv_PrevMode, Cur_Lv_PrevMode, Prev_Register_PrevMode, Cur_Register_PrevMode
             , Prev_Lv_CurMode, Cur_Lv_CurMode, Prev_Register_CurMode, Cur_Register_CurMode);

            Assert.AreEqual(result, expected);
        }
    }
}
