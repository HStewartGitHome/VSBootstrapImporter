using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSBootstrapImporter.Common.Models;

namespace VSBootstrapImporter.Tests
{

    [TestClass]
    public class DataInfoTests
    {
        #region data
        bool _firstTime = true;
        string _asset1 = "";
        string _asset2 = "";
        #endregion



        #region IsWebAssemblyType
        [DataTestMethod]
        [DataRow(2)]                // BlazorWebassembly
        [DataRow(3)]                // BlazorWebassemblyPWA
        public void IsWebAssemblyType_NormalTest(int value)
        {
            bool result = DataInfo.IsWebAssemblyType((Type_Options)value);
            Assert.IsTrue(result);
        }

        [DataTestMethod]
        [DataRow(1)]                // BlazorServer
        [DataRow(4)]                // ASPNetRazor
        [DataRow(0)]                // Unknowned
        public void IsWebAssemblyType_FailureTest(int value)
        {
            bool result = DataInfo.IsWebAssemblyType((Type_Options)value);
            Assert.IsFalse(result);
        }
        #endregion

        #region GetAssetFromString
        [TestMethod]
        public void GetAssetFromString_NormalTest1()
        {
            DataInfo info = CreateDataInfoWithAssets();
            Asset asset = info.GetAssetFromString(_asset1);
            Assert.IsNotNull(asset);
            Assert.AreEqual(_asset1, asset.HtmlAsset);
        }

        [TestMethod]
        public void GetAssetFromString_NormalTest2()
        {
            DataInfo info = CreateDataInfoWithAssets();
            Asset asset = info.GetAssetFromString(_asset2);
            Assert.IsNotNull(asset);
            Assert.AreEqual(_asset2, asset.HtmlAsset);
        }

        [TestMethod]
        public void GetAssetFromString_FailureTest()
        {
            string str = "test asset";
            DataInfo info = CreateDataInfoWithAssets();
            Asset asset = info.GetAssetFromString(str);
            Assert.IsNull(asset);
        }
        #endregion

        #region HasScriptAsset
        [TestMethod]
        public void HasScriptAsset_NormalTest()
        {
            DataInfo info = CreateDataInfoWithAssets();
            bool result = info.HasScriptAsset();
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void HasScriptAsset_FailureTest()
        {
            DataInfo info = new DataInfo();
            Asset asset = new Asset(_asset1, AssetType_Options.ImageAsset);
            info.Assets.Add(asset);
            bool result = info.HasScriptAsset();
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void HasScriptAsset_FailureEmptyTest()
        {
            DataInfo info = new DataInfo();
            bool result = info.HasScriptAsset();
            Assert.IsFalse(result);
        }

        #endregion

        #region GetIsBootstrapString
        [TestMethod]
        public void GetIsBootstrapString_BootstrapTest()
        {
            DataInfo info = new DataInfo
            {
                IsBootstrapHtml = true
            };
            string expected = "Bootstrap";
            string actual = info.GetIsBootstrapString();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetIsBootstrapString_OtherTest()
        {
            DataInfo info = new DataInfo
            {
                IsBootstrapHtml = false
            };
            string expected = "Other";
            string actual = info.GetIsBootstrapString();
            Assert.AreEqual(expected, actual);
        }
        #endregion

        #region Support Methods
        private DataInfo CreateDataInfoWithAssets()
        {
            DataInfo info = new DataInfo();

            if ( _firstTime )
            {
                _asset1 = "src=temp.bmp";
                _asset2 = "script=x";
                _firstTime = false;
            }

            // just fake misc data
            Asset asset = new Asset(_asset1, AssetType_Options.ImageAsset);
            info.Assets.Add(asset);
            asset = new Asset(_asset2, AssetType_Options.ScriptAsset);
            info.Assets.Add(asset);

            return info;

        }
        #endregion

    }
}
