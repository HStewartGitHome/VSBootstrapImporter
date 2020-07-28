using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System;
using VSBootstrapImporter.Common.Models;
using WpfNetBootstrap.Common.Services.IO;

namespace VSBootstrapImporter.Common.Services
{
    public class Output : CommonSupport
    {
        public void OutputData(Options options,
                               DataInfo info)
        {
            try
            {
                // output blazer file
                if (options.CanOutput() == true)
                {
                    if (options.SelectedModifyOptions != Modify_Options.PreviewOnly)
                    {
                        OutputBlazorFile(options, info);
                        if (info.ProjectType == Type_Options.ASPNetRazor)
                            OutputRazorCodeBehindFile(options, info);
                    }
                    if (options.SelectedHostOptions == Host_Options.ModifyHost)
                    {
                        if ((info.ProjectType == Type_Options.ASPNetRazor) &&
                            (options.MakeCustomRazorLayout == true))
                        {
                            OutputCustomRazorLayout(options, info);
                        }

                        if ((options.IsUseCustomAssets() == true) ||
                            (options.IsUseSharedAssets() == true))
                        {
                            OutputHostFile(options, info);
                            if (options.IsUseMoveAssets() == true)
                                MoveAssetsDirectory(options, info);
                            else
                                CopyAssetsDirectory(options);
                        }
                    }
                    if (options.SelectedNavMenuOptions == NavMenu_Options.AddMenuItem)
                        OutputNavMenuFile(options, info);
                }
                info.IsModificationsSuccessfull = true;
            }
            catch(IOException e)
            {
                if (options.IsTraceOn(Trace_Options.TraceExceptions))
                    Trace.TraceError("Exception outputting data", e);
            }
        }

        public static void OutputPreview(Options options,
                                  DataInfo info)
        {
            if (options.HasPreview() == true)
            {
                string previewFile = options.PreviewFile;

                if (options.IsTraceOn(Trace_Options.TraceInfo))
                    Trace.TraceInformation("Preview  File = [" + previewFile + "]");

                CommonIO.OutputFile(previewFile, info.PreviewStrings, options);
            }
        }

        public static void OutputHostFile(Options options,
                                          DataInfo info)
        {
            if (info.NewHostStrings.Count > 0)
            {
                string htmlFile = GetHostFile(options, info);

                if (options.IsTraceOn(Trace_Options.TraceInfo))
                    Trace.TraceInformation("Host File = [" + htmlFile + "]");

                CommonIO.OutputFile(htmlFile, info.NewHostStrings, options);
            }
        }

        public static void OutputNavMenuFile(Options options,
                                             DataInfo info)
        {
            if (info.NewNavMenuStrings.Count > 0)
            {
                string strFile = GetNavMenuFile(options, info);

                if (options.IsTraceOn(Trace_Options.TraceInfo))
                    Trace.TraceInformation("NavMenu File = [" + strFile + "]");

                CommonIO.OutputFile(strFile, info.NewNavMenuStrings, options);
            }
        }


        private void MoveAssetsDirectory(Options options,
                                         DataInfo info)
        {
            string sourceDirectory = GetSourceDirectory(options);
            string destinationDirectory = GetDestinationDirectory(options);

            try
            {
                if (options.IsTraceOn(Trace_Options.TraceInfo))
                {
                    Trace.TraceInformation("Moving Assets Source=" + sourceDirectory);
                    Trace.TraceInformation("              Dest  =" + destinationDirectory);
                }

                CommonIO.CreateDirectory(destinationDirectory, options);

                foreach (Asset TheAsset in info.Assets)
                {
                    if (TheAsset.AssetType == AssetType_Options.ImageAsset)
                    {
                        string sourceFile = options.BootstrapProjectPath + TheAsset.HtmlAsset;
                        string destFile = destinationDirectory + TheAsset.RawAsset;
                        string sourcePath = Path.GetDirectoryName(sourceFile);
                        string destPath = Path.GetDirectoryName(destFile) + @"\assets\img";

                        if (options.IsTraceOn(Trace_Options.TraceInfo))
                        {
                            Trace.TraceInformation("Copying Assets Source=" + sourcePath);
                            Trace.TraceInformation("               Dest  =" + destPath);
                        }

                        CommonIO.CopyDirectory(sourcePath, destPath, options);
                    }
                    else if (TheAsset.AssetType == AssetType_Options.HostAsset)
                    {
                        string sourcePath = options.BootstrapProjectPath + TheAsset.RawAsset;
                        string destPath = options.BlazorProjectPath + "wwwroot\\" + options.PageName + "\\" + TheAsset.RawAsset;

                        if (options.IsTraceOn(Trace_Options.TraceInfo))
                        {
                            Trace.TraceInformation("Copying Assets Source=" + sourcePath);
                            Trace.TraceInformation("               Dest  =" + destPath);
                        }

                        CommonIO.CopyDirectory(sourcePath, destPath, options);
                    }
                    else if (TheAsset.AssetType == AssetType_Options.ScriptAsset)
                    {
                        string sourcePath = options.BootstrapProjectPath + TheAsset.RawAsset;
                        string destPath = options.BlazorProjectPath + "wwwroot\\" + options.PageName + "\\" + TheAsset.RawAsset;

                        if (options.IsTraceOn(Trace_Options.TraceInfo))
                        {
                            Trace.TraceInformation("Copying Assets Source=" + sourcePath);
                            Trace.TraceInformation("               Dest  =" + destPath);
                        }

                        CommonIO.CopyDirectory(sourcePath, destPath,options);
                    }
                }
            }
            catch (IOException e)
            {
                if (options.IsTraceOn(Trace_Options.TraceExceptions))
                    Trace.TraceError("IO Exception copying assets" + e.ToString());
            }
        }

        private void CopyAssetsDirectory(Options options)
        {
            string sourceDirectory = GetSourceDirectory(options);
            string destinationDirectory = GetDestinationDirectory(options);

            if (options.IsTraceOn(Trace_Options.TraceInfo))
            {
                Trace.TraceInformation("Copying Assets Source=" + sourceDirectory);
                Trace.TraceInformation("               Dest  =" + destinationDirectory);
            }
            CommonIO.CopyDirectory(sourceDirectory, destinationDirectory, options);
        }

  

        private void OutputBlazorFile(Options options,
                                      DataInfo info)
        {
            string blazerFile = GetRazorFile(options, info);
            if (options.IsTraceOn(Trace_Options.TraceInfo))
                Trace.TraceInformation("Blazor File = [" + blazerFile + "]");

            CommonIO.OutputFile(blazerFile, info.BlazorStrings, options);
        }

        private void OutputRazorCodeBehindFile(Options options,
                                               DataInfo info)
        {
            string blazerFile = GetRazorFile(options, info) + ".cs";
            Console.WriteLine("Razor Code Behind File = [" + blazerFile + "]");

            CommonIO.OutputFile(blazerFile, info.RazorCodeBehindStrings, options);
        }

        private void OutputCustomRazorLayout(Options options,
                                             DataInfo info)
        {
            string strFile = GetRazorCustomLayoutFile(options, info);

            if (options.IsTraceOn(Trace_Options.TraceInfo))
                Trace.TraceError("Razor Custom Layout File = [" + strFile + "]");

            CommonIO.OutputFile(strFile, info.RazorCustomLayoutStrings, options);
        }
    }
}