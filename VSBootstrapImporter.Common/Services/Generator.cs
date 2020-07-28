using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using VSBootstrapImporter.Common.Models;
using WpfNetBootstrap.Common.Services.IO;

namespace VSBootstrapImporter.Common.Services
{
    public class Generator : CommonSupport
    {
        public static DataInfo CreateDataInfo(Options options)
        {
            DataInfo info = new DataInfo();
            if (options.IsUseSameIndexAssets() == false)
            {
                options.IndexPageName = options.PageName;
            }

            info.ProjectType = GetProjectType(options);
            info.HtmlStrings = MakeHtml(options);
            info.HostStrings = MakeHostHtml(info, options);
            info.AssetsStrings = MakeAssets(info, options);
            info.Assets = MakeScriptAssets(info, options);

            if (options.IsUseMoveAssets() == true)
            {
                info.Assets = CreateMoveAssets(info, options);
                info.HtmlStrings = AdjustHtmlForMoveAssets(info, options);
            }

            if (info.ProjectType == Type_Options.ASPNetRazor)
            {
                options.RazorNameSpace = GetRazorNameSpace(options);
                info.BlazorStrings = MakeRazor(info, options);
                info.RazorCodeBehindStrings = MakeRazorCodeBehind(options);
                info.HostAssetsStrings = MakeHostAssets(info, options);
                info.NewHostStrings = MakeNewHostStrings(info);
                info.NavMenuStrings = MakeRazorNavMenuStrings(options);
                info.NewNavMenuStrings = MakeNewNavMenuStrings(info, options);
                info.RazorAssetsStrings = MakeRazorHostAssets(info, options);
                info.RazorCustomLayoutStrings = MakeRazorCustomLayoutString(info, options);

                UpdateBlazorForAssets(info, options);
                if (info.HasScriptAsset() == true)
                    info.RazorCustomLayoutStrings = AdjustRazorCustomLayoutForScriptAssets(info, options);
            }
            else
            {
                info.BlazorStrings = MakeBlazer(info, options);
                info.HostAssetsStrings = MakeHostAssets(info, options);
                info.HostRenderMode = MakeHostRenderMode(info, options);
                info.NewHostStrings = MakeNewHostStrings(info);
                info.NavMenuStrings = MakeNavMenuStrings(options);
                info.NewNavMenuStrings = MakeNewNavMenuStrings(info, options);

                UpdateBlazorForAssets(info, options);
                if (info.HasScriptAsset() == true)
                    info.NewHostStrings = AdjustForScriptAssets(info, options);
            }



            if (options.HasPreview() == true)
            {
                options.PreviewFile = options.BlazorProjectPath + options.PageName + "_preview.txt";
                info.PreviewStrings = MakePreview(options, info);
            }
            info.UpdateGenerateStatus();
            return info;
        }

        public static bool IsBootstrapHtml(Options options,
                                 string strPath,
                                 string strHtml)
        {
            bool result = false;

            string stringHtml = strPath + strHtml;
            List<string> stringResult;

            try
            {
                stringResult = LoadStrings(stringHtml, options);
                foreach (string s in stringResult)
                {
                    if (s.Contains("stylesheet") == true)
                    {
                        if (s.Contains("bootstrap") == true)
                            result = true;
                    }
                }
            }
            catch (IOException e)
            {
                if (options.IsTraceOn(Trace_Options.TraceExceptions))
                    Trace.TraceError("Exception loading HtmlFile: " + stringHtml, e);
            }

            return result;
        }

        // private methods

        public static List<string> MakePreview(Options options,
                                          DataInfo info)
        {
            List<string> stringsResult = new List<string>();
            string str;

            if (info.IsBootstrapHtml == true)
                str = "Bootstrap HTML Project Path = " + options.BootstrapProjectPath;
            else
                str = "HTML (other)   Project Path = " + options.BootstrapProjectPath;
            stringsResult.Add(str);
            str = "Project Path                = " + options.BlazorProjectPath;
            stringsResult.Add(str);
            stringsResult.Add("");


            str = "SelectedModifyOptions     = " + options.SelectedModifyOptions.ToString();
            stringsResult.Add(str);
            str = "SelectedAssetOptions      = " + options.SelectedAssetOptions.ToString();
            stringsResult.Add(str);
            str = "SelectedCSSptions         = " + options.SelectedCSSOptions.ToString();
            stringsResult.Add(str);
            str = "SelectedHostOptions       = " + options.SelectedHostOptions.ToString();
            stringsResult.Add(str);
            str = "SelectedScriptOptions     = " + options.SelectedScriptOptions.ToString();
            stringsResult.Add(str);
            str = "SelectedNavMenuOptions    = " + options.SelectedNavMenuOptions.ToString();
            stringsResult.Add(str);
            str = "SelectedRazorOptions      = " + options.SelectedRazorOptions.ToString();
            stringsResult.Add(str);
            str = "SelectedTraceOptions      = " + options.SelectedTraceOptions.ToString();
            stringsResult.Add(str);
            str = "SelectedRenderModeOptions = " + options.SelectedRenderModeOptions.ToString();
            stringsResult.Add(str);
            stringsResult.Add("");



            if (info.ProjectType == Type_Options.BlazorServer)
                str = "Preview for Blazor Page " + options.PageName + " in file " + GetRazorFile(options, info);
            else if (info.ProjectType == Type_Options.BlazorWebassembly)
                str = "Preview for Blazor Webassembly Page " + options.PageName + " in file " + GetRazorFile(options, info);
            else if (info.ProjectType == Type_Options.BlazorWebassemblyPWA)
                str = "Preview for Blazor Webassembly PWA Page " + options.PageName + " in file " + GetRazorFile(options, info);
            else if (info.ProjectType == Type_Options.ASPNetRazor)
                str = "Preview for ASP.Net Razor Page " + options.PageName + " in file " + GetRazorFile(options, info);
            else
                str = "Unknown Type Preview";

            stringsResult.Add(str);
            stringsResult.Add("");

            // first output blazor page
            foreach (string s in info.BlazorStrings)
                stringsResult.Add(s);
            stringsResult.Add("");

            if (info.RazorCodeBehindStrings.Count > 0)
            {
                str = "Preview of Razor Code behind strings in " + options.BlazorProjectPath + "Pages//" + options.PageName + ".cshtml.cs";
                stringsResult.Add(str);
                stringsResult.Add("");

                // output razor code behind page
                foreach (string s in info.RazorCodeBehindStrings)
                    stringsResult.Add(s);
                stringsResult.Add("");
            }

            if (info.HostAssetsStrings.Count > 0)
            {
                str = "Preview for changes in " + GetHostFile(options, info);
                stringsResult.Add(str);
                stringsResult.Add("");

                foreach (string s in info.HostAssetsStrings)
                    stringsResult.Add(s);
                stringsResult.Add("");
            }

            if (info.HostRenderMode.Length > 0)
            {
                stringsResult.Add("");
                str = "    " + info.HostRenderMode;
                stringsResult.Add(str);
                stringsResult.Add("");
            }

            if (info.NewHostStrings.Count > 0)
            {
                str = "Preview for Host Changes in " + GetHostFile(options, info);
                stringsResult.Add(str);
                stringsResult.Add("");

                foreach (string s in info.NewHostStrings)
                    stringsResult.Add(s);
                stringsResult.Add("");
            }

            if ((info.ProjectType == Type_Options.ASPNetRazor) &&
                  (info.RazorCustomLayoutStrings.Count > 0))
            {
                str = "Preview for Razor Custom Layout Changes in " + GetRazorCustomLayoutFile(options, info);
                stringsResult.Add(str);
                stringsResult.Add("");

                foreach (string s in info.RazorCustomLayoutStrings)
                    stringsResult.Add(s);
                stringsResult.Add("");
            }

            if (options.SelectedNavMenuOptions != NavMenu_Options.None)
            {
                if (info.NewNavMenuStrings.Count > 0)
                {
                    if (info.ProjectType == Type_Options.ASPNetRazor)
                        str = "Preview for Razor NavMenu Changes in" + GetNavMenuFile(options, info);
                    else
                        str = "Preview for NavMenu.Razor Changes in" + GetNavMenuFile(options, info);
                    stringsResult.Add(str);
                    stringsResult.Add("");

                    foreach (string s in info.NewNavMenuStrings)
                        stringsResult.Add(s);
                    stringsResult.Add("");
                }
            }

            stringsResult.Add("Scripts assets");
            stringsResult.Add("  Total Multi-Line Scripts detected: " + info.MultiLineScriptCount.ToString());
            stringsResult.Add("  Total External   Scripts detected: " + info.ScriptAssets.Count.ToString());
            foreach (string s in info.ScriptAssets)
                stringsResult.Add("       " + s);
            stringsResult.Add("");



            stringsResult.Add("Asset List");
            stringsResult.Add("");
            foreach (Asset ThisAsset in info.Assets)
            {
                str = "   Index=" + ThisAsset.Index.ToString() + "   " + ThisAsset.AssetType.ToString();
                stringsResult.Add(str);
                str = "   HtmlAsset=[" + ThisAsset.HtmlAsset + "]   RawAssset=[" + ThisAsset.RawAsset + "]";
                stringsResult.Add(str);
            }
            stringsResult.Add("");

            if ((options.SelectedHostOptions != Host_Options.None) &&
                (options.HostAssetsDirectory.Length > 0))
            {
                string sourceDirectory = GetSourceDirectory(options);
                string destinationDirectory = GetDestinationDirectory(options);

                stringsResult.Add("Will copy Assets Source=" + sourceDirectory);
                stringsResult.Add("                 Dest  =" + destinationDirectory);
                stringsResult.Add("");
            }

            return stringsResult;
        }

        public static Type_Options GetProjectType(Options options)
        {
            Type_Options result = Type_Options.Unknown;

            try
            {
                string strFile = options.BlazorProjectPath + options.HostHtml;
                if (CommonIO.Exists(strFile, options) == true)
                    result = Type_Options.BlazorServer;
                else
                {
                    strFile = options.BlazorProjectPath + options.WasmHostHtml;
                    if (CommonIO.Exists(strFile, options) == true)
                    {
                        strFile = options.BlazorProjectPath + options.PWAFile;
                        if (CommonIO.Exists(strFile, options) == true)
                            result = Type_Options.BlazorWebassemblyPWA;
                        else
                            result = Type_Options.BlazorWebassembly;
                    }
                    else
                    {
                        strFile = options.BlazorProjectPath + options.RazorHostHtml;
                        if (CommonIO.Exists(strFile, options) == true)
                            result = Type_Options.ASPNetRazor;
                    }
                }
            }
            catch (IOException e)
            {
                if (options.IsTraceOn(Trace_Options.TraceExceptions))
                    Trace.TraceError("Exception get Project Type", e);
            }

            return result;
        }

        public static List<string> MakeHtml(Options options)
        {
            List<string> stringResult = new List<string>();

            string htmlFile = GetBootstrapHtml(options);

            try
            {
                if (options.IsTraceOn(Trace_Options.TraceInfo))
                    Trace.TraceInformation("Html file = [" + htmlFile + "]");
                stringResult = LoadStrings(htmlFile, options);
            }
            catch (IOException e)
            {
                if (options.IsTraceOn(Trace_Options.TraceExceptions))
                    Trace.TraceError("IO Exception loading HtmlFile: " + htmlFile, e);
            }

            return stringResult;
        }

        public static List<string> MakeHostHtml(DataInfo info,
                                           Options options)
        {
            List<string> stringResult = new List<string>();
            string htmlFile = GetHostFile(options, info);

            if (CommonIO.Exists(htmlFile, options) == true)
            {
                if (options.IsTraceOn(Trace_Options.TraceInfo))
                    Trace.TraceInformation("Host file = [" + htmlFile + "]");

                stringResult = LoadStrings(htmlFile, options);
            }
            else
            {
                if (options.IsTraceOn(Trace_Options.TraceWarning))
                    Trace.TraceWarning("No Host file, likely webassembly or Razor");
            }

            return stringResult;
        }

        public static List<Asset> MakeScriptAssets(DataInfo info,
                                            Options options)
        {
            List<string> stringFile = info.HtmlStrings;
            List<string> stringWork;
            List<Asset> assetList = new List<Asset>();
            bool scriptToHost = false;
            bool inside = false;
            Asset asset;

            if ((options.SelectedScriptOptions == Script_Options.SelectedScriptToHost))
                scriptToHost = true;
            else if (options.SelectedScriptOptions == Script_Options.All)
                scriptToHost = true;

            if (scriptToHost == true)
            {
                if (options.IsTraceOn(Trace_Options.TraceInfo))
                    Trace.TraceInformation("Script to host, remove embedded scripts");

                int count = 0;
                stringWork = new List<string>();
                bool insideScript = false;
                foreach (string s in stringFile)
                {
                    if ((s.Contains("script>") == true) &&
                         (s.Contains(@"<script") == false))
                    {
                        count++;
                        insideScript = false;
                    }
                    else if ((s.Contains(@"<script") == true) &&
                              (s.Contains("script>") == false))
                    {
                        insideScript = true;
                    }
                    else if (insideScript == false)
                    {
                        if (options.IsTraceOn(Trace_Options.TraceInfo))
                            Trace.TraceInformation(s);
                        stringWork.Add(s);
                    }
                }
                info.MultiLineScriptCount = count;

                if (options.IsTraceOn(Trace_Options.TraceInfo))
                    Trace.TraceInformation("Start AssetList");

                foreach (string s in stringWork)
                {
                    if (inside == true)
                    {
                        if (options.IsTraceOn(Trace_Options.TraceInfo))
                            Trace.TraceInformation(s);
                        if ((s.Contains("<script")) && (s.Contains("script>")))
                            info.ScriptAssets.Add(s);
                        if (s.Contains(@"</body>") == true)
                            inside = false;
                        else if ((s.Contains(@"<script") == true) &&
                             (s.Contains("script>") == true) &&
                             (scriptToHost == true))
                        {
                            asset = AddScriptToHostAsset(options, s);
                            if (asset != null)
                                assetList.Add(asset);
                        }
                    }
                    else if (s.Contains("<body") == true)
                        inside = true;
                }
            }
            if (options.IsTraceOn(Trace_Options.TraceInfo))
                Trace.TraceInformation("End AssetList");

            return assetList;
        }

        public static List<string> MakeBlazer(DataInfo info,
                                        Options options)
        {
            List<string> stringResult = new List<string>();
            List<string> stringFile = info.HtmlStrings;
            List<string> stringWork;

            bool ignoreScript = false;
            bool stripScript = false;
            bool inside;

            if ((options.SelectedScriptOptions == Script_Options.IgnoreScript))
                ignoreScript = true;
            else if ((options.SelectedScriptOptions == Script_Options.StripMultiLine))
                stripScript = true;
            else if ((options.SelectedScriptOptions == Script_Options.All))
                stripScript = true;

            string str = "@page " + Support.Quote("/" + options.PageName);
            stringResult.Add(str);
            stringResult.Add("");

            string stringDivStyle = "";
            if (options.SelectedRazorOptions == Page_Options.ConvertBodyStyleBackground)
            {
                stringDivStyle = ConvertBodyStyleToDivStyle(stringFile);
                if (stringDivStyle.Length > 0)
                    stringResult.Add(stringDivStyle);
            }
            if (options.IsTraceOn(Trace_Options.TraceInfo))
                Trace.TraceInformation("first part of scripts");

            if (stripScript == true)
            {
                stringWork = new List<string>();
                bool insideScript = false;
                foreach (string s in stringFile)
                {
                    if ((s.Contains("script>") == true) &&
                         (s.Contains(@"<script") == false))
                    {
                        insideScript = false;
                    }
                    else if ((s.Contains(@"<script") == true) &&
                              (s.Contains("script>") == false))
                    {
                        insideScript = true;
                    }
                    else if (insideScript == false)
                    {
                        if (options.IsTraceOn(Trace_Options.TraceInfo))
                            Trace.TraceInformation(s);
                        stringWork.Add(s);
                    }
                }

            }
            else
                stringWork = stringFile;

            if (options.IsTraceOn(Trace_Options.TraceInfo))
                Trace.TraceInformation("second part of scripts");

            inside = false;
            foreach (string s in stringWork)
            {
                if (inside == true)
                {
                    if (s.Contains(@"</body>") == true)
                        inside = false;
                    else if ((ignoreScript == true) ||
                          (s.Contains("script>") == false))
                    {
                        if (options.IsTraceOn(Trace_Options.TraceInfo))
                            Trace.TraceInformation(s);
                        stringResult.Add(s);
                    }
                }
                else if (s.Contains("<body") == true)
                    inside = true;
            }

            if (stringDivStyle.Length > 0)
                stringResult.Add("</div>");

            stringResult.Add("");
            stringResult.Add("@code {");
            stringResult.Add("");
            stringResult.Add("}");

            return stringResult;
        }

        public static Asset AddScriptToHostAsset(Options options,
                                            string str)
        {
            Asset CurrentAsset = null;
            if (str.Contains("https") == true)
            {
                if ((str.Contains("aos.js") == true) &&
                     (options.IsCSSAOSOption() == true))
                {
                    CurrentAsset = new Asset(str, AssetType_Options.ScriptHttpsAsset);
                }
            }
            else if ((str.Contains(@"jquery.min.js") == true) ||
                      (str.Contains(@"script.min.js") == true))
            {
                CurrentAsset = new Asset(str, AssetType_Options.ScriptAsset);
            }

            return CurrentAsset;
        }

        public static string ConvertBodyStyleToDivStyle(List<string> stringFile)
        {
            string result = "";
            string strFound = "";

            foreach (string s in stringFile)
            {
                if ((s.Contains("<body") == true) &&
                     (s.Contains("style") == true))
                {
                    strFound = s;
                }
            }

            if (strFound.Length > 0)
                result = strFound.Replace("body", "div class=" + Support.Quote("container-fluid"));

            return result;
        }

        public static List<string> MakeAssets(DataInfo info,
                                        Options options)
        {
            List<string> stringsResult = new List<string>();

            foreach (string s in info.HtmlStrings)
            {
                if (s.Contains("stylesheet") == true)
                {
                    if (s.Contains(@"assets/css") == true)
                    {
                        if (options.IsTraceOn(Trace_Options.TraceInfo))
                            Trace.TraceInformation(s);
                        stringsResult.Add(s);
                    }
                    else if (s.Contains(@"assets/fonts") == true)
                    {
                        if (options.IsTraceOn(Trace_Options.TraceInfo))
                            Trace.TraceInformation(s);
                        stringsResult.Add(s);
                    }
                    else if (s.Contains(@"https://fonts") == true)
                    {
                        if (options.IsCSSFontOption() == true)
                        {
                            if (options.IsTraceOn(Trace_Options.TraceInfo))
                                Trace.TraceInformation(s);
                            stringsResult.Add(s);
                        }
                    }
                    else if ((s.Contains("bootstrap") == true) &&
                             (s.Contains("https") == false))
                    {
                        if (options.IsCSSBootstrapOption() == true)
                        {
                            if (options.IsTraceOn(Trace_Options.TraceInfo))
                                Trace.TraceInformation(s);
                            stringsResult.Add(s);
                        }
                    }
                    else if ((s.Contains("https") == true) &&
                             (s.Contains("aos.css") == true))
                    {
                        if (options.IsCSSAOSOption() == true)
                        {
                            if (options.IsTraceOn(Trace_Options.TraceInfo))
                                Trace.TraceInformation(s);
                            stringsResult.Add(s);
                        }
                    }
                }
            }
            return stringsResult;
        }

        public static void UpdateBlazorForAssets(DataInfo info,
                                            Options options)
        {
            if (options.IsUseCustomAssets() == true)
            {
                List<string> newBlazor = new List<string>();
                foreach (string s in info.BlazorStrings)
                {
                    string str = s;
                    if (s.Contains("assets") == true)
                        str = str.Replace("assets", options.IndexPageName + @"/assets");
                    newBlazor.Add(str);
                }
                info.BlazorStrings = newBlazor;
            }
        }

        public static List<string> MakeHostAssets(DataInfo info,
                                            Options options)
        {
            List<string> stringsResult = new List<string>();
            string str;

            if (options.IsUseSharedAssets() == true)
                options.HostAssetsDirectory = @"wwwroot\assets";
            else if (options.IsUseCustomAssets() == true)
                options.HostAssetsDirectory = @"wwwroot\" + options.IndexPageName + @"\assets";

            if ((info.ProjectType == Type_Options.BlazorServer) &&
                (options.IsUseSameIndexAssets() == false))
            {
                if (info.AssetsStrings.Count > 0)
                {
                    str = "    @if (Request.Path.Value == " + Support.Quote("/" + options.PageName) + ")";
                    stringsResult.Add(str);
                    stringsResult.Add("    {");
                    foreach (string s in info.AssetsStrings)
                    {
                        str = "    " + s;
                        if (options.IsUseCustomAssets() == true)
                            str = str.Replace("assets", options.IndexPageName + "/assets");
                        stringsResult.Add(str);
                    }

                    stringsResult.Add("    }");
                }
            }
            else if (options.IsUseSharedAssets() == true)
            {
                if (info.AssetsStrings.Count > 0)
                {
                    foreach (string s in info.AssetsStrings)
                    {
                        stringsResult.Add(s);
                    }
                }
            }


            return stringsResult;
        }

        public static List<string> MakeNavMenuStrings(Options options)
        {
            List<string> stringsResult = new List<string>();
            string str;

            str = "        <li class=" + Support.Quote("nav-item px-3") + ">";
            stringsResult.Add(str);
            str = "           <NavLink class=" + Support.Quote("nav-link") + " href=" + Support.Quote(options.PageName) + ">";
            stringsResult.Add(str);
            str = "                <span class=" + Support.Quote("oi oi-target") + " aria-hidden=" + Support.Quote("true") + "></span> " + options.PageName;
            stringsResult.Add(str);
            str = "           </NavLink>";
            stringsResult.Add(str);
            str = "        </li>";
            stringsResult.Add(str);
            return stringsResult;
        }

        public static string MakeHostRenderMode(DataInfo info,
                                          Options options)
        {
            string result = "";

            foreach (string s in info.HostStrings)
            {
                if (s.Contains("render-mode") == true)
                    result = s;
            }

            if ((options.IsUseStaticAssets() == true) &&
                (info.ProjectType == Type_Options.BlazorServer) &&
                (options.HostAssetsDirectory.Length > 0))
            {
                string str = options.GetRenderModeString();
                result = result.Replace("ServerPrerendered", str);
            }
            return result;
        }

        public static List<string> MakeNewHostStrings(DataInfo info)
        {
            List<string> stringsResult = new List<string>();

            foreach (string s in info.HostStrings)
            {
                string str = s;
                if (s.Contains(@"</head>") == true)
                {
                    foreach (string hostStr in info.HostAssetsStrings)
                        stringsResult.Add(hostStr);
                }

                if (s.Contains("render-mode") == true)
                    str = info.HostRenderMode;
                stringsResult.Add(str);
            }

            return stringsResult;
        }

        public static List<string> MakeNewNavMenuStrings(DataInfo info,
                                                   Options options)
        {
            string fileName = GetNavMenuFile(options, info);

            List<string> strings;
            if (info.ProjectType == Type_Options.ASPNetRazor)
                strings = info.NewHostStrings;
            else
                strings = LoadStrings(fileName, options);

            List<string> stringsResult = MakeNavStrings(info, options, strings);
            return stringsResult;
        }

        public static List<string> MakeNavStrings(DataInfo info,
                                            Options options,
                                            List<string> strings)
        {
            List<string> stringsResult = new List<string>();
            bool addStrings = true;
            string strCheck = Support.Quote(options.PageName);

            foreach (string s in strings)
            {
                if (s.Contains("nav-link") == true)
                {
                    if (s.Contains(strCheck) == true)
                        addStrings = false;
                }
            }

            if (addStrings == true)
            {
                foreach (string s in strings)
                {
                    if (s.Contains(@"</ul>") == true)
                    {
                        foreach (string str in info.NavMenuStrings)
                            stringsResult.Add(str);
                    }

                    stringsResult.Add(s);
                }
            }

            return stringsResult;
        }


        public static List<Asset> CreateMoveAssets(DataInfo info,
                                                   Options options)
        {
            List<Asset> assets = info.Assets;

            if (options.IsUseMoveAssets() == true)
            {
                foreach (string s in info.HtmlStrings)
                {
                    Asset theAsset = MakeImageAsset(s);
                    if (theAsset == null)
                        theAsset = MakeHostAsset(options, s);
                    if (theAsset != null)
                    {
                        theAsset.Index = info.HtmlStrings.IndexOf(s);
                        assets.Add(theAsset);
                    }
                }
            }
            return assets;
        }

        public static List<string> AdjustHtmlForMoveAssets(DataInfo info,
                                                    Options options)
        {
            List<string> stringsResult = new List<string>();

            if (options.IsUseMoveAssets() == true)
            {
                foreach (string s in info.HtmlStrings)
                {
                    string str = s;
                    string strReplace;
                    Asset theAsset = info.GetAssetFromString(s);
                    if (theAsset != null)
                    {
                        if (theAsset.IsScriptAsset() == false)
                        {
                            strReplace = @"assets\img\" + theAsset.RawAsset;
                            str = s.Replace(theAsset.HtmlAsset, strReplace);
                            stringsResult.Add(str);
                        }
                    }
                    else
                        stringsResult.Add(str);
                }
            }

            return stringsResult;
        }

        public static List<string> AdjustForScriptAssets(DataInfo info,
                                                         Options options)
        {
            string str;
            List<string> stringsResult = new List<string>();

            foreach (string s in info.NewHostStrings)
            {
                if (s.Contains(@"</body>") == true)
                {
                    if (info.HasScriptAsset() == true)
                    {

                        if (info.ProjectType == Type_Options.BlazorServer)
                        {
                            str = "    @if(Request.Path.Value == " + Support.Quote(@"/" + options.PageName) + ")";
                            stringsResult.Add(str);
                            if (options.IsTraceOn(Trace_Options.TraceInfo))
                                Trace.TraceInformation(str);
                            str = "    {";
                            stringsResult.Add(str);
                            if (options.IsTraceOn(Trace_Options.TraceInfo))
                                Trace.TraceInformation(str);
                            foreach (Asset asset in info.Assets)
                            {
                                if (asset.IsScriptAsset() == true)
                                {
                                    str = asset.HtmlAsset;
                                    if (str.Contains("assets") == true)
                                        str = str.Replace("assets", options.PageName + @"/assets");
                                    str = "        " + str;
                                    stringsResult.Add(str);
                                    if (options.IsTraceOn(Trace_Options.TraceInfo))
                                        Trace.TraceInformation(str);
                                }
                            }

                            str = "    }";
                            stringsResult.Add(str);
                            if (options.IsTraceOn(Trace_Options.TraceInfo))
                                Trace.TraceInformation(str);
                        }
                        else if (options.IsUseSharedAssets() == true)
                        {
                            foreach (Asset asset in info.Assets)
                            {
                                if (asset.IsScriptAsset() == true)
                                {
                                    str = asset.HtmlAsset;
                                    stringsResult.Add(str);
                                    if (options.IsTraceOn(Trace_Options.TraceInfo))
                                        Trace.TraceInformation(str);
                                }
                            }
                        }
                    }
                }
                stringsResult.Add(s);
                if (options.IsTraceOn(Trace_Options.TraceInfo))
                    Trace.TraceInformation(s);
            }

            return stringsResult;
        }
        #region Razor support

        // Razor support

        public static string GetRazorNameSpace(Options options)
        {
            string nameSpace = "";
            string fileName = options.GetRazorNameSpaceSource();
            string search = "@namespace";
            List<string> stringsFile = LoadStrings(fileName, options);

            foreach (string s in stringsFile)
            {
                if (s.Contains(search) == true)
                {
                    int searchIndex = s.IndexOf(search) + search.Length;
                    nameSpace = s.Substring(searchIndex, s.Length - searchIndex);
                }
            }

            return nameSpace;
        }

        public static List<string> MakeRazor(DataInfo info,
                                       Options options)
        {
            List<string> stringsResult = new List<string>();
            List<string> stringsFile = info.HtmlStrings;

            string str = "@page";
            stringsResult.Add(str);
            str = "@model " + options.PageName + "Model";
            stringsResult.Add(str);
            str = "@{";
            stringsResult.Add(str);
            str = "    ViewData[" + Support.Quote("Title") + "] = " + Support.Quote(options.PageName) + ";";
            stringsResult.Add(str);
            if (options.MakeCustomRazorLayout == true)
            {
                str = "    Layout=" + Support.Quote(options.PageName + "_Layout.cshtml") + ";";
                stringsResult.Add(str);
                stringsResult.Add("");
            }
            str = "}";
            stringsResult.Add(str);
            stringsResult.Add("");
            str = "<h1>@ViewData[" + Support.Quote("Title") + "]</h1>";
            stringsResult.Add(str);
            stringsResult.Add("");

            string stringDivStyle = "";
            if (options.SelectedRazorOptions == Page_Options.ConvertBodyStyleBackground)
            {
                stringDivStyle = ConvertBodyStyleToDivStyle(stringsFile);
                if (stringDivStyle.Length > 0)
                    stringsResult.Add(stringDivStyle);
            }

            bool inside = false;
            foreach (string s in stringsFile)
            {
                if (inside == true)
                {
                    if (s.Contains(@"</body>") == true)
                        inside = false;
                    else if ((options.SelectedScriptOptions != Script_Options.IgnoreScript) ||
                          (s.Contains("script>") == false))
                    {
                        str = IsRazorScriptAllowed(s);
                        if (string.IsNullOrEmpty(str) == false)
                            stringsResult.Add(str);
                    }
                }
                else if (s.Contains("<body") == true)
                    inside = true;
            }

            if (stringDivStyle.Length > 0)
                stringsResult.Add("</div>");

            stringsResult.Add("");

            return stringsResult;
        }

        public static string IsRazorScriptAllowed(string str)
        {
            string result = "";
            if (str.Contains("<script") == false)
                result = str;

            return result;
        }

        public static List<string> MakeRazorCodeBehind(Options options)
        {
            List<string> stringsResult = new List<string>();

            string str = "using System;";
            stringsResult.Add(str);
            str = "using System.Collections.Generic;";
            stringsResult.Add(str);
            str = "using System.Linq;";
            stringsResult.Add(str);
            str = "using System.Threading.Tasks;";
            stringsResult.Add(str);
            str = "using Microsoft.AspNetCore.Mvc;";
            stringsResult.Add(str);
            str = "using Microsoft.AspNetCore.Mvc.RazorPages;";
            stringsResult.Add(str);
            str = "using Microsoft.Extensions.Logging;";
            stringsResult.Add(str);
            stringsResult.Add("");
            str = "namespace " + options.RazorNameSpace;
            stringsResult.Add(str);
            stringsResult.Add("{");
            str = "   public class " + options.PageName + "Model : PageModel";
            stringsResult.Add(str);
            str = "   {";
            stringsResult.Add(str);
            str = "        private readonly ILogger<" + options.PageName + "Model> _logger;";
            stringsResult.Add(str);
            stringsResult.Add("");
            str = "        public " + options.PageName + "Model(ILogger<" + options.PageName + "Model> logger)";
            stringsResult.Add(str);
            str = "        {";
            stringsResult.Add(str);
            str = "            _logger = logger;";
            stringsResult.Add(str);
            str = "        }";
            stringsResult.Add(str);
            stringsResult.Add("");
            str = "        public void OnGet()";
            stringsResult.Add(str);
            str = "        {";
            stringsResult.Add(str);
            stringsResult.Add("");
            str = "        }";
            stringsResult.Add(str);
            str = "    }";
            stringsResult.Add(str);
            stringsResult.Add("}");
            stringsResult.Add("");
            return stringsResult;
        }

        public static List<string> MakeRazorNavMenuStrings(Options options)
        {
            List<string> stringsResult = new List<string>();
            string str;
            string page = options.PageName;

            str = "                <li class=" + Support.Quote("nav-item") + ">";
            stringsResult.Add(str);
            str = "                    <a class=" + Support.Quote("nav-link text-dark") + "  asp-area=" + Support.Quote("") + " asp-page=" + Support.Quote("/" + page) + ">" + page + "</a>";
            stringsResult.Add(str);
            str = "                 </li>";
            stringsResult.Add(str);
            return stringsResult;
        }

        public static List<string> MakeRazorCustomLayoutString(DataInfo info,
                                                         Options options)
        {
            List<string> stringsResult = new List<string>();
            List<string> strings = info.NewHostStrings;

            if ((options.MakeCustomRazorLayout == true) &&
                 (info.RazorAssetsStrings.Count > 0))
            {
                foreach (string s in strings)
                {
                    if (s.Contains(@"</head>") == true)
                    {
                        foreach (string str in info.RazorAssetsStrings)
                            stringsResult.Add(str);
                    }
                    stringsResult.Add(s);
                }

                // must add new Nav strings to custom host
                if ((options.MakeCustomRazorLayout == true) &&
                    (info.NavMenuStrings.Count > 0))
                {
                    strings = stringsResult;
                    stringsResult = MakeNavStrings(info, options, strings);
                }
            }
            return stringsResult;
        }

        public static List<string> AdjustRazorCustomLayoutForScriptAssets(DataInfo info,
                                                                          Options options)
        {
            string str;
            List<string> stringsResult = new List<string>();

            foreach (string s in info.RazorCustomLayoutStrings)
            {
                if (s.Contains(@"</body>") == true)
                {
                    if (info.HasScriptAsset() == true)
                    {
                        if (options.IsUseSharedAssets() == true)
                        {
                            foreach (Asset asset in info.Assets)
                            {
                                if (asset.IsScriptAsset() == true)
                                {
                                    str = asset.HtmlAsset;
                                    stringsResult.Add(str);
                                }
                            }
                        }
                        else
                        {
                            foreach (Asset asset in info.Assets)
                            {
                                if (asset.IsScriptAsset() == true)
                                {
                                    str = asset.HtmlAsset;
                                    if (str.Contains("assets") == true)
                                        str = str.Replace("assets", options.PageName + @"/assets");
                                    stringsResult.Add(str);
                                }
                            }
                        }
                    }
                }
                stringsResult.Add(s);
            }

            return stringsResult;
        }

        public static List<string> MakeRazorHostAssets(DataInfo info,
                                                Options options)
        {
            List<string> stringsResult = new List<string>();
            string str;

            if (info.ProjectType == Type_Options.ASPNetRazor)
            {
                if (info.AssetsStrings.Count > 0)
                {
                    foreach (string s in info.AssetsStrings)
                    {
                        str = "    " + s;
                        if (options.IsUseCustomAssets() == true)
                            str = str.Replace("assets", options.IndexPageName + "/assets");
                        stringsResult.Add(str);
                    }
                }
            }

            return stringsResult;
        }
        #endregion

        #region Support Methods
        // support methods


        public static List<string> LoadStrings(string fileName,
                                         Options options)
        {
            List<string> strings = new List<string>();

            if (CommonIO.Exists(fileName, options) == true)
            {
                string[] strs = CommonIO.ReadAllLines(fileName, options);
                foreach (string s in strs)
                    strings.Add(s);
            }

            return strings;
        }
        #endregion
    }
}