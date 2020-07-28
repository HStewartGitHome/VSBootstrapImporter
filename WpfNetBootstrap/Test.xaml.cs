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
        public Test()
        {
            string strFile = @"c:\temp\log\wpfdemo.log";
            IFileIO fileIO = new LogFileIO(strFile);
            FileIOFactory.SetFileIO(fileIO);
            InitializeComponent();
        }
    }
}
