using System.Windows;
using VSBootstrapImporter.Common.Services.IO;
using WpfNetBootstrap.Common.Services.IO;

namespace WpfNetBootstrap
{
    /// <summary>
    /// Interaction logic for Test.xaml
    /// </summary>
    public partial class Test : Window
    {
        // logging is turn off for check in version
        private readonly bool _enableLogging = false;
        public Test()
        {
            if (_enableLogging)
            {
                string strFile = @"c:\temp\log\wpfdemo.log";
                IFileIO fileIO = new LogFileIO(strFile);
                FileIOFactory.SetFileIO(fileIO);
            }
            else
            {
                IFileIO fileIO = new FileIO();
                FileIOFactory.SetFileIO(fileIO);
            }
            InitializeComponent();
        }
    }
}
