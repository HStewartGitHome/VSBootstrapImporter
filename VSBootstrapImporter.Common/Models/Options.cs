using System.Diagnostics;

namespace VSBootstrapImporter.Common.Models
{
    public class Options
    {
        public Options()
        {
            BootstrapProjectPath = "";
            BootstrapHtml = "index.html";
            BootstrapIndexHtml = "index.html";
            BootstrapAssets = "assets";
            HostHtml = "Pages\\_Host.cshtml";
            WasmHostHtml = "wwwroot\\index.html";
            RazorHostHtml = "Pages\\Shared\\_Layout.cshtml";
            NavMenuFile = "Shared\\NavMenu.Razor";
            PWAFile = "wwwroot\\manifest.json";
            BlazorProjectPath = "";
            PageName = "";
            IndexPageName = "";
            PreviewFile = "";
            AllowHtmlEditing = false;
            BootStrapOverrideHtml = "";
            RazorNameSpace = "";
            RazorNameSpaceSource = "Pages\\_Viewimports.cshtml";
            MakeCustomRazorLayout = true;
            AllowNonBootstrap = false;
            HostAssetsDirectory = "";
            SelectedModifyOptions = Modify_Options.Default;
            SelectedAssetOptions = Asset_Options.Default;
            SelectedCSSOptions = CSS_Options.Default;
            SelectedHostOptions = Host_Options.Default;
            SelectedScriptOptions = Script_Options.Default;
            SelectedNavMenuOptions = NavMenu_Options.Default;
            SelectedRazorOptions = Page_Options.Default;
            SelectedTraceOptions = Trace_Options.Default;
            SelectedRenderModeOptions = RenderMode_Options.Default;
        }

        public bool IsReady()
        {
            if ((BootstrapProjectPath.Length > 0) && (PageName.Length > 0) && (BlazorProjectPath.Length > 0))
                return true;
            else
                return false;
        }

        public bool HasPreview()
        {
            bool result = false;
            switch (SelectedModifyOptions)
            {
                case Modify_Options.ModifyProject:
                case Modify_Options.PreviewOnly:
                    result = true;
                    break;
            }

            return result;
        }

        public bool CanOutput()
        {
            bool result = false;
            switch (SelectedModifyOptions)
            {
                case Modify_Options.ModifyProject:
                case Modify_Options.ModifyNoPreview:
                    result = true;
                    break;
            }

            return result;
        }

        public static string GetProjectTypeString(Type_Options projectType)
        {
            string str;
            switch (projectType)
            {
                case Type_Options.BlazorServer:
                    str = "Blazor Server";
                    break;

                case Type_Options.BlazorWebassembly:
                    str = "Blazor Webassembly";
                    break;

                case Type_Options.BlazorWebassemblyPWA:
                    str = "Blazor Webassembly PWA";
                    break;

                case Type_Options.ASPNetRazor:
                    str = "ASP.Net Razor";
                    break;

                default:
                    str = "Unknown please select";
                    break;
            }
            return str;
        }

        public string GetRenderModeString()
        {
            string result;

            switch (SelectedRenderModeOptions)
            {
                case RenderMode_Options.Server:
                    result = "Server";
                    break;

                case RenderMode_Options.Static:
                    result = "Static";
                    break;

                case RenderMode_Options.ServerPrerendered:
                default:
                    result = "ServerPrerendered";
                    break;
            }

            return result;
        }

        public string GetImageAssetString()
        {
            string result;

            switch (SelectedAssetOptions)
            {
                case Asset_Options.Static:
                    result = "Assets directory is copy from html";
                    break;

                case Asset_Options.StaticMove:
                    result = "Assets directory are specific copy";
                    break;

                case Asset_Options.StaticSameIndexMove:
                case Asset_Options.StaticSameIndexPage:
                    result = "Assets not copy, use primary index";
                    break;

                case Asset_Options.SharedPages:
                    result = "Assets are shared with primary page";
                    break;

                case Asset_Options.SharedPagesMove:
                    result = "Assets are moved shared with primary page";
                    break;

                default:
                    result = "None";
                    break;
            }
            return result;
        }

        public string GetRazorNameSpaceSource()
        {
            string result = BlazorProjectPath + RazorNameSpaceSource;
            return result;
        }

        public bool IsTraceOn(Trace_Options option)
        {
            bool result = false;
            switch (option)
            {
                case Trace_Options.TraceAll:
                    if (SelectedTraceOptions != Trace_Options.TraceNone)
                        result = true;
                    break;

                case Trace_Options.TraceExceptions:
                case Trace_Options.TraceError:
                case Trace_Options.TraceWarning:
                case Trace_Options.TraceInfo:
                    if (((int)SelectedTraceOptions & (int)option) != 0)
                        result = true;
                    break;

                case Trace_Options.TraceNone:
                    result = false;
                    break;
                default:
                    Trace.TraceWarning("Failed with value of ", (int)option);
                    result = false;
                    break;
            }



            return result;
        }

        public bool IsUseCustomAssets()
        {
            bool result;
            switch (SelectedAssetOptions)
            {
                case Asset_Options.Static:
                case Asset_Options.StaticMove:
                case Asset_Options.StaticSameIndexPage:
                case Asset_Options.StaticSameIndexMove:
                    result = true;
                    break;

                default:
                    result = false;
                    break;
            }
            return result;
        }

        public bool IsUseSharedAssets()
        {
            bool result;
            switch (SelectedAssetOptions)
            {
                case Asset_Options.SharedPages:
                case Asset_Options.SharedPagesMove:
                    result = true;
                    break;

                default:
                    result = false;
                    break;
            }
            return result;
        }

        public bool IsUseMoveAssets()
        {
            bool result;
            switch (SelectedAssetOptions)
            {
                case Asset_Options.StaticSameIndexMove:
                case Asset_Options.StaticMove:
                case Asset_Options.SharedPagesMove:
                    result = true;
                    break;

                default:
                    result = false;
                    break;
            }
            return result;
        }

        public bool IsUseSameIndexAssets()
        {
            bool result;
            switch (SelectedAssetOptions)
            {
                case Asset_Options.StaticSameIndexPage:
                case Asset_Options.StaticSameIndexMove:
                    result = true;
                    break;

                default:
                    result = false;
                    break;
            }
            return result;
        }

        public bool IsUseStaticAssets()
        {
            bool result;
            switch (SelectedAssetOptions)
            {
                case Asset_Options.Static:
                case Asset_Options.StaticMove:
                    result = true;
                    break;

                default:
                    result = false;
                    break;
            }
            return result;
        }

        public bool IsCSSFontOption()
        {
            bool result;
            switch (SelectedCSSOptions)
            {
                case CSS_Options.IncludeHttpsFonts:
                case CSS_Options.IncludeBootstrapAndFonts:
                case CSS_Options.IncludeAOSHttpsFonts:
                case CSS_Options.IncludeAOSBootstrapAndFonts:
                    result = true;
                    break;

                default:
                    result = false;
                    break;
            }
            return result;
        }

        public bool IsCSSBootstrapOption()
        {
            return Options.IsCSSBootstrap(SelectedCSSOptions);
        }

        public static bool IsCSSBootstrap(CSS_Options cssOption)
        {
            bool result;
            switch (cssOption)
            {
                case CSS_Options.IncludeBootstrap:
                case CSS_Options.IncludeBootstrapAndFonts:
                case CSS_Options.IncludeAOSBootstrap:
                case CSS_Options.IncludeAOSBootstrapAndFonts:
                    result = true;
                    break;

                default:
                    result = false;
                    break;
            }
            return result;
        }

        public bool IsCSSAOSOption()
        {
            bool result;
            switch (SelectedCSSOptions)
            {
                case CSS_Options.IncludeAOS:
                case CSS_Options.IncludeAOSHttpsFonts:
                case CSS_Options.IncludeAOSBootstrap:
                case CSS_Options.IncludeAOSBootstrapAndFonts:
                    result = true;
                    break;

                default:
                    result = false;
                    break;
            }
            return result;
        }

        public static bool IsCSSWebassemblyAllowed(CSS_Options cssOption,
                                                    Asset_Options assetOption)
        {
            bool result;
            if (cssOption == CSS_Options.None)
                result = true;
            else if ((assetOption == Asset_Options.SharedPages) &&
                      (Options.IsCSSBootstrap(cssOption) == false))
            {
                result = true;
            }
            else
                result = false;

            return result;
        }

        public bool IsCSSOptionsWebassemblyAllowed()
        {
            bool result = Options.IsCSSWebassemblyAllowed(SelectedCSSOptions,
                                                          SelectedAssetOptions);
            return result;
        }

        public string BootstrapProjectPath { get; set; }
        public string BootstrapHtml { get; set; }
        public string BootstrapIndexHtml { get; set; }
        public string BootstrapAssets { get; set; }
        public string HostHtml { get; set; }
        public string HostAssetsDirectory { get; set; }
        public string WasmHostHtml { get; set; }
        public string RazorHostHtml { get; set; }
        public string BlazorProjectPath { get; set; }
        public string NavMenuFile { get; set; }

        public string PageName { get; set; }
        public string IndexPageName { get; set; }

        public bool AllowNonBootstrap { get; set; }
        public Modify_Options SelectedModifyOptions { get; set; }
        public Asset_Options SelectedAssetOptions { get; set; }
        public CSS_Options SelectedCSSOptions { get; set; }
        public Host_Options SelectedHostOptions { get; set; }
        public Script_Options SelectedScriptOptions { get; set; }
        public NavMenu_Options SelectedNavMenuOptions { get; set; }
        public Page_Options SelectedRazorOptions { get; set; }
        public Trace_Options SelectedTraceOptions { get; set; }
        public RenderMode_Options SelectedRenderModeOptions { get; set; }
        public string PreviewFile { get; set; }
        public bool AllowHtmlEditing { get; set; }
        public string BootStrapOverrideHtml { get; set; }
        public bool MakeCustomRazorLayout { get; set; }
        public string PWAFile { get; set; }
        public string RazorNameSpace { get; set; }
        public string RazorNameSpaceSource { get; set; }
    }
}