using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace WpfNetBootstrap.Common.Services.IO
{
    public class FileIO : IFileIO
    { 

        #region File IO
        public void OutputFile(string fileName,
                               List<string> strings,
                               bool traceException)
        {
            try
            {
                if (File.Exists(fileName) == true)
                    File.Delete(fileName);
                File.AppendAllLines(fileName, strings);
            }
            catch (IOException e)
            {
                if (traceException)
                    Trace.TraceError("IO Exception outputing file" + e.ToString());
            }
        }

        public bool Exists(string fileName,
                           bool traceException)
        {
            bool result = false;
            try
            {
                result = File.Exists(fileName);
            }
            catch (IOException e)
            {
                if (traceException)
                    Trace.TraceError("IO Exception checking if file exists" + e.ToString());
            }

            return result;
        }

        public string[] ReadAllLines(string fileName, 
                                     bool traceException)
        {
            string[] strs = Array.Empty<string>();

            try
            {
                strs = File.ReadAllLines(fileName);
            }
            catch (IOException e)
            {
                if (traceException)
                    Trace.TraceError("IO Exception readings all line in file" + e.ToString());
            }
            return strs;
        }

        public void AppendFile(string fileName,
                               string str,
                               bool traceException)
        {
            try
            {
                if (!File.Exists(fileName))
                {
                    using (StreamWriter sw = File.CreateText(fileName))
                    {
                        sw.WriteLine(str);
                    }
                }
                else
                {
                    using (StreamWriter sw = File.AppendText(fileName))
                    {
                        sw.WriteLine(str);
                    }
                }
            }
            catch (IOException e)
            {
                if (traceException)
                    Trace.TraceError("IO Exception append text to file" + e.ToString());
            }
        }

        public void WriteLog(string str,
                           bool traceException)
        {
           // this is ignore during normal operations
           // use LogFileIO if desiring logging
        }

        public void SetDataDir( string str )
        {
            throw new NotImplementedException();
        }

        public string GetDataDir()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Directory IO

        public void CreateDirectory(string destinationDirectory,
                                     bool traceException)
        {
            try
            {
                Directory.CreateDirectory(destinationDirectory);
            }
            catch (IOException e)
            {
                if (traceException)
                    Trace.TraceError("IO Exception creating directory" + e.ToString());
            }
        }

        public void CopyDirectory(string source,
                                  string destination,
                                   bool traceException)
        {
            try
            {
                CreateDirectory(destination, traceException);

                foreach (var file in Directory.GetFiles(source))
                    File.Copy(file, Path.Combine(destination, Path.GetFileName(file)));

                foreach (var directory in Directory.GetDirectories(source))
                    CopyDirectory(directory, Path.Combine(destination, Path.GetFileName(directory)), traceException);
            }
            catch (IOException e)
            {
                if (traceException)
                    Trace.TraceError("IO Exception copying directory" + e.ToString());
            }
        }
        #endregion

    }
}
