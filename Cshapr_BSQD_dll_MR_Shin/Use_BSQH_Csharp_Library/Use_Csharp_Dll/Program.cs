using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BSQH_Csharp_Library;

namespace Use_Csharp_Dll
{
    class Program
    {
        static void Main(string[] args)
        {
            DllTest test = new DllTest();
            Console.WriteLine("Sum : " + test.Add(10, 20));
            Console.ReadLine();
        }
    }
}
