using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSBootstrapImporter.Common.Services.IO;
using WpfNetBootstrap.Common.Services.IO;

namespace VSBootstrapImporter.Tests.IO
{
    public class MinTestFileIO : IFileIO
    {
        private bool _enableLogging = false;

        #region Data
        IFileIO _fileIO = null;
        string _dataDir = "";

        #endregion

        #region Contructor
        public MinTestFileIO(string dir)
        {
            if (_enableLogging)
                _fileIO = new LogFileIO(@"c:\temp\log\mintest.log");
            else
                _fileIO = new FileIO();
            SetDataDir(dir);
        }
        #endregion

        #region FileIO methods
        public void AppendFile(string fileName, string str, bool traceException)
        {
            throw new NotImplementedException();
        }

        public void CopyDirectory(string source, string destination, bool traceException)
        {
            _fileIO.CopyDirectory(source, destination, traceException);
        }

        public void CreateDirectory(string destinationDirectory, bool traceException)
        {
            _fileIO.CreateDirectory(destinationDirectory, traceException);
        }

        public bool Exists(string fileName, bool traceException)
        {
            string str = Directory.GetCurrentDirectory();
            _fileIO.WriteLog("Current Directory = " + str, traceException);
            string actualFileName = GetActualFileName(fileName);
            _fileIO.WriteLog("Actual FileName = " + actualFileName, traceException);
            return (_fileIO.Exists(actualFileName, traceException));
        }

        public void OutputFile(string fileName, List<string> strings, bool traceException)
        {

            string actualFileName = GetActualFileName(fileName);
            _fileIO.WriteLog("Actual FileName = " + actualFileName, traceException);
            _fileIO.OutputFile(actualFileName, strings, traceException);

            string str = "Lines = " + strings.Count.ToString();
            _fileIO.WriteLog(str, traceException);
            foreach(string s in strings)
            {
                str = "---" + s;
                _fileIO.WriteLog(str, traceException);
            }

            
        }

        public string[] ReadAllLines(string fileName, bool traceException)
        {
            string actualFileName = GetActualFileName(fileName);
            WriteLog("Actual FileName = " + actualFileName, traceException);
            string[] output =  _fileIO.ReadAllLines(actualFileName, traceException);

            if ( _enableLogging )
            { 
                string str = "Lines = " + output.Length.ToString();
                WriteLog(str, traceException);
                foreach (string s in output)
                {
                    str = "---" + s;
                    WriteLog(str, traceException);
                }
            }

            return output;
        }

        public void WriteLog(string str,
                          bool traceException)
        {
            if ( _enableLogging )
                _fileIO.WriteLog( str, traceException);
        }

        public void SetDataDir(string str)
        {
            _dataDir = str;
            WriteLog("#### Setting DataDir to [" + str + "]", false);
        }

        public string GetDataDir()
        {
            WriteLog("#### Getting DataDir to [" + _dataDir + "]", false);
            return _dataDir;
        }

        #endregion

        #region Support
        private string GetActualFileName(string fileName)
        {
            string str = _dataDir + Path.GetFileName(fileName);
            return str;
        }
        #endregion


    }
}
