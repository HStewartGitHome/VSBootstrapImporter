using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSBootstrapImporter.Tests.Models;

namespace VSBootstrapImporter.Tests.Services
{
    public static class FileFactory
    {
        #region Data
        static readonly List<FileData> _fileData = new List<FileData>();
        #endregion

        #region Public functions
        public static void Initialize()
        {
            // Expected Blazor files
            List<string> strings = Files.MinNavMenu_Razor_File();
            FileData data = CreateFileData(strings, "MinNavMenu.Razor");
            _fileData.Add(data);
            strings = Files.MinMin_Razor_File();
            data = CreateFileData(strings, "MinMin.Razor");
            _fileData.Add(data);
            strings = Files.Min_Host_CSHtml_File();
            data = CreateFileData(strings, "Min_Host.cshtml");
            _fileData.Add(data);

            // Expected Blazor Wasm files
            strings = Files.Min_Wasm_NavMenu_Razor_File();
            data = CreateFileData(strings, "WasmNavMenu.Razor");
            _fileData.Add(data);
            strings = Files.Wasm_Razor_File();
            data = CreateFileData(strings, "WasmWasm.Razor");
            _fileData.Add(data);
            strings = Files.Min_Wasm_Index_Html_File();
            data = CreateFileData(strings, "WasmIndex.html");
            _fileData.Add(data);

            // Expected Blazor Wasm PWA files
            strings = Files.Min_Wasm_PWA_NavMenu_Razor_File();
            data = CreateFileData(strings, "WasmPWANavMenu.Razor");
            _fileData.Add(data);
            strings = Files.Wasm_PWA_Razor_File();
            data = CreateFileData(strings, "WasmPWAWasmPWA.Razor");
            _fileData.Add(data);
            strings = Files.Min_Wasm_PWA_Index_Html_File();
            data = CreateFileData(strings, "WasmPWAIndex.html");
            _fileData.Add(data);

            // Expected Razor files

            // Expected Blazor Wasm PWA files
            strings = Files.MIN_Razor_CSHtml_File();
            data = CreateFileData(strings, "RazorRazor.cshtml");
            _fileData.Add(data);
            strings = Files.MIN_Razor_CSHtml_CS_File();
            data = CreateFileData(strings, "RazorRazor.cshtml.cs");
            _fileData.Add(data);
            strings = Files.MIN_Razor_Layout_CSHtml_File();
            data = CreateFileData(strings, "Razor_Layout.cshtml");
            _fileData.Add(data);
            strings = Files.MIN_Razor_Razor_Layout_CSHtml_File();
            data = CreateFileData(strings, "RazorRazor_Layout.cshtml");
            _fileData.Add(data);

        }

        public static string[] LoadFileStrings( string fileName )
        {
            string[] stringsResult = null;
            string actualFileName = Path.GetFileName(fileName).ToUpper(); ;

            foreach( FileData data in _fileData )
            {
                if (actualFileName.CompareTo(data.FileName.ToUpper()) == 0)
                    stringsResult = data.Lines;
            }

            return stringsResult;
        }
        #endregion

        #region Private methods
        private static FileData CreateFileData(List<string> strings,
                                               string       fileName)
        {
            FileData data = new FileData
            {
                FileName = Path.GetFileName(fileName),
                Lines = strings.ToArray()
            };

            return data;

        }
        #endregion


    }
}
