using System.Diagnostics;
using System.IO;

namespace VSBootstrapImporter.Common.Models
{
    public class Asset
    {
        // constructor
        public Asset(string strAsset,
                     AssetType_Options assetType)
        {
            AssetType = assetType;
            HtmlAsset = strAsset;
            Index = 0;

            try
            {
                if (assetType == AssetType_Options.ImageAsset)
                {
                    RawAsset = Path.GetFileName(strAsset);
                }
                else if (assetType == AssetType_Options.ScriptAsset)
                {
                    string strResult = Asset.GetScriptAsset(strAsset);
                    if (string.IsNullOrEmpty(strResult) == false)
                        RawAsset = strResult;
                }
            }
            catch (IOException e)
            {
                Trace.TraceError("Exception creating asset", e);
            }
        }

        public static string GetScriptAsset(string str)
        {
            string output = "";
            string strResult;
            string searchStr = "";
            char quoteChar = '"';
            bool continueExecution = false;

            if (str.Contains("src") == true)
            {
                continueExecution = true;
                searchStr = "src";
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
                            output = Path.GetDirectoryName(strResult);
                        }
                    }
                }
            }

            return output;
        }


        // public variables

        public AssetType_Options AssetType { get; set; }
        public string HtmlAsset { get; set; }
        public int Index { get; set; }
        public string RawAsset { get; set; }

        // public methods
        public bool IsScriptAsset()
        {
            bool result;
            switch (AssetType)
            {
                case AssetType_Options.ScriptAsset:
                case AssetType_Options.ScriptHttpsAsset:
                    result = true;
                    break;

                default:
                    result = false;
                    break;
            }
            return result;
        }
    }
}