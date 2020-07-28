using System.Collections.Generic;

namespace WpfNetBootstrap.Common.Services.IO
{
    public interface IFileIO
    {
        void CopyDirectory(string source, string destination, bool traceException);
        void CreateDirectory(string destinationDirectory, bool traceException);
        bool Exists(string fileName, bool traceException);
        void OutputFile(string fileName, List<string> strings, bool traceException);
        string[] ReadAllLines(string fileName, bool traceException);
        void AppendFile(string fileName, string str, bool traceException);
        void WriteLog(string str, bool traceException);
        void SetDataDir(string str);
        string GetDataDir();
    }
}