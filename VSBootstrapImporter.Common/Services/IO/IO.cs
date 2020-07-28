using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSBootstrapImporter.Common.Models;

namespace WpfNetBootstrap.Common.Services.IO
{
    public static class CommonIO
    {
        #region File IO
      
        public static void OutputFile(string fileName,
                                      List<string> strings,
                                      Options options)
        {
            bool traceException = options.IsTraceOn(Trace_Options.TraceExceptions);
            IFileIO fileIO = FileIOFactory.GetFileIO();
            fileIO.OutputFile(fileName, strings, traceException);
        }

        public static bool Exists(string fileName,
                                  Options options)
        {
            bool traceException = options.IsTraceOn(Trace_Options.TraceExceptions);
            IFileIO fileIO = FileIOFactory.GetFileIO();
            return fileIO.Exists(fileName, traceException);
        }

        internal static string[] ReadAllLines(string fileName, Options options)
        {
            bool traceException = options.IsTraceOn(Trace_Options.TraceExceptions);
            IFileIO fileIO = FileIOFactory.GetFileIO();
            return fileIO.ReadAllLines(fileName, traceException);
        }

        #endregion

         #region Directory io
        public static void CreateDirectory(string destinationDirectory,
                                           Options options)
        {
            bool traceException = options.IsTraceOn(Trace_Options.TraceExceptions);
            IFileIO fileIO = FileIOFactory.GetFileIO();
            fileIO.CreateDirectory(destinationDirectory, traceException);
        }

        public static void CopyDirectory(string source,
                                         string destination,
                                         Options options)
        {
            bool traceException = options.IsTraceOn(Trace_Options.TraceExceptions);
            IFileIO fileIO = FileIOFactory.GetFileIO();
            fileIO.CopyDirectory(source, destination, traceException);
        }

  
        #endregion
    }
}
