using Microsoft.VisualStudio.TestTools.UnitTesting;
using VSBootstrapImporter.Common.Models;

namespace VSBootstrapImporter.Tests
{
    [TestClass]
    public class OptionsTests
    {

        #region IsReady
        [TestMethod]
        public void IsReady_TestNormalSuccess()
        {
            Options options = CreateDefaultOptions();

            bool isReady = options.IsReady();
            if (isReady)
                Assert.IsTrue(true, "Success");
            else
            {
                if (string.IsNullOrEmpty(options.BlazorProjectPath))
                    Assert.IsTrue(false, "BlazorProjectPath is empty");
                else if (string.IsNullOrEmpty(options.BootstrapProjectPath))
                    Assert.IsTrue(false, "BootstrapProjectPath is empty");
                else if (string.IsNullOrEmpty(options.PageName))
                    Assert.IsTrue(false, "PageName is empty");
                else
                    Assert.IsTrue(false, "Unknown failure");
            }
        }

        [TestMethod]
        public void IsReady_TestNoBlazorProjectFailure()
        {
            Options options = CreateDefaultOptions();
            options.BlazorProjectPath = "";

            bool isReady = options.IsReady();
            if (isReady == false)
                Assert.IsTrue(true, "Successful failure on no BlazorProject");
            else
               Assert.IsTrue(false, "Should not have success with no BlazorProject");
        }

        [TestMethod]
        public void IsReady_TestNoBootstrapProjectFailure()
        {
            Options options = CreateDefaultOptions();
            options.BootstrapProjectPath = "";

            bool isReady = options.IsReady();
            if (isReady == false)
                Assert.IsTrue(true, "Successful failure on no BootstrapProject");
            else
                Assert.IsTrue(false, "Should not have success with no BootstrapProject");
        }

        [TestMethod]
        public void IsReady_TestNoPageNameFailure()
        {
            Options options = CreateDefaultOptions();
            options.PageName = "";

            bool isReady = options.IsReady();
            if (isReady == false)
                Assert.IsTrue(true, "Successful failure on no PageName");
            else
                Assert.IsTrue(false, "Should not have success with no PageName");
        }

        [TestMethod]
        public void IsReady_TestEmptyFailure()
        {
            Options options = new Options
            {
                BlazorProjectPath = "",
                BootstrapProjectPath = "",
                PageName = ""
            };

            bool isReady = options.IsReady();
            if (isReady == false)
                Assert.IsTrue(true, "Successful failure on empty");
            else
                Assert.IsTrue(false, "Should not have success if empty");
        }
        #endregion

        #region HasPreview

        [DataTestMethod]
        [DataRow(1)]                // ModifyProject
        [DataRow(2)]                // PreviewOnly
        public void HasPreview_NormalTest( int value )
        {
            Options options = new Options
            {
                SelectedModifyOptions = (Modify_Options)value
            };

            bool result = options.HasPreview();
            Assert.IsTrue(result, "Success");
        }

        [DataTestMethod]
        [DataRow(3)]                // ModifyNoPreview
        [DataRow(0)]                // Zero
        public void HasPreview_FailureTest(int value)
        {
            Options options = new Options
            {
                SelectedModifyOptions = (Modify_Options)value
            };

            bool result = options.HasPreview();
            Assert.IsFalse(result, "Success");
        }
        #endregion

        #region CanOutput


        [DataTestMethod]
        [DataRow(1)]                // ModifyProject
        [DataRow(3)]                // ModifyNoPreview
        public void CanOutput_NormalTest(int value)
        {
            Options options = new Options
            {
                SelectedModifyOptions = (Modify_Options)value
            };

            bool result = options.CanOutput();
            Assert.IsTrue(result, "Success");
        }

        [DataTestMethod]
        [DataRow(2)]                // NoPreview
        [DataRow(0)]                // Zero
        public void CanOutput_FailureTest(int value)
        {
            Options options = new Options
            {
                SelectedModifyOptions = (Modify_Options)value
            };

            bool result = options.CanOutput();
            Assert.IsFalse(result, "Success");
        }
        #endregion

        #region GetProjectTypeString

        [DataTestMethod]
        [DataRow(1, "Blazor Server")]
        [DataRow(2, "Blazor Webassembly")]
        [DataRow(3, "Blazor Webassembly PWA")]
        [DataRow(4, "ASP.Net Razor")]
        public void GetProjectTypeString_NormalTest(int value, string str)
        {
            string actual = Options.GetProjectTypeString((Type_Options)value);
            Assert.AreEqual(str, actual);
        }

        [DataTestMethod]
        [DataRow(0, "Unknown please select")]
        public void GetProjectTypeString_FailureTest(int value, string str)
        {
            string actual = Options.GetProjectTypeString((Type_Options)value);
            Assert.AreEqual(str, actual);
        }
        #endregion

        #region GetRenderMode

        [DataTestMethod]
        [DataRow(1, "ServerPrerendered")]
        [DataRow(2, "Server")]
        [DataRow(3, "Static")]
        public void GetRenderMode_NormalTest(int value, string str)
        {
            Options options = new Options
            {
                SelectedRenderModeOptions = (RenderMode_Options)value
            };
            string actual = options.GetRenderModeString();
            Assert.AreEqual(str, actual);
        }

        [DataTestMethod]
        [DataRow(0)]
        public void GetRenderMode_FailurTest(int value)
        {
            Options options = new Options
            {
                SelectedRenderModeOptions = (RenderMode_Options)value
            };
            string actual = options.GetRenderModeString();
            Assert.AreNotEqual("Server", actual);
            Assert.AreNotEqual("Static", actual);
        }
        #endregion

        #region GetImageAssetString

        [DataTestMethod]
        [DataRow(1, "Assets directory is copy from html")]
        [DataRow(4, "Assets directory are specific copy")]
        [DataRow(5, "Assets not copy, use primary index")]
        [DataRow(2, "Assets not copy, use primary index")]
        [DataRow(3, "Assets are shared with primary page")]
        public void GetImageAssetString_NormalTest(int value,string str)
        {
            Options options = new Options
            {
                SelectedAssetOptions = (Asset_Options)value
            };
            string actual = options.GetImageAssetString();
            Assert.AreEqual(str, actual);
        }

        [DataTestMethod]
        [DataRow(0)]
        public void GetImageAssetString_FailureTest(int value)
        {
            Options options = new Options
            {
                SelectedAssetOptions = (Asset_Options)value
            };
            string actual = options.GetImageAssetString();
            Assert.AreNotEqual("Assets directory is copy from html", actual);
            Assert.AreNotEqual("Assets directory are specific copy", actual);
            Assert.AreNotEqual("Assets not copy, use primary index", actual);
            Assert.AreNotEqual("Assets are shared with primary page", actual);
        }
        #endregion

        #region GetRazorNameSpaceSource
        [TestMethod]
        public void GetRazorNameSpaceSource_NormalTest()
        {
            Options options = CreateDefaultOptions();
            string expected = @"C:\test\BlazorServer\Pages\_Viewimports.cshtml";
            string actual = options.GetRazorNameSpaceSource();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetRazorNameSpaceSource_FailureTest()
        {
            Options options = CreateDefaultOptions();
            options.BlazorProjectPath = "";
            string expected = @"C:\test\BlazorServer\Pages\_Viewimports.cshtml";
            string actual = options.GetRazorNameSpaceSource();
            Assert.AreNotEqual(expected, actual);
        }

        #endregion

        #region IsTraceOn

        [DataTestMethod]
        [DataRow(1)]                // Trace Info
        [DataRow(2)]                // Trace Warning
        [DataRow(4)]                // Trace Error
        [DataRow(8)]                // Trace Exceptions
        [DataRow(15)]               // Trace all
        public void IsTraceOn_NormalTraceAllTest(int value)
        {
            Options options = new Options
            {
                SelectedTraceOptions = Trace_Options.TraceAll
            };

            bool result = options.IsTraceOn((Trace_Options)value);
            Assert.IsTrue(result);
        }

        [DataTestMethod]
        [DataRow(1)]                // Trace Info
        [DataRow(2)]                // Trace Warning
        [DataRow(4)]                // Trace Error
        [DataRow(8)]                // Trace Exceptions
        [DataRow(15)]               // Trace all
        public void IsTraceOn_FailureTraceNoneTest(int value)
        {
            Options options = new Options
            {
                SelectedTraceOptions = Trace_Options.TraceNone
            };

            bool result = options.IsTraceOn((Trace_Options)value);
            Assert.IsFalse(result);
        }


        #endregion

        #region IsUseCustomAssets

        [DataTestMethod]
        [DataRow(1)]                // Static
        [DataRow(2)]                // Static same index page
        [DataRow(4)]                // Static move
        [DataRow(5)]                // Static move same index page
        public void IsUseCustomAssets_NormalTest(int value)
        {
            Options options = new Options
            {
                SelectedAssetOptions = (Asset_Options)value
            };

            bool result = options.IsUseCustomAssets();
            Assert.IsTrue(result);
        }

        [DataTestMethod]
        [DataRow(3)]                // Share Pages
        [DataRow(0)]                // None
        public void IsUseCustomAssets_FailureTest(int value)
        {
            Options options = new Options
            {
                SelectedAssetOptions = (Asset_Options)value
            };

            bool result = options.IsUseCustomAssets();
            Assert.IsFalse(result);
        }

        #endregion

        #region IsUseSharedAssets

        [DataTestMethod]
        [DataRow(3)]                // Share Pages
        [DataRow(6)]                // Share Pages Moved
        public void IsUseSharedAssets_NormalTest(int value)
        {
            Options options = new Options
            {
                SelectedAssetOptions = (Asset_Options)value
            };

            bool result = options.IsUseSharedAssets();
            Assert.IsTrue(result);
        }

        [DataTestMethod]
        [DataRow(1)]                // Static
        [DataRow(2)]                // Static same index page
        [DataRow(4)]                // Static move
        [DataRow(5)]                // Static move same index page
        [DataRow(0)]                // None
        public void IsUseSharedAssets_FailureTest(int value)
        {
            Options options = new Options
            {
                SelectedAssetOptions = (Asset_Options)value
            };

            bool result = options.IsUseSharedAssets();
            Assert.IsFalse(result);
        }

        #endregion

        #region IsUseMoveAssets

        [DataTestMethod]
        [DataRow(4)]                // Static move
        [DataRow(5)]                // Static move same index page
        [DataRow(6)]                // Share Pages Moved
        public void IsUseMoveAssets_NormalTest(int value)
        {
            Options options = new Options
            {
                SelectedAssetOptions = (Asset_Options)value
            };

            bool result = options.IsUseMoveAssets();
            Assert.IsTrue(result);
        }

        [DataTestMethod]
        [DataRow(1)]                // Static
        [DataRow(2)]                // Static same index page
        [DataRow(3)]                // Shared Pages
        [DataRow(0)]                // None
        public void IsUseMoveAssets_FailureTest(int value)
        {
            Options options = new Options
            {
                SelectedAssetOptions = (Asset_Options)value
            };

            bool result = options.IsUseMoveAssets();
            Assert.IsFalse(result);
        }

        #endregion

        #region IsUseSameIndexAssets

        [DataTestMethod]
        [DataRow(2)]                // Static same index page
        [DataRow(5)]                // Static same index move
        public void IsUseSameIndexAssets_NormalTest(int value)
        {
            Options options = new Options
            {
                SelectedAssetOptions = (Asset_Options)value
            };

            bool result = options.IsUseSameIndexAssets();
            Assert.IsTrue(result);
        }

        [DataTestMethod]
        [DataRow(1)]                // Static
        [DataRow(3)]                // Share Pages
        [DataRow(4)]                // Static move
        [DataRow(0)]                // None
        public void IsUseSameIndexAssets_FailureTest(int value)
        {
            Options options = new Options
            {
                SelectedAssetOptions = (Asset_Options)value
            };

            bool result = options.IsUseSameIndexAssets();
            Assert.IsFalse(result);
        }

        #endregion

        #region sUseStaticAssets
        [DataTestMethod]
        [DataRow(1)]                // Static 
        [DataRow(4)]                // Static move 
        public void IsUseStaticAssets_NormalTest(int value)
        {
            Options options = new Options
            {
                SelectedAssetOptions = (Asset_Options)value
            };

            bool result = options.IsUseStaticAssets();
            Assert.IsTrue(result);
        }

        [DataTestMethod]
        [DataRow(2)]                // Static same index page
        [DataRow(5)]                // Static same index move
        [DataRow(3)]                // Shared Pages
        [DataRow(0)]                // None
        public void IsUseStaticAssets_FailureTest(int value)
        {
            Options options = new Options
            {
                SelectedAssetOptions = (Asset_Options)value
            };

            bool result = options.IsUseStaticAssets();
            Assert.IsFalse(result);
        }
        #endregion

        #region IsCSSFontOption

        [DataTestMethod]
        [DataRow(1)]                    // IncludeHttpsFonts
        [DataRow(3)]                    // IncludeBootstrapAndFonts
        [DataRow(5)]                    // IncludeAOSHttpsFonts
        [DataRow(7)]                    // IncludeAOSBootstrapAndFonts
        public void IsCSSFontOption_NormalTest(int value)
        {
            Options options = new Options
            {
                SelectedCSSOptions = (CSS_Options)value
            };

            bool result = options.IsCSSFontOption();
            Assert.IsTrue(result);
        }

        [DataTestMethod]
        [DataRow(2)]                    // IncludeBootstrap
        [DataRow(4)]                    // IncludeAOS
        [DataRow(6)]                    // IncludeAOSBootstrap
        [DataRow(0)]                    // None
        public void IsCSSFontOption_FailureTest(int value)
        {
            Options options = new Options
            {
                SelectedCSSOptions = (CSS_Options)value
            };

            bool result = options.IsCSSFontOption();
            Assert.IsFalse(result);
        }

        #endregion

        #region IsCSSBootstrap
        [DataTestMethod]
        [DataRow(2)]                    // IncludeBootstrap
        [DataRow(3)]                    // IncludeBootstrapAndFonts
        [DataRow(6)]                    // IncludeAOSBootstrap
        [DataRow(7)]                    // IncludeAOSBootstrapAndFonts
        public void IsCSSBootstrap_NormalTest(int value)
        {
            Options options = new Options
            {
                SelectedCSSOptions = (CSS_Options)value
            };

            bool result = options.IsCSSBootstrapOption();
            Assert.IsTrue(result);
        }


        [DataTestMethod]
        [DataRow(1)]                    // IncludeHttpsAndFonts
        [DataRow(4)]                    // IncludeAOS
        [DataRow(5)]                    // IncludeAOSHttpsFonts
        [DataRow(0)]                    // None
        public void IsCSSBootstap_FailureTest(int value)
        {
            Options options = new Options
            {
                SelectedCSSOptions = (CSS_Options)value
            };

            bool result = options.IsCSSBootstrapOption();
            Assert.IsFalse(result);
        }
        #endregion

        #region IsCSSAOSOption
        [DataTestMethod]
        [DataRow(4)]                    // IncludeAOS
        [DataRow(5)]                    // IncludeAOSHttpsFonts
        [DataRow(6)]                    // IncludeAOSBootstrap
        [DataRow(7)]                    // IncludeAOSBootstrapAndFonts
        public void IsCSSAOSOption_NormalTest(int value)
        {
            Options options = new Options
            {
                SelectedCSSOptions = (CSS_Options)value
            };

            bool result = options.IsCSSAOSOption();
            Assert.IsTrue(result);
        }


        [DataTestMethod]
        [DataRow(1)]                    // IncludeHttpsAndFonts
        [DataRow(2)]                    // IncludeBootstrap
        [DataRow(3)]                    // IncludeBootstrapHttpsAndFonts
        [DataRow(0)]                    // None
        public void IsCSSAOSOption_FailureTest(int value)
        {
            Options options = new Options
            {
                SelectedCSSOptions = (CSS_Options)value
            };

            bool result = options.IsCSSAOSOption();
            Assert.IsFalse(result);
        }
        #endregion

        #region IsCSSWebassemblyAllowed
        [DataTestMethod]
        [DataRow(0,0)]
        [DataRow(0,1)]
        [DataRow(0,2)]
        [DataRow(0,3)]
        [DataRow(0,4)]
        [DataRow(0,5)]
        [DataRow(0,6)]
        [DataRow(0,7)]
        [DataRow(1,3)]
        [DataRow(4,3)]
        [DataRow(5,3)]
        public void IsCSSWebassemblyAllowed_NormalTest(int css, int asset)
        {
            bool result = Options.IsCSSWebassemblyAllowed((CSS_Options)css, (Asset_Options)asset);
            Assert.IsTrue(result);
        }

        [DataTestMethod]
        [DataRow(2, 1)]
        [DataRow(2, 4)]
        [DataRow(2, 5)]
        [DataRow(2, 6)]
        [DataRow(2, 7)]
        [DataRow(3, 1)]
        [DataRow(3, 4)]
        [DataRow(3, 5)]
        [DataRow(3, 6)]
        [DataRow(3, 7)]
        public void IsCSSWebassemblyAllowed_FailureTest(int css, int asset)
        {
            bool result = Options.IsCSSWebassemblyAllowed((CSS_Options)css, (Asset_Options)asset);
            Assert.IsFalse(result);
        }
        #endregion

        #region Support method
        private Options CreateDefaultOptions()
        {
            Options options = new Options
            {
                BlazorProjectPath = @"C:\test\BlazorServer\",
                BootstrapProjectPath = @"c:\Test\Bootstrap\",
                PageName = "index"
            };
            return options;
        }

        #endregion

    }
}
