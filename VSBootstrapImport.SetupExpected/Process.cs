using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using VSBootstrapImport.SetupExpected.Models;
using WpfNetBootstrap.Common.Services.IO;

namespace VSBootstrapImport.SetupExpected
{
    public class Process
    {
        #region Data
        readonly List<FileDataInfo> _fileInfos = null;
        readonly List<string> _strings = null;
        readonly IFileIO _fileIO = null;

        public string OutputFilePath { get; set; }
        #endregion

        #region Constructor
        public Process()
        {
            _fileInfos = new List<FileDataInfo>();
            _strings = new List<string>();
            _fileIO = new FileIO();

            OutputFilePath = @"C:\SRC\VSBootstrapImporter\VSBootstrapImporter.Tests\Services\files.cs";
        }
        #endregion

        public void Run()
        {
            InitializeFileInfos();
            CreateHeader();

            foreach (FileDataInfo info in _fileInfos)
                AddFileInfo(info);

            CreateFooter();
        }

        public void CreateFile()
        {
            string fileName = OutputFilePath;

            Console.WriteLine("Outputing file [" + fileName + "] with " + _strings.Count.ToString() + " Lines");
            foreach (string s in _strings)
                Console.WriteLine("-----" + s);

            _fileIO.OutputFile(fileName, _strings, false);

            Console.WriteLine("Finish Outputing File [" + fileName + "]");
        }

        #region Initialize private methods
        private void InitializeFileInfos()
        {
            // change the contents of this method if desired to change paths

            
            // Expected Blazor files
            // Minimun _Host.cshtml for blazor 
            FileDataInfo info = new FileDataInfo
            {
                FilePath = @"C:\SRC\VSBootstrapImporter\VSBootstrapImporter.Tests\MinBlazor\Min_Host.cshtml",
                Method = "Min_Host_CSHtml_File"
            };
            _fileInfos.Add(info);

            // Minimun NavMenu.Razor for blazor
            info = new FileDataInfo
            {
                FilePath = @"C:\SRC\VSBootstrapImporter\VSBootstrapImporter.Tests\MinBlazor\MinNavMenu.Razor",
                Method = "MinNavMenu_Razor_File"
            };
            _fileInfos.Add(info);

            // Minimun Min.Razor for blazor wasm
            info = new FileDataInfo
            {
                FilePath = @"C:\SRC\VSBootstrapImporter\VSBootstrapImporter.Tests\MinBlazor\MinMin.Razor",
                Method = "MinMin_Razor_File"
            };
            _fileInfos.Add(info);

            // Expected Blazor Wasm files
            // Minimun _Host.cshtml for blazor 
            info = new FileDataInfo
            {
                FilePath = @"C:\SRC\VSBootstrapImporter\VSBootstrapImporter.Tests\MinWasm\WasmIndex.html",
                Method = "Min_Wasm_Index_Html_File"
            };
            _fileInfos.Add(info);

            // Minimun NavMenu.Razor for blazor wasm
            info = new FileDataInfo
            {
                FilePath = @"C:\SRC\VSBootstrapImporter\VSBootstrapImporter.Tests\MinWasm\WasmNavMenu.Razor",
                Method = "Min_Wasm_NavMenu_Razor_File"
            };
            _fileInfos.Add(info);

            // Minimun Wasm.Razor for blazor wasm
            info = new FileDataInfo
            {
                FilePath = @"C:\SRC\VSBootstrapImporter\VSBootstrapImporter.Tests\MinWasm\Wasm.Razor",
                Method = "Wasm_Razor_File"
            };
            _fileInfos.Add(info);


            // Expected Blazor Wasm PWA files
            // Minimun index.html for blazor 
            info = new FileDataInfo
            {
                FilePath = @"C:\SRC\VSBootstrapImporter\VSBootstrapImporter.Tests\MinWasmPWA\WasmPWAIndex.html",
                Method = "Min_Wasm_PWA_Index_Html_File"
            };
            _fileInfos.Add(info);

            // Minimun NavMenu.Razor for blazor wasm
            info = new FileDataInfo
            {
                FilePath = @"C:\SRC\VSBootstrapImporter\VSBootstrapImporter.Tests\MinWasmPWA\WasmPWANavMenu.Razor",
                Method = "Min_Wasm_PWA_NavMenu_Razor_File"
            };
            _fileInfos.Add(info);

            // Minimun Wasm.Razor for blazor wasm
            info = new FileDataInfo
            {
                FilePath = @"C:\SRC\VSBootstrapImporter\VSBootstrapImporter.Tests\MinWasmPWA\WasmPWA.Razor",
                Method = "Wasm_PWA_Razor_File"
            };
            _fileInfos.Add(info);

            // Expected _Layout.cshtml files
            // Minimun _Layout.cshtml for blazor 
            info = new FileDataInfo
            {
                FilePath = @"C:\SRC\VSBootstrapImporter\VSBootstrapImporter.Tests\MinRazor\Razor_Layout.cshtml",
                Method = "Min_Razor_Layout_CSHtml_File"
            };
            _fileInfos.Add(info);

            // Minimun NavMenu.Razor for blazor wasm
            info = new FileDataInfo
            {
                FilePath = @"C:\SRC\VSBootstrapImporter\VSBootstrapImporter.Tests\MinRazor\WasmPWANavMenu.Razor",
                Method = "Min_Wasm_PWA_NavMenu_Razor_File"
            };
            _fileInfos.Add(info);

            // Minimun Razor.CSHtml for Razor
            info = new FileDataInfo
            {
                FilePath = @"C:\SRC\VSBootstrapImporter\VSBootstrapImporter.Tests\MinRazor\Razor.cshtml",
                Method = "MIN_Razor_CSHtml_File"
            };
            _fileInfos.Add(info);

            // Minimun Razor.CSHtml.cs for Razor
            info = new FileDataInfo
            {
                FilePath = @"C:\SRC\VSBootstrapImporter\VSBootstrapImporter.Tests\MinRazor\Razor.cshtml.cs",
                Method = "MIN_Razor_CSHtml_CS_File"
            };
            _fileInfos.Add(info);

            // Minimun Razor_Layout.CSHtml for Razor
            info = new FileDataInfo
            {
                FilePath = @"C:\SRC\VSBootstrapImporter\VSBootstrapImporter.Tests\MinRazor\Razor_Razor_Layout.cshtml",
                Method = "MIN_Razor_Razor_Layout_CSHtml_File"
            };
            _fileInfos.Add(info);


            // Minimun _Layout.CSHtml for Razor
            info = new FileDataInfo
            {
                FilePath = @"C:\SRC\VSBootstrapImporter\VSBootstrapImporter.Tests\MinRazor\Razor_Layout.cshtml",
                Method = "MIN_Razor_Layout_CSHtml_File"
            };
            _fileInfos.Add(info);

            Console.WriteLine("Initialize " + _fileInfos.Count.ToString() + " files");

        }
        #endregion

        #region Header and footer methods
        private void CreateHeader()
        {
            Console.WriteLine("Adding Header");
            AddString("// auto generated by VSBootstrapImport.SetupExpected");
            AddString("using System.Collections.Generic;");
            AddString("");
            AddString("namespace VSBootstrapImporter.Tests.Services");
            AddString("{");
            AddString("   public static class Files");
            AddString("   {");
        }

        private void CreateFooter()
        {
            Console.WriteLine("Adding Footer");
            AddString("   }");
            AddString("}");
            AddString("");
        }

        #endregion

        #region main File method
        private void AddFileInfo( FileDataInfo info)
        {
            string str, strEnd;
            string[] strings = _fileIO.ReadAllLines(info.FilePath, false);
            int index = 1;
            int count = strings.Length;

            str = "Adding File [" + info.FilePath + "] Count=" + count.ToString();
            Console.WriteLine(str);

            if ( count > 0 )
            {
                AddString("       public static List<string> " + info.Method + "()");
                AddString("       {");
                AddString("            List<string> strings = new List<string>");
                AddString("            {");

                foreach( string s in strings )
                {
                    if (index < count)
                        strEnd = ",";
                    else
                        strEnd = "";
                    str = AdjustSlashs(s);
                    str = AdjustQuotes(str);
                    AddString("                 " + Quote(str) + strEnd);
                }

                AddString("            };");
                AddString("");
                AddString("            return strings;");
                AddString("       }");
            }
        }
        #endregion

        #region support methods
        private string Quote(string str)
        {
            string result = '"' + str + '"';
            return result;
        }

        private string AdjustQuotes(string str)
        {
            string result = str.Replace(@"""", @"\""");
            return result;
        }

        private string AdjustSlashs(string str)
        {
            string result = str.Replace(@"\", @"\\");
            return result;
        }

        private void AddString( string str )
        {
            _strings.Add(str);
            // Console.WriteLine(str);
        }
        #endregion
    }
}
