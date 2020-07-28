using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VSBootstrapImporter.Common.Models;
using VSBootstrapImporter.Common.Services;


namespace VSBootstrapImporter.Tests
{
    [TestClass]
    public class CommonTests
    {
        #region GetSourceDirectory
        [TestMethod]
        public void GetSourceDirectory_TestNormalSuccess()
        {
            Options options = new Options();
            string bootstrapProject = @"c:\test\bootstrap\";
            string expected = @"c:\test\bootstrap\assets";

            options.BootstrapProjectPath = bootstrapProject;
            string actual = CommonSupport.GetSourceDirectory(options);

            if (string.IsNullOrEmpty(actual))
                Assert.IsTrue(false);
            else
                Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetSourceDirectory_TestNormalFailure()
        {
            Options options = new Options();
            string bootstrapProject = @"c:\test\bootstrap\";
            string expected = @"c:\test\bootstrap\assets";

            options.BootstrapProjectPath = bootstrapProject;
            options.BootstrapAssets = "newassets";
            string actual = CommonSupport.GetSourceDirectory(options);


            if (string.IsNullOrEmpty(actual))
                Assert.IsTrue(false);
            else
                Assert.AreNotEqual(expected, actual);

        }

        [TestMethod]
        public void GetSourceDirectory_TestOverideSuccess()
        {
            Options options = new Options();
            string bootstrapProject = @"c:\test\override\";
            string expected = @"c:\test\override\assets";

            options.AllowHtmlEditing = true;
            options.BootStrapOverrideHtml = bootstrapProject;
            string actual = CommonSupport.GetSourceDirectory(options);

            if (string.IsNullOrEmpty(actual))
                Assert.IsTrue(false);
            else
                Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetSourceDirectory_TestOverrideFailure()
        {
            Options options = new Options();
            string bootstrapProject = @"c:\test\override\";
            string expected = @"c:\test\bootstrap\assets";

            options.AllowHtmlEditing = true;
            options.BootStrapOverrideHtml = bootstrapProject;
            string actual = CommonSupport.GetSourceDirectory(options);


            if (string.IsNullOrEmpty(actual))
                Assert.IsTrue(false);
            else
                Assert.AreNotEqual(expected, actual);

        }
        #endregion

        #region  GetDestinationDirectory

        [TestMethod]
        public void GetDestinationDirectory_TestNormalSuccess()
        {
            Options options = new Options();
            string blazorProject = @"c:\test\BlazorServer\";
            string expected = @"c:\test\BlazorServer\wwwroot\assets";

            options.BlazorProjectPath = blazorProject;
            options.HostAssetsDirectory = @"wwwroot\assets";
            string actual = CommonSupport.GetDestinationDirectory(options);

            if (string.IsNullOrEmpty(actual))
                Assert.IsTrue(false);
            else
                Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetDestinationDirectory_TestNormalFailure()
        {
            Options options = new Options();
            string blazorProject = @"c:\test\BlazorServer";
            string expected = @"c:\test\BlazorServer\wwwroot\assets";

            options.BlazorProjectPath = blazorProject;
            options.HostAssetsDirectory = @"wwwroot\assets";
            string actual = CommonSupport.GetDestinationDirectory(options);

            if (string.IsNullOrEmpty(actual))
                Assert.IsTrue(false);
            else
                Assert.AreNotEqual(expected, actual);
        }
        #endregion
    }
}
