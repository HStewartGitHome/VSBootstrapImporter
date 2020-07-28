using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSBootstrapImporter.Tests.Models;
using WpfNetBootstrap.Common.Services.IO;

namespace VSBootstrapImporter.Tests.Services
{
    public static class Compare
    {
        static IFileIO _fileIO = null;

        #region Public static functions
        public static CompareResult CompareGeneratedFile(string fileName, string pageName, string dataDir)
        {
            string actual = fileName;
            string expected = pageName + fileName;

            return CompareFile(actual, expected, dataDir);
        }
        public static CompareResult CompareFile( string actual, string expected, string dataDir )
        {
            CompareTest test = new CompareTest
            {
                Actual = actual,
                Expected = expected
            };
   
            return CompareFilesTest(test, dataDir);
        }
        public static CompareResult CompareFilesTest( CompareTest test,
                                                      string dataDir)
        {
            CompareResult result = new CompareResult();



            
            _fileIO = FileIOFactory.GetFileIO();

            string str = Directory.GetCurrentDirectory();
            Trace.TraceInformation("Current Directory = " + str, false);

            string actualFileName = GetDeployedString(test.Actual, dataDir);
            string[] files1 = LoadStrings(actualFileName);
            string[] files2 = FileFactory.LoadFileStrings(test.Expected);
            if ( files1 != null)
                result.File1Count = files1.Length;
            if ( files2 != null)
                result.File2Count = files2.Length;
            if ( (files1 != null ) && 
                 (files2 != null ) &&
                 (files1.Length == files2.Length))
            {
                result.CountDifferent = false;
                if (CompareStringArrays(files1, files2))
                    result.ContentsDifferent = false;
            }
            str = "#### File1 [" + test.Actual + "] Count=" + result.File1Count.ToString();
            Trace.TraceInformation(str);
            _fileIO.WriteLog(str, false);
            str = "#### File2 [" + test.Expected + "] Count=" + result.File2Count.ToString();
            Trace.TraceInformation(str);
            _fileIO.WriteLog(str, false);
            str = "#### Files Contents Is Different = " + result.ContentsDifferent.ToString();
            Trace.TraceInformation(str);
            _fileIO.WriteLog(str, false);
            return result;
        }

        public static void LogFailure( bool result,
                                       bool expected,
                                       string msg )
        {
            if ( result != expected )
            {
                _fileIO = FileIOFactory.GetFileIO();
                string str = "##### Failure: Result=" + result.ToString() + " Expected=" + expected.ToString() + " Message=[" + msg + "]";
                _fileIO.WriteLog(str, false);
               
            }
        }

        
        #endregion

        #region private functions
        // private functions
        private static string GetDeployedString(string fileName, string dataDir)
        {
            string actualFileName = dataDir + Path.GetFileName(fileName);
            return actualFileName;
        }

        private static string[] LoadStrings(string fileName)
        {
            string[] output;

            _fileIO = FileIOFactory.GetFileIO();

            if (_fileIO.Exists(fileName, false) == false)
            {
                string str = "#### " + fileName + " does not exist";
                Trace.TraceWarning(str);
                _fileIO.WriteLog(str, false);
            }
            
            output = _fileIO.ReadAllLines(fileName, false);
           

            return output;
        }
        static bool CompareStringArrays( string[] array1, string[] array2 )
        {
            bool result = false;
            string str1, str2;
   
            if ( array1.Length == array2.Length )
            {
                int count = array1.Length;
                result = true;
                for(int index=0;index<count;index++)
                {
                    str1 = array1[index];
                    str2 = array2[index];
                    if ( (string.IsNullOrEmpty(str1)) ||
                         (string.IsNullOrEmpty(str2)))
                    {
                        if (str1.Length != str2.Length)
                        {
                            string str = "#### String 1 Lenght of " + str1.Length.ToString() + " does not equal String 2 Lenght of " + str1.Length.ToString();
                            Trace.TraceWarning(str);
                            _fileIO.WriteLog(str, false);
                            result = false;
                            index = count;
                        }
                    }
                    else if ( str1.CompareTo(str2) != 0)
                    {
                        string str = "#### String 1: [" + str1 + "]";
                        Trace.TraceWarning(str);
                        _fileIO.WriteLog(str, false);
                        str = "#### String 2: [" + str2 + "]";
                        Trace.TraceWarning(str);
                        _fileIO.WriteLog(str, false);
                        result = false;
                        index = count;
                    }
                }
            }
            return result;
        }

        #endregion

    }
}
