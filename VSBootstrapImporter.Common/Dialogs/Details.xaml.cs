using System.Windows;
using VSBootstrapImporter.Common.Models;
using VSBootstrapImporter.Common.Services;

namespace VSBootstrapImporter
{
    /// <summary>
    /// Interaction logic for Details.xaml
    /// </summary>
    public partial class Details : Window
    {
        private readonly DataInfo _currentInfo = null;
        private readonly Options _currentOptions = null;

        public Details(DataInfo info,
                       Options options)
        {
            InitializeComponent();
            _currentInfo = info;
            _currentOptions = options;

            InitializeControls();
        }

        // commands

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }

        // support methods

        private void InitializeControls()
        {
            HTML.Text = Generator.GetBootstrapHtml(_currentOptions);
            HTMLType.Text = _currentInfo.GetIsBootstrapString();
            ImgAsset.Text = _currentOptions.GetImageAssetString();
            Page.Text = _currentOptions.PageName;
            Project.Text = _currentOptions.BlazorProjectPath;
            ProjectType.Text = Options.GetProjectTypeString(_currentInfo.ProjectType);
            if (_currentInfo.ProjectType == Type_Options.BlazorServer)
                RenderMode.Text = _currentOptions.GetRenderModeString();
            else
                RenderMode.Text = "Not appliable";
            if (_currentInfo.IsWebAssembly() == true)
            {
                if (_currentOptions.IsCSSOptionsWebassemblyAllowed() == true)
                    CSSIsolation.Text = "Custom CSS is shared between pages";
                else
                    CSSIsolation.Text = "Not supported";
            }
            else
                CSSIsolation.Text = "Limited";

            Scripts.Text = _currentInfo.ScriptAssets.Count.ToString();
            Embedded.Text = _currentInfo.MultiLineScriptCount.ToString();
        }
    }
}