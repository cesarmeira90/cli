﻿using System;
using System.Collections.Generic;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tests.Objects
{
    internal static class MockPackage
    {
        internal static readonly MockFileSystem Html = new MockFileSystem(new Dictionary<string, MockFileData>
            {{ "/src/packages/customization.common/package.json", new MockFileData(
            @"{
              ""name"": ""customization.package""
            }")},
            { "/src/packages/customization.common/customization.common.js", new MockFileData(string.Empty)},
            { "/package.json", new MockFileData(
            @"{
              ""name"": ""customization.package""
            }")},
            { "/cmfpackage.json", new MockFileData(
            @"{
              ""packageId"": ""Cmf.Custom.HTML"",
              ""version"": ""1.1.0"",
              ""description"": ""Cmf Custom HTML Package"",
              ""packageType"": ""Html"",
              ""isInstallable"": true,
              ""isUniqueInstall"": false,
              ""contentToPack"": [
                {
                  ""source"": ""src\\packages\\*"",
                  ""target"": ""node_modules"",
                  ""ignoreFiles"": [
                    "".npmignore""
                  ]
                }
              ]
            }")}});
    }
}
