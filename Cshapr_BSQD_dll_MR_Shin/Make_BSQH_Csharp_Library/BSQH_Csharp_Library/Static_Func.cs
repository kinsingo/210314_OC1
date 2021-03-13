using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BSQH_Csharp_Library
{
    class Static_Func
    {
        public static byte[][] Get_WriteCMD_Output_bytes(byte Start_Index, byte Address, byte[] Parameters)
        {
            byte[][] Output = new byte[3][];
            Output[0] = new byte[1];//Offset
            Output[0][0] = Start_Index;

            Output[1] = new byte[1];//Address
            Output[1][0] = Address;

            Output[2] = Parameters;
            return Output;
        }

        public static byte[] Get_ReadCMD_Output_bytes(byte Start_Index, byte Address, byte Quantity)
        {
            byte[] Output = new byte[3];
            Output[0] = Start_Index;
            Output[1] = Address;
            Output[2] = Quantity;
            return Output;
        }
    }
}
