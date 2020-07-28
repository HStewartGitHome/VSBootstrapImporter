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
    public class AssetTests
    {
        #region Data
        readonly string _dummyStr = "dummy";
        #endregion

        #region Constructor
        public AssetTests()
        {


        }
        #endregion

        #region Asset Contructor
        [TestMethod]
        public void Constructor_DummyNormalTest()
        {
            Asset asset = CreateDummyAsset();
            Assert.IsNotNull(asset);
        }


        #endregion

        #region  IsScriptAsset

        [DataTestMethod]
        [DataRow(2)]                // ScriptAsset
        [DataRow(4)]                // ScriptHttpsAsset
        public void IsScriptAsset_NormalTest(int value)
        {
            Asset asset = CreateDummyAsset();
            asset.AssetType = (AssetType_Options)value;

            bool result = asset.IsScriptAsset();
            Assert.IsTrue(result);
        }

        [DataTestMethod]
        [DataRow(1)]                // ImageAsset
        [DataRow(3)]                // HostAsset
        [DataRow(0)]                // none
        public void IsScriptAsset_FailureTest(int value)
        {
            Asset asset = CreateDummyAsset();
            asset.AssetType = (AssetType_Options)value;

            bool result = asset.IsScriptAsset();
            Assert.IsFalse(result);
        }

        #endregion

        #region Support method
        private Asset CreateDummyAsset()
        {
            Asset asset = new Asset(_dummyStr, AssetType_Options.HostAsset);
            return asset;
        }
        #endregion
    }
}
