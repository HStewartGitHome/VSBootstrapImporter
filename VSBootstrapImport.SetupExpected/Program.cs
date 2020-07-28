using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSBootstrapImport.SetupExpected
{
    class Program
    {
        static void Main()
        {
            Process process = new Process();

            Console.WriteLine("VSBootstrapImport.SetupExpected create contents for file.cs");
            Console.WriteLine("");
            process.Run();
            Console.WriteLine("");
            Console.WriteLine("Finish processing");
            Console.ReadLine();
            Console.WriteLine("Creating [" + process.OutputFilePath + "]");
            process.CreateFile();
            Console.WriteLine("");
            Console.ReadLine();
        }
    }
}
