using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfNetBootstrap.Common.Services.IO;

namespace VSBootstrapImporter.Common.Services.IO
{
    public class LogFileIO : IFileIO
    {
        private readonly string _fileName = "";
        private readonly FileIO _fileIO = null;

        public LogFileIO( string fileName)
        {
            _fileName = fileName;
            _fileIO = new FileIO();
        }

        public void CopyDirectory(string source, string destination, bool traceException)
        {
            string str = "CopyDirectory Source=" + source + " to Destination=" + destination;
            WriteLog(str, traceException);
            _fileIO.CopyDirectory(source, destination, traceException);
        }

        public void CreateDirectory(string destinationDirectory, bool traceException)
        {
            string str = "CreateDirectory Destination=" + destinationDirectory;
            WriteLog(str, traceException);
            _fileIO.CreateDirectory(destinationDirectory, traceException);
        }

        public bool Exists(string fileName, bool traceException)
        {
            bool result = _fileIO.Exists(fileName, traceException);
            string str = "Exists FileName=" + fileName + " with result=" + result.ToString();

            WriteLog(str, traceException);

            if (result == false)
                WriteLog("#### Failure ####", traceException);
            return result;
        }

        public void OutputFile(string fileName, List<string> strings, bool traceException)
        {
            string str = "OutputFile FileName=" + fileName;
            WriteLog(str, traceException);
            _fileIO.OutputFile(fileName, strings, traceException);
        }

        public string[] ReadAllLines(string fileName, bool traceException)
        {
            string str = "ReadAllLine FileName=" + fileName;
            WriteLog(str, traceException);
            return _fileIO.ReadAllLines(fileName, traceException);
        }

        public void AppendFile(string fileName,
                               string str,
                               bool traceException)
        {
            string s = "AppendFile FileName=" + fileName;
            WriteLog(s, traceException);
            _fileIO.AppendFile(fileName, str, traceException);
        }

        public  void WriteLog( string str,
                               bool traceException)
        {
            _fileIO.AppendFile(_fileName, str, traceException);
        }

        public void SetDataDir(string str)
        {
            throw new NotImplementedException();
        }

        public string GetDataDir()
        {
            throw new NotImplementedException();
        }
    }
}
