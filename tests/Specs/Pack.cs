using Cmf.Common.Cli.Commands;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.IO;
using System.IO;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.IO.Compression;
using System.Linq;
using tests.Objects;

namespace tests.Specs
{
    [TestClass]
    public class Pack
    {
        [TestMethod]
        public void Args_Paths_BothSpecified()
        {
            string _workingDir = null;
            string _outputDir = null;
            bool? _force = null;

            var packCommand = new PackCommand();
            var cmd = new Command("pack");
            packCommand.Configure(cmd);

            cmd.Handler = CommandHandler.Create<IDirectoryInfo, IDirectoryInfo, string, bool, bool>(
            (workingDir, outputDir, repo, force, skipDependencies) =>
            {
                _workingDir = workingDir.Name;
                _outputDir = outputDir.Name;
                _force = force;
            });

            var console = new TestConsole();
            cmd.Invoke(new[] {
                "-o", "test_package_dir", "working_dir"
            }, console);

            Assert.AreEqual("working_dir", _workingDir);
            Assert.AreEqual("test_package_dir", _outputDir);
            Assert.IsNotNull(_force);
            Assert.IsFalse(_force ?? true);
        }

        [TestMethod]
        public void Args_Paths_WorkDirSpecified()
        {
            string _workingDir = null;
            string _outputDir = null;
            bool? _force = null;

            var packCommand = new PackCommand();
            var cmd = new Command("pack");
            packCommand.Configure(cmd);

            cmd.Handler = CommandHandler.Create<IDirectoryInfo, IDirectoryInfo, string, bool, bool>(
            (workingDir, outputDir, repo, force, skipDependencies) =>
            {
                _workingDir = workingDir.Name;
                _outputDir = outputDir.Name;
                _force = force;
            });

            var console = new TestConsole();
            cmd.Invoke(new[] {
                "working_dir"
            }, console);

            Assert.AreEqual("working_dir", _workingDir);
            Assert.AreEqual("Package", _outputDir);
            Assert.IsNotNull(_force);
            Assert.IsFalse(_force ?? true);
        }

        [TestMethod]
        public void Args_Paths_OutDirSpecified()
        {
            string _workingDir = null;
            string _outputDir = null;
            bool? _force = null;

            var packCommand = new PackCommand();
            var cmd = new Command("pack");
            packCommand.Configure(cmd);

            cmd.Handler = CommandHandler.Create<IDirectoryInfo, IDirectoryInfo, string, bool, bool>(
            (workingDir, outputDir, repo, force, skipDependencies) =>
            {
                _workingDir = workingDir.Name;
                _outputDir = outputDir.Name;
                _force = force;
            });

            var console = new TestConsole();
            cmd.Invoke(new[] {
                "-o", "test_package_dir"
            }, console);

            var curDir = new DirectoryInfo(System.IO.Directory.GetCurrentDirectory());
            Assert.AreEqual(curDir.Name, _workingDir);
            Assert.AreEqual("test_package_dir", _outputDir);
            Assert.IsNotNull(_force);
            Assert.IsFalse(_force ?? true);
        }

        [TestMethod]
        public void Args_Paths_NoneSpecified()
        {
            string _workingDir = null;
            string _outputDir = null;
            bool? _force = null;

            var packCommand = new PackCommand();
            var cmd = new Command("pack");
            packCommand.Configure(cmd);

            cmd.Handler = CommandHandler.Create<IDirectoryInfo, IDirectoryInfo, string, bool, bool>(
            (workingDir, outputDir, repo, force, skipDependencies) =>
            {
                _workingDir = workingDir.Name;
                _outputDir = outputDir.Name;
                _force = force;
            });

            var console = new TestConsole();
            cmd.Invoke(new string[] {
            }, console);

            var curDir = new DirectoryInfo(System.IO.Directory.GetCurrentDirectory());

            Assert.AreEqual(curDir.Name, _workingDir);
            Assert.AreEqual("Package", _outputDir);
            Assert.IsNotNull(_force);
            Assert.IsFalse(_force ?? true);
        }

        [TestMethod]
        public void HTML()
        {
            var fileSystem = MockPackage.Html;

            var packCommand = new PackCommand(fileSystem);
            packCommand.Execute(fileSystem.DirectoryInfo.FromDirectoryName("."), fileSystem.DirectoryInfo.FromDirectoryName("output"), false);

            IEnumerable<IFileInfo> assembledFiles = fileSystem.DirectoryInfo.FromDirectoryName("output").EnumerateFiles("Cmf.Custom.HTML.1.1.0.zip");
            Assert.AreEqual(1, assembledFiles.Count());

            using (Stream zipToOpen = fileSystem.FileStream.Create(assembledFiles.First().FullName, FileMode.Open))
            {
                using (ZipArchive zip = new(zipToOpen, ZipArchiveMode.Read))
                {
                    // these tuples allow us to rewrite entry paths
                    var entriesToExtract = new List<Tuple<ZipArchiveEntry, string>>();
                    entriesToExtract.AddRange(zip.Entries.Select(selector: entry => new Tuple<ZipArchiveEntry, string>(entry, entry.FullName)));

                    List<string> expectedFiles = new()
                    {
                        "config.json",
                        "manifest.xml",
                        @"node_modules\customization.package\package.json",
                        @"node_modules\customization.package\customization.common.js"
                    };
                    Assert.AreEqual(expectedFiles.Count, entriesToExtract.Count);
                    foreach (var expectedFile in expectedFiles)
                    {
                        Assert.IsNotNull(entriesToExtract.FirstOrDefault(x => x.Item2.Equals(expectedFile)));
                    }
                }
            }
        }
    }
}
