using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSBootstrapImporter.Common.Models;
using VSBootstrapImporter.Common.Services;
using VSBootstrapImporter.Common.Services.IO;
using VSBootstrapImporter.Tests.IO;
using VSBootstrapImporter.Tests.Models;
using VSBootstrapImporter.Tests.Services;
using WpfNetBootstrap.Common.Services.IO;

namespace VSBootstrapImporter.Tests
{
    [TestClass]
    public class GeneratorTests
    {

        #region Setup

        private static IFileIO _fileIO = null;

        [ClassInitialize]
#pragma warning disable IDE0060 // Remove unused parameter
        public static void ClassInitialize(TestContext testContext)
#pragma warning restore IDE0060 // Remove unused parameter
        {
            SetDefaultFileIO();
            FileFactory.Initialize();
        }

        [TestInitialize]
        public void TestInitialize()
        {
        }

        [TestCleanup]
        public void TestCleanup()
        {
        }

        #endregion Setup

        #region Generator Empty Project Failue
        [TestMethod]
        public void Generator_FailureEmptyProjectTest()
        {
            Options CurrentOptions = new Options();
            DataInfo info = DlgSupport.GenerateData(CurrentOptions, true, false);
            Assert.IsNotNull(info, "Empty Project Failed");
        }
        #endregion

        #region Success Min Preview Test

        [TestMethod]
        [DeploymentItem(@"MinBlazor\min.html", "MinBlazor")]
        [DeploymentItem(@"MinBlazor\_host.cshtml", "MinBlazor")]
        [DeploymentItem(@"MinBlazor\navmenu.razor", "MinBlazor")]
        public void Generator_NormalMinPreviewTest()
        {
            string str;
            GeneratorTests.SetMinFileIO("MinBlazor");

            Options CurrentOptions = CreateMinOptions("MinBlazor", Type_Options.BlazorServer);
            DataInfo info = DlgSupport.GenerateData(CurrentOptions, true, false);
            Assert.IsNotNull(info, "DataInfo is null");
            str = "BLAZOR Project Type " + info.ProjectType.ToString() + " is not Blazor Project";
            Compare.LogFailure(info.ProjectType == Type_Options.BlazorServer, true, str);
            Assert.IsTrue(info.ProjectType == Type_Options.BlazorServer, str);
            str = "BLAZOR Preview Generate is not successful";
            Compare.LogFailure(info.IsGenerateSuccessfull, true, str);
            Assert.IsTrue(info.IsGenerateSuccessfull, str);
            str = "BLAZOR Preview is not created";
            Compare.LogFailure(info.IsPreviewCreated, true, str);
            Assert.IsTrue(info.IsPreviewCreated, str);
            str = "BLAZOR Preview Modifications should not happen";
            Compare.LogFailure(info.IsModificationsSuccessfull, false, str);
            Assert.IsFalse(info.IsModificationsSuccessfull, str);

            bool flag = false;
            if (info.ScriptAssets.Count == 2)
                flag = true;
            str = "BLAZOR Scripts should be 2, and has value of " + info.ScriptAssets.Count.ToString();
            Compare.LogFailure(flag, true, str);
            Assert.IsTrue(flag, str);
            if (info.MultiLineScriptCount == 0)
                flag = true;
            else
                flag = false;
            str = "BLAZOR Embedded Scripts should be 0, and has value of " + info.MultiLineScriptCount.ToString();
            Compare.LogFailure(flag, true, str);
            Assert.IsTrue(flag, str);

            GeneratorTests.SetDefaultFileIO();
        }

        #endregion

        #region Success Min Blazor Output 

        [TestMethod]
        [DeploymentItem(@"MinBlazor\min.html", "MinBlazor")]
        [DeploymentItem(@"MinBlazor\_host.cshtml", "MinBlazor")]
        [DeploymentItem(@"MinBlazor\navmenu.razor", "MinBlazor")]
        public void Generator_NormalMinBlazorOutputTest()
        {
            string str;
            string dir = "MinBlazor";
            string dataDir = @"\MinBlazor\";
            GeneratorTests.SetMinFileIO(dir);
            Options CurrentOptions = CreateMinOptions(dir, Type_Options.BlazorServer);

            DataInfo info = DlgSupport.GenerateData(CurrentOptions, false, true);
            Assert.IsNotNull(info, "DataInfo is null for Min Blazor");
            str = "BLAZOR Project Type " + info.ProjectType.ToString() + " is not Blazor Project";
            Compare.LogFailure(info.ProjectType == Type_Options.BlazorServer, true, str);
            Assert.IsTrue(info.ProjectType == Type_Options.BlazorServer, str);
            str = "BLAZOR Generate is not successful for Min Blazor";
            Compare.LogFailure(info.IsGenerateSuccessfull, true, str);
            Assert.IsTrue(info.IsGenerateSuccessfull, str);
            str = "BLAZOR Preview is not created for Min Blazor";
            Compare.LogFailure(info.IsPreviewCreated, true, str);
            Assert.IsTrue(info.IsPreviewCreated, str);
            str = "BLAZOR Modifications is not successfull for Min Blazor";
            Compare.LogFailure(info.IsModificationsSuccessfull, true, str);
            Assert.IsTrue(info.IsModificationsSuccessfull, str);

            bool flag = false;
            if (info.ScriptAssets.Count == 2)
                flag = true;
            str = "BLAZOR Scripts should be 2, and has value of " + info.ScriptAssets.Count.ToString();
            Compare.LogFailure(flag, true, str);
            Assert.IsTrue(flag, str);
            if (info.MultiLineScriptCount == 0)
                flag = true;
            else
                flag = false;
            str = "BLAZOR Embedded Scripts should be 0, and has value of " + info.MultiLineScriptCount.ToString();
            Compare.LogFailure(flag, true, str);
            Assert.IsTrue(flag, str);

            CompareResult result = Compare.CompareGeneratedFile("_host.cshtml", "Min", dataDir);
            str = "BLAZOR _host.cshtml count is different Actual Count=" + result.File1Count.ToString() + " Expected Count="+ result.File2Count.ToString();
            Compare.LogFailure(result.CountDifferent, false, str);
            Assert.IsFalse(result.CountDifferent, str);
            str = "BLAZOR _host.cshtml contents is different for Min Blazor";
            Compare.LogFailure(result.ContentsDifferent, false, str);
            Assert.IsFalse(result.ContentsDifferent, str);
        

            result = Compare.CompareGeneratedFile("NavMenu.Razor", "Min",dataDir);
            str = "BLAZOR NavMenu.Razor count is different for Min Blazor";
            Assert.IsFalse(result.CountDifferent, str);
            Compare.LogFailure(result.CountDifferent, false, str);
            str = "BLAZOR NavMenu.Razor contents is different for Min Blazor";
            Compare.LogFailure(result.ContentsDifferent, false, str);
            Assert.IsFalse(result.ContentsDifferent, str);
     

            result = Compare.CompareGeneratedFile("Min.Razor", "Min", dataDir);
            str = "BLAZOR Min.Razor count is different Actual Count=" + result.File1Count.ToString() + " Expected Count=" + result.File2Count.ToString();
            Compare.LogFailure(result.CountDifferent, false, str);
            Assert.IsFalse(result.CountDifferent, str);
            str = "BLAZOR Min.Razor contents is different for Min Blazor";
            Compare.LogFailure(result.ContentsDifferent, false, str);
            Assert.IsFalse(result.ContentsDifferent, str);
  

            GeneratorTests.SetDefaultFileIO();
        }
        #endregion

        #region Success Min Blazor Wasm Output 

        [TestMethod]
        [DeploymentItem(@"MinWasm\min.html", "MinWasm")]
        [DeploymentItem(@"MinWasm\index.html", "MinWasm")]
        [DeploymentItem(@"MinWasm\navmenu.razor", "MinWasm")]
        public void Generator_NormalMinBlazorWasmOutputTest()
        {
            string str;
            string dir = "MinWasm";
            string dataDir = @"\MinWasm\";
            GeneratorTests.SetMinFileIO(dir);
            Options CurrentOptions = CreateMinOptions(dir, Type_Options.BlazorWebassembly);

            DataInfo info = DlgSupport.GenerateData(CurrentOptions, false, true);
            Assert.IsNotNull(info, "DataInfo is null for Min Blazor Wasm");
            str = "WASM Project Type " + info.ProjectType.ToString() + " is not Blazor Wasm Project";
            Compare.LogFailure(info.ProjectType == Type_Options.BlazorWebassembly, true, str);
            Assert.IsTrue(info.ProjectType == Type_Options.BlazorWebassembly, str);
            str = "WASM Generate is not successful for Min Blazor Wasm";
            Compare.LogFailure(info.IsGenerateSuccessfull, true, str);
            Assert.IsTrue(info.IsGenerateSuccessfull, str);
            str = "WASM Preview is not created for Min Blazor Wasm";
            Compare.LogFailure(info.IsPreviewCreated, true, str);
            Assert.IsTrue(info.IsPreviewCreated, str);
            str = "WASM Modifications is not successfull for Min Blazor Wasm";
            Compare.LogFailure(info.IsModificationsSuccessfull, true, str);
            Assert.IsTrue(info.IsModificationsSuccessfull, str);

            bool flag = false;
            if (info.ScriptAssets.Count == 2)
                flag = true;
            str = "WASM Scripts should be 2, and has value of " + info.ScriptAssets.Count.ToString();
            Compare.LogFailure(flag, true, str);
            Assert.IsTrue(flag, str);
            if (info.MultiLineScriptCount == 0)
                flag = true;
            else
                flag = false;
            str = "WASM Embedded Scripts should be 0, and has value of " + info.MultiLineScriptCount.ToString();
            Compare.LogFailure(flag, true, str);
            Assert.IsTrue(flag, str);

            CompareResult result = Compare.CompareGeneratedFile("index.html", "Wasm", dataDir);
            str = "WASM index.html count is different Actual Count=" + result.File1Count.ToString() + " Expected Count=" + result.File2Count.ToString();
            Compare.LogFailure(result.CountDifferent, false, str);
            Assert.IsFalse(result.CountDifferent, str);
            str = "WASM index.html contents is different for Min Blazor Wasm";
            Compare.LogFailure(result.ContentsDifferent, false, str);
            Assert.IsFalse(result.ContentsDifferent, str);


            result = Compare.CompareGeneratedFile("NavMenu.Razor", "Wasm", dataDir);
            str = "WASM NavMenu.Razor count is different for Min Blazor Wasm";
            Assert.IsFalse(result.CountDifferent, str);
            Compare.LogFailure(result.CountDifferent, false, str);
            str = "WASM NavMenu.Razor contents is different for Min Blazor Wasm";
            Compare.LogFailure(result.ContentsDifferent, false, str);
            Assert.IsFalse(result.ContentsDifferent, str);


            result = Compare.CompareGeneratedFile("Wasm.Razor", "Wasm", dataDir);
            str = "WASM Wasm.Razor count is different Actual Count=" + result.File1Count.ToString() + " Expected Count=" + result.File2Count.ToString();
            Compare.LogFailure(result.CountDifferent, false, str);
            Assert.IsFalse(result.CountDifferent, str);
            str = "WASM Wasm.Razor contents is different for Blazor Wasm";
            Compare.LogFailure(result.ContentsDifferent, false, str);
            Assert.IsFalse(result.ContentsDifferent, str);


            GeneratorTests.SetDefaultFileIO();
        }
        #endregion

        #region Success Min Blazor Wasm PWA Output 

        [TestMethod]
        [DeploymentItem(@"MinWasmPWA\min.html", "MinWasmPWA")]
        [DeploymentItem(@"MinWasmPWA\index.html", "MinWasmPWA")]
        [DeploymentItem(@"MinWasmPWA\navmenu.razor", "MinWasmPWA")]
        [DeploymentItem(@"MinWasmPWA\manifest.json", "MinWasmPWA")]
        public void Generator_NormalMinBlazorWasmPWAOutputTest()
        {
            string str;
            string dir = "MinWasmPWA";
            string dataDir = @"\MinWasmPWA\";
            GeneratorTests.SetMinFileIO(dir);
            Options CurrentOptions = CreateMinOptions(dir, Type_Options.BlazorWebassemblyPWA);

            DataInfo info = DlgSupport.GenerateData(CurrentOptions, false, true);
            Assert.IsNotNull(info, "DataInfo is null for Min Blazor PWA Wasm");
            str = "WASM PWA Project Type " + info.ProjectType.ToString() + " is not Blazor Wasm PWA Project";
            Compare.LogFailure(info.ProjectType == Type_Options.BlazorWebassemblyPWA, true, str);
            Assert.IsTrue(info.ProjectType == Type_Options.BlazorWebassemblyPWA, str);
            str = "WASM PWA Generate is not successful for Min Blazor Wasm PWA";
            Compare.LogFailure(info.IsGenerateSuccessfull, true, str);
            Assert.IsTrue(info.IsGenerateSuccessfull, str);
            str = "WASM PWA Preview is not created for Min Blazor Wasm PWA";
            Compare.LogFailure(info.IsPreviewCreated, true, str);
            Assert.IsTrue(info.IsPreviewCreated, str);
            str = "WASM PWA Modifications is not successfull for Min Blazor Wasm PWA";
            Compare.LogFailure(info.IsModificationsSuccessfull, true, str);
            Assert.IsTrue(info.IsModificationsSuccessfull, str);

            bool flag = false;
            if (info.ScriptAssets.Count == 2)
                flag = true;
            str = "WASM PWA Scripts should be 2, and has value of " + info.ScriptAssets.Count.ToString();
            Compare.LogFailure(flag, true, str);
            Assert.IsTrue(flag, str);
            if (info.MultiLineScriptCount == 0)
                flag = true;
            else
                flag = false;
            str = "WASM PWA Embedded Scripts should be 0, and has value of " + info.MultiLineScriptCount.ToString();
            Compare.LogFailure(flag, true, str);
            Assert.IsTrue(flag, str);

            CompareResult result = Compare.CompareGeneratedFile("index.html", "WasmPWA", dataDir);
            str = "WASM PWA index.html count is different Actual Count=" + result.File1Count.ToString() + " Expected Count=" + result.File2Count.ToString();
            Compare.LogFailure(result.CountDifferent, false, str);
            Assert.IsFalse(result.CountDifferent, str);
            str = "WASM PWA index.html contents is different for Min Blazor Wasm";
            Compare.LogFailure(result.ContentsDifferent, false, str);
            Assert.IsFalse(result.ContentsDifferent, str);


            result = Compare.CompareGeneratedFile("NavMenu.Razor", "WasmPWA", dataDir);
            str = "WASM PWA NavMenu.Razor count is different for Min Blazor Wasm";
            Assert.IsFalse(result.CountDifferent, str);
            Compare.LogFailure(result.CountDifferent, false, str);
            str = "WASM PWA NavMenu.Razor contents is different for Min Blazor Wasm";
            Compare.LogFailure(result.ContentsDifferent, false, str);
            Assert.IsFalse(result.ContentsDifferent, str);


            result = Compare.CompareGeneratedFile("WasmPWA.Razor", "WasmPWA", dataDir);
            str = "WASM PWA WasmPWA.Razor count is different Actual Count=" + result.File1Count.ToString() + " Expected Count=" + result.File2Count.ToString();
            Compare.LogFailure(result.CountDifferent, false, str);
            Assert.IsFalse(result.CountDifferent, str);
            str = "WASM PWA WasmPWA.Razor contents is different for Blazor Wasm PWA";
            Compare.LogFailure(result.ContentsDifferent, false, str);
            Assert.IsFalse(result.ContentsDifferent, str);


            GeneratorTests.SetDefaultFileIO();
        }
        #endregion

        #region Success Min Razor Output 

        [TestMethod]
        [DeploymentItem(@"MinRazor\min.html", "MinRazor")]
        [DeploymentItem(@"MinRazor\_layout.cshtml", "MinRazor")]
        public void Generator_NormalMinRazorOutputTest()
        {
            string str;
            string dir = "MinRazor";
            string dataDir = @"\MinRazor\";
            GeneratorTests.SetMinFileIO(dir);
            Options CurrentOptions = CreateMinOptions(dir, Type_Options.ASPNetRazor);

            DataInfo info = DlgSupport.GenerateData(CurrentOptions, false, true);
            Assert.IsNotNull(info, "DataInfo is null for Min Razor");
            str = "RAZOR Project Type " + info.ProjectType.ToString() + " is not a Razor Project";
            Compare.LogFailure(info.ProjectType == Type_Options.ASPNetRazor, true, str);
            Assert.IsTrue(info.ProjectType == Type_Options.ASPNetRazor, str);
            str = "RAZOR Generate is not successful for Min Razor";
            Compare.LogFailure(info.IsGenerateSuccessfull, true, str);
            Assert.IsTrue(info.IsGenerateSuccessfull, str);
            str = "RAZOR Preview is not created for Min Blazor Razor";
            Compare.LogFailure(info.IsPreviewCreated, true, str);
            Assert.IsTrue(info.IsPreviewCreated, str);
            str = "RAZOR Modifications is not successfull for Min Razor";
            Compare.LogFailure(info.IsModificationsSuccessfull, true, str);
            Assert.IsTrue(info.IsModificationsSuccessfull, str);

            bool flag = false;
            if (info.ScriptAssets.Count == 2)
                flag = true;
            str = "RAZOR Scripts should be 2, and has value of " + info.ScriptAssets.Count.ToString();
            Compare.LogFailure(flag, true, str);
            Assert.IsTrue(flag, str);
            if (info.MultiLineScriptCount == 0)
                flag = true;
            else
                flag = false;
            str = "RAZOR Embedded Scripts should be 0, and has value of " + info.MultiLineScriptCount.ToString();
            Compare.LogFailure(flag, true, str);
            Assert.IsTrue(flag, str);

            CompareResult result = Compare.CompareGeneratedFile("_layout.cshtml", "Razor", dataDir);
            str = "RAZOR _layout.cshtml count is different Actual Count=" + result.File1Count.ToString() + " Expected Count=" + result.File2Count.ToString();
            Compare.LogFailure(result.CountDifferent, false, str);
            Assert.IsFalse(result.CountDifferent, str);
            str = "RAZOR _layout.cshtml contents is different for Min Razor";
            Compare.LogFailure(result.ContentsDifferent, false, str);
            Assert.IsFalse(result.ContentsDifferent, str);


            result = Compare.CompareGeneratedFile("Razor_layout.cshtml", "Razor", dataDir);
            str = "RAZOR Razor_layout.cshtml count is different Actual Count=" + result.File1Count.ToString() + " Expected Count=" + result.File2Count.ToString();
            Compare.LogFailure(result.CountDifferent, false, str);
            Assert.IsFalse(result.CountDifferent, str);
            str = "RAZOR Razor_layout.cshtml contents is different for Min Razor";
            Compare.LogFailure(result.ContentsDifferent, false, str);
            Assert.IsFalse(result.ContentsDifferent, str);

            result = Compare.CompareGeneratedFile("Razor.cshtml","Razor", dataDir);
            str = "RAZOR Razor.cshtml count is different Actual Count=" + result.File1Count.ToString() + " Expected Count=" + result.File2Count.ToString();
            Compare.LogFailure(result.CountDifferent, false, str);
            Assert.IsFalse(result.CountDifferent, str);
            str = "RAZOR Razor.cshtml contents is different for Razor";
            Compare.LogFailure(result.ContentsDifferent, false, str);
            Assert.IsFalse(result.ContentsDifferent, str);


            GeneratorTests.SetDefaultFileIO();
        }
        #endregion

        #region Support

        private Options CreateMinOptions(string dir,
                                         Type_Options typeOptions )
        {
            string filePath = ".\\" + dir + "\\";

            Options options = new Options
            {
                BootstrapProjectPath = filePath,
                PageName = "Min",
                BlazorProjectPath = filePath,
                BootStrapOverrideHtml = "min.html",
                AllowHtmlEditing = true
            };

            // temp
            options.SelectedTraceOptions = Trace_Options.TraceAll;
            // temp

            switch ( typeOptions)
            {
                case Type_Options.BlazorWebassembly:
                case Type_Options.BlazorWebassemblyPWA:
                    options.SelectedAssetOptions = Asset_Options.SharedPagesMove;
                    options.SelectedCSSOptions = CSS_Options.IncludeAOSHttpsFonts;
                    options.SelectedRenderModeOptions = RenderMode_Options.NoChange;
                    if ( typeOptions == Type_Options.BlazorWebassembly)
                        options.PageName = "Wasm";
                    else
                        options.PageName = "WasmPWA";
                    break;
                case Type_Options.ASPNetRazor:
                    options.SelectedRenderModeOptions = RenderMode_Options.NoChange;
                    options.PageName = "Razor";
                    break;
            }

            return options;
        }

        private static void SetDefaultFileIO()
        {
            _fileIO = new FileIO();
            FileIOFactory.SetFileIO(_fileIO);
        }

        private static void SetMinFileIO(string dir)
        {
            string dataDir = ".\\" + dir + "\\";
            _fileIO = new MinTestFileIO(dataDir);
            FileIOFactory.SetFileIO(_fileIO);
        }

        #endregion
    }
}
