using System.Collections.Generic;

namespace VSBootstrapImporter.Common.Models
{
    public class DataInfo
    {
        public DataInfo()
        {
            Assets = new List<Asset>();
            AssetsStrings = new List<string>();
            BlazorStrings = new List<string>();
            HostAssetsStrings = new List<string>();
            HostRenderMode = "";
            HostStrings = new List<string>();
            HtmlStrings = new List<string>();
            IsBootstrapHtml = false;
            NavMenuStrings = new List<string>();
            NewHostStrings = new List<string>();
            NewNavMenuStrings = new List<string>();
            PreviewStrings = new List<string>();
            ProjectType = Type_Options.Unknown;
            RazorAssetsStrings = new List<string>();
            RazorCodeBehindStrings = new List<string>();
            RazorCustomLayoutStrings = new List<string>();
            MultiLineScriptCount = 0;
            ScriptAssets = new List<string>();

            ResetForGeneration();
        }
        public bool IsBootstrapHtml { get; set; }
        public List<Asset> Assets { get; set; }
        public List<string> AssetsStrings { get; set; }
        public List<string> BlazorStrings { get; set; }
        public List<string> HostAssetsStrings { get; set; }
        public List<string> HostStrings { get; set; }
        public List<string> HtmlStrings { get; set; }
        public List<string> NavMenuStrings { get; set; }
        public List<string> NewHostStrings { get; set; }
        public List<string> NewNavMenuStrings { get; set; }
        public List<string> PreviewStrings { get; set; }
        public List<string> RazorAssetsStrings { get; set; }
        public List<string> RazorCodeBehindStrings { get; set; }
        public List<string> RazorCustomLayoutStrings { get; set; }

        public string HostRenderMode { get; set; }
        public Type_Options ProjectType { get; set; }

        public bool IsGenerateSuccessfull { get; set; }
        public bool IsPreviewCreated { get; set; }
        public bool IsModificationsSuccessfull { get; set; }

        public int MultiLineScriptCount { get; set; }
        public List<string> ScriptAssets { get; set; }

        // Public methods

        public void ResetForGeneration()
        {
            IsGenerateSuccessfull = false;
            IsPreviewCreated = false;
            IsModificationsSuccessfull = false;
        }

        public void UpdateGenerateStatus()
        {
            bool generateOk = true;
            if (ProjectType == Type_Options.ASPNetRazor)
            {
                if ((RazorCodeBehindStrings.Count == 0) ||
                     (RazorCustomLayoutStrings.Count == 0))
                {
                    generateOk = false;
                }

            }


            if ((BlazorStrings.Count > 0) &&
                 (NewHostStrings.Count > 0) &&
                 (NavMenuStrings.Count > 0))
            {
                IsGenerateSuccessfull = generateOk;
            }

            if (PreviewStrings.Count > 0)
                IsPreviewCreated = true;
        }

        public static bool IsWebAssemblyType(Type_Options projectType)
        {
            bool result;
            switch (projectType)
            {
                case Type_Options.BlazorWebassembly:
                case Type_Options.BlazorWebassemblyPWA:
                    result = true;
                    break;

                case Type_Options.BlazorServer:
                case Type_Options.ASPNetRazor:
                default:
                    result = false;
                    break;
            }
            return result;
        }

        public Asset GetAssetFromString(string str)
        {
            Asset theAsset = null;

            foreach (Asset workingAsset in Assets)
            {
                if (str.Contains(workingAsset.HtmlAsset) == true)
                    theAsset = workingAsset;
            }

            return theAsset;
        }

        public string GetIsBootstrapString()
        {
            string str = "Other";
            if (IsBootstrapHtml == true)
                str = "Bootstrap";
            return str;
        }

        public bool HasScriptAsset()
        {
            bool result = false;

            foreach (Asset asset in Assets)
            {
                if (asset.IsScriptAsset() == true)
                    result = true;
            }

            return result;
        }

        public bool IsWebAssembly()
        {
            return DataInfo.IsWebAssemblyType(ProjectType);
        }
    }
}