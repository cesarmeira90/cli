using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO.Abstractions;
using System.Text.Json;
using Cmf.Common.Cli.Attributes;
using Cmf.Common.Cli.Objects;
using Cmf.Common.Cli.Utilities;

namespace Cmf.Common.Cli.Commands.New
{
    /// <summary>
    /// Generates the Test layer structure
    /// </summary>
    [CmfCommand("test", Parent = "new")]
    public class TestCommand : LayerTemplateCommand
    {
        /// <inheritdoc />
        public TestCommand() : base("test", Enums.PackageType.Tests)
        {
        }

        /// <inheritdoc />
        public override void Configure(Command cmd)
        {
            cmd.AddOption(new Option<string>(
                aliases: new[] { "--version" },
                description: "Package Version",
                getDefaultValue: () => "1.0.0"
            ));
            cmd.Handler = CommandHandler.Create<string>(Execute);
        }

        /// <inheritdoc />
        protected override List<string> GenerateArgs(IDirectoryInfo projectRoot, IDirectoryInfo workingDir, List<string> args, JsonDocument projectConfig)
        {
            

            return args;
        }

        /// <summary>
        /// Execute the command
        /// </summary>
        /// <param name="version">the package version</param>
        public void Execute(string version)
        {
            var packageName = "Cmf.Custom.Tests";
            var projectRoot = FileSystemUtilities.GetProjectRoot(ExecutionContext.Instance.FileSystem);
            var projectConfig = FileSystemUtilities.ReadProjectConfig(ExecutionContext.Instance.FileSystem);
            var tenant = projectConfig.RootElement.GetProperty("Tenant").GetString();
            var args = new List<string>()
            {
                // engine options
                "--output", projectRoot.FullName,
                
                // template symbols
                "--name", packageName,
                "--packageVersion", version,
                "--idSegment", tenant,
                "--Tenant", tenant
            };
            
            var restPort = projectConfig.RootElement.GetProperty("RESTPort").GetString();
            var mesVersion = projectConfig.RootElement.GetProperty("MESVersion").GetString();
            var htmlPort = projectConfig.RootElement.GetProperty("HTMLPort").GetString();
            var vmHostname = projectConfig.RootElement.GetProperty("vmHostname").GetString();
            var testScenariosNugetVersion = projectConfig.RootElement.GetProperty("TestScenariosNugetVersion").GetString();
            var isSslEnabled = projectConfig.RootElement.GetProperty("IsSslEnabled").GetString();
            args.AddRange(new []
            {
                "--vmHostname", vmHostname,
                "--RESTPort", restPort,
                "--testScenariosNugetVersion", testScenariosNugetVersion,
                "--HTMLPort", htmlPort,
                "--MESVersion", mesVersion
            });

            if (string.Equals(isSslEnabled, "True"))
            {
                args.Add("--IsSslEnabled");
            }
            
            this.executedArgs = args.ToArray();
            base.RunCommand(args);
            base.RegisterAsDependencyInParent(packageName, version, projectRoot.FullName, isTestPackage: true);
        }
    }
}