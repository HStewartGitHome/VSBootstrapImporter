using System;
using System.Diagnostics;
using System.IO;

namespace VSBootstrapImporter.Common.Services
{
    public static class Shell
    {
        public static bool Start(string fileName, string arguments)
        {
            bool result = false;
            string currentDir, dir;

            currentDir = Directory.GetCurrentDirectory();

            try
            {
                dir = Path.GetDirectoryName(fileName);
                if ((dir != null) && (dir.Length > 0))
                {
                    Directory.SetCurrentDirectory(dir);
                }

                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    FileName = fileName,
                    WindowStyle = ProcessWindowStyle.Normal,
                    Arguments = arguments
                };

                using (Process exeProcess = Process.Start(startInfo))
                {
                    exeProcess.WaitForExit();
                }

                result = true;
            }
            catch (IOException e)
            {
                Console.WriteLine("Exception executing start process=" + fileName, e);
            }

            Directory.SetCurrentDirectory(currentDir);
            return result;
        }
    }
}