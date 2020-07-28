using System.IO;
using VSBootstrapImporter.Common.Models;

namespace VSBootstrapImporter.Common.Services
{
    // this classed is designed to be inherited
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1052:Static holder types should be Static or NotInheritable", Justification = "<Pending>")]
    public class CommonSupport
    {
        public static string GetSourceDirectory(Options options)
        {
            string result;
            if (options.AllowHtmlEditing)
                result = Path.GetDirectoryName(options.BootStrapOverrideHtml) + "\\" + options.BootstrapAssets;
            else
                result = options.BootstrapProjectPath + options.BootstrapAssets;

            return result;
        }

        public static string GetDestinationDirectory(Options options)
        {
            string result = options.BlazorProjectPath + options.HostAssetsDirectory;

            return result;
        }

        public static string GetBootstrapHtml(Options options)
        {
            string result;
            if (options.AllowHtmlEditing == true)
                result = options.BootStrapOverrideHtml;
            else
                result = options.BootstrapProjectPath + options.BootstrapHtml;

            return result;
        }

        public static string GetRazorFile(Options options,
                                   DataInfo info)
        {
            string result;
            if (info.ProjectType == Type_Options.ASPNetRazor)
                result = options.BlazorProjectPath + "pages\\" + options.PageName + ".cshtml";
            else
                result = options.BlazorProjectPath + "pages\\" + options.PageName + ".razor";

            return result;
        }

        public static string GetNavMenuFile(Options options,
                                            DataInfo info)
        {
            string result;
            if (info.ProjectType == Type_Options.ASPNetRazor)
                result = options.BlazorProjectPath + options.RazorHostHtml;
            else
                result = options.BlazorProjectPath + options.NavMenuFile;

            return result;
        }

        public static string GetHostFile(Options options,
                                         DataInfo info)
        {
            string result;
            if (info.ProjectType == Type_Options.ASPNetRazor)
                result = options.BlazorProjectPath + options.RazorHostHtml;
            else if (info.IsWebAssembly() == true)
                result = options.BlazorProjectPath + options.WasmHostHtml;
            else
                result = options.BlazorProjectPath + options.HostHtml;

            return result;
        }



        public static Asset MakeImageAsset(string str)

        {
            Asset theAsset = null;
            bool continueExecution = false;
            string searchStr = "";
            char quoteChar = '"';
            string strResult;

            // example of asset string
            //   <figure class="figure"><img class="img-fluid figure-img" src="assets/img/interior.jpg">

            if (str.Contains("<img") == true)
            {
                if (str.Contains("src") == true)
                {
                    continueExecution = true;
                    searchStr = "src";
                }
            }

            if (continueExecution == true)
            {
                int searchIndex = str.IndexOf(searchStr);
                if ((searchIndex > 0) && (searchIndex < (str.Length - 1)))
                {
                    searchIndex = str.IndexOf(quoteChar, searchIndex);
                    if (searchIndex > 0)
                    {
                        searchIndex++;
                        int searchIndex2 = str.IndexOf(quoteChar, searchIndex);
                        if (searchIndex2 > searchIndex)
                        {
                            strResult = str.Substring(searchIndex, searchIndex2 - searchIndex);
                            theAsset = new Asset(strResult, AssetType_Options.ImageAsset);
                        }
                    }
                }
            }

            return theAsset;
        }

        public static Asset MakeHostAsset(Options options,
                                   string str)
        {
            Asset theAsset = null;
            string strCheck = "";

            if (str.Contains("stylesheet") == true)
            {
                if (str.Contains(@"assets/css") == true)
                    strCheck = @"assets/css";
                else if (str.Contains(@"assets/fonts") == true)
                    strCheck = @"assets/fonts";
                else if (str.Contains(@"assets/bootstrap") == true)
                {
                    if (options.IsCSSBootstrapOption() == true)
                    {
                        strCheck = @"assets/bootstrap"; ;
                    }
                }

                if (strCheck.Length > 0)
                {
                    theAsset = new Asset(str, AssetType_Options.HostAsset)
                    {
                        RawAsset = strCheck
                    };
                }
            }
            return theAsset;
        }

        #region Razor support
        public static string GetRazorCustomLayoutFile(Options options,
                                                       DataInfo info)
        {
            string result = "";

            if ((info.ProjectType == Type_Options.ASPNetRazor) &&
                 (options.MakeCustomRazorLayout == true))
            {
                result = options.BlazorProjectPath + "Pages\\" + options.PageName + "_Layout.cshtml";
            }
            return result;
        }
        #endregion
    }
}