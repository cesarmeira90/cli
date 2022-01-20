﻿using Cmf.Common.Cli.Attributes;
using Cmf.Common.Cli.Objects;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Cmf.Common.Cli.Commands.New
{
    /// <summary>
    /// Generates Database package structure
    /// </summary>
    [CmfCommand("database", Parent = "new")]
    public class DatabaseCommand : LayerTemplateCommand
    {
        /// <inheritdoc />
        public DatabaseCommand() : base("database", Enums.PackageType.Database)
        {
            this.registerInParent = false;
        }

        /// <inheritdoc />
        protected override List<string> GenerateArgs(IDirectoryInfo projectRoot, IDirectoryInfo workingDir, List<string> args, JsonDocument projectConfig)
        {
            var relativePathToRoot =
                ExecutionContext.Instance.FileSystem.Path.Join("..", "..", //always two levels deeper
                    ExecutionContext.Instance.FileSystem.Path.GetRelativePath(
                        workingDir.FullName,
                        projectRoot.FullName)
                ).Replace("\\", "/");

            args.AddRange(new[]
            {
                "--rootRelativePath", relativePathToRoot
            });

            return args;
        }
    }
}
