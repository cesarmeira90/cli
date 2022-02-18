using Cmf.Common.Cli.Constants;
using Cmf.Common.Cli.Factories;
using Cmf.Common.Cli.Handlers;
using Cmf.Common.Cli.Interfaces;
using Cmf.Common.Cli.Objects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tests.Objects;

namespace tests.Specs
{
    [TestClass]
    public class PackageTypeHandler
    {
        [TestMethod]
        public void GetContentToPack_WithNonExistentIgnoreFiles()
        {
            var fileSystem = MockPackage.Html;

            ExecutionContext.Initialize(fileSystem);

            using (StringWriter standardOutput = new())
            {
                Console.SetOut(standardOutput);
                string exceptionMessage = string.Empty;
                try
                {
                    var cmfPackage = fileSystem.FileInfo.FromFileName(CliConstants.CmfPackageFileName);
                    var packageTypeHandler = PackageTypeFactory.GetPackageTypeHandler(cmfPackage) as PresentationPackageTypeHandler;

                    packageTypeHandler.GetContentToPack(fileSystem.DirectoryInfo.FromDirectoryName("output"));
                }
                catch (Exception ex)
                {
                    exceptionMessage = ex.Message;
                }

                Assert.AreEqual(string.Empty, exceptionMessage);
                Assert.AreEqual($"{ MockUnixSupport.Path(@"C:\src\packages\customization.common\") }.npmignore not found!", standardOutput.ToString().Trim());
            }
        }
    }
}
