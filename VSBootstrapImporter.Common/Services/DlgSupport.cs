using Ookii.Dialogs.Wpf;
using System.Diagnostics;
using System.Windows;
using VSBootstrapImporter.Common.Models;

namespace VSBootstrapImporter.Common.Services
{
    public class DlgSupport
    {
        public Window Window { get; set; }
        public string Title { get; set; }
        public bool IsBootstrapHtml { get; set; }

        public DlgSupport(Window window,
                          string title)
        {
            Window = window;
            Title = title;
        }

        public string GetProjectDirectory(string str)
        {
            string result = "";

            VistaFolderBrowserDialog dialog = new VistaFolderBrowserDialog
            {
                Description = "Please select a folder for " + str,
                UseDescriptionForTitle = true // This applies to the Vista style dialog only, not the old dialog.
            };

            if (!VistaFolderBrowserDialog.IsVistaFolderDialogSupported)
                MessageBox.Show(Window, "Because you are not using Windows Vista or later, the regular folder browser dialog will be used. Please use Windows Vista to see the new dialog.", "Sample folder browser dialog");
            if ((bool)dialog.ShowDialog(Window))
                result = dialog.SelectedPath + "\\";
            return result;
        }

        public static DataInfo GenerateData(Options currentOptions,
                                            bool previewOnly,
                                            bool allowOutput)
        {

            if (currentOptions.IsTraceOn(Trace_Options.TraceInfo))
                Trace.TraceInformation("Generating DataInfo");

            DataInfo info = Generator.CreateDataInfo(currentOptions);

            if (allowOutput == true)
            {
                if (previewOnly == false)
                    Output.OutputData(currentOptions, info);
                Output.OutputPreview(currentOptions, info);
            }

            return info;
        }

        public bool IsHtmlAllowed(Options options,
                                   string strPath,
                                   string strHtml)
        {
            IsBootstrapHtml = Generator.IsBootstrapHtml(options, strPath, strHtml);
            bool result = IsBootstrapHtml;

            if (result == false)
            {
                if (options.AllowNonBootstrap == false)
                {
                    if (MessageBox.Show("Do you wish to allow Non Bootstrap HTML?", Title, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        result = true;
                    }
                }
                else
                    result = true;
            }

            return result;
        }
    }
}