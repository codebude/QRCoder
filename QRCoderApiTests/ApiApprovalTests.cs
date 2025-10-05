using System.Diagnostics;
using System.Reflection;
using System.Xml.Linq;
using PublicApiGenerator;
using Shouldly;
using Xunit;

/*********************************************
 * 
 * This file copied from GraphQL.NET on 4/26/2024
 * https://github.com/graphql-dotnet/graphql-dotnet/blob/dce3a8d9335eb2ff0674a1e48af01fdd6b942119/src/GraphQL.ApiTests/ApiApprovalTests.cs
 * 
 * Unmodified portions of this file are subject to the following license:
 * https://github.com/graphql-dotnet/graphql-dotnet/blob/dce3a8d9335eb2ff0674a1e48af01fdd6b942119/LICENSE.md
 *
 * The MIT License (MIT)
 *
 * Copyright (c) 2015-2023 Joseph T. McBride, Ivan Maximov, Shane Krueger, et al. All rights reserved.
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy of this software and
 * associated documentation files (the "Software"), to deal in the Software without restriction, including
 * without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the
 * following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
 * TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
 * THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
 * CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
 * IN THE SOFTWARE.
 * 
 *********************************************/

namespace QRCoderApiTests;

/// <summary>
/// See more info about API approval tests here <see href="https://github.com/JakeGinnivan/ApiApprover"/>.
/// </summary>
public class ApiApprovalTests
{
    [Theory]
    [InlineData(typeof(QRCoder.QRCodeData))]
    [InlineData(typeof(QRCoder.Xaml.XamlQRCode))]
    public void PublicApi(Type type)
    {
        string baseDir = AppDomain.CurrentDomain.BaseDirectory;
        string projectName = type.Assembly.GetName().Name!;
        string testDir = Path.Combine(baseDir, $"..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}..");
        string projectDir = Path.Combine(testDir, "..");
        string buildDir = Path.Combine(projectDir, projectName, "bin",
#if DEBUG
            "Debug");
#else
            "Release");
#endif
        Debug.Assert(Directory.Exists(buildDir), $"Directory '{buildDir}' doesn't exist");
        string csProject = Path.Combine(projectDir, projectName, projectName + ".csproj");
        var project = XDocument.Load(csProject);
        string[] tfms = project.Descendants("TargetFrameworks").Union(project.Descendants("TargetFramework")).First().Value.Split(";", StringSplitOptions.RemoveEmptyEntries);

        // There may be old stuff from earlier builds like net45, netcoreapp3.0, etc. so filter it out
        string[] actualTfmDirs = Directory.GetDirectories(buildDir).Where(dir => tfms.Any(tfm => dir.EndsWith(tfm))).ToArray();
        Debug.Assert(actualTfmDirs.Length > 0, $"Directory '{buildDir}' doesn't contain subdirectories matching {string.Join(";", tfms)}");

        (string tfm, string content)[] publicApi = actualTfmDirs.Select(tfmDir => (new DirectoryInfo(tfmDir).Name.Replace(".", ""), Assembly.LoadFile(Path.Combine(tfmDir, projectName + ".dll")).GeneratePublicApi(new ApiGeneratorOptions
        {
            IncludeAssemblyAttributes = false,
            //AllowNamespacePrefixes = new[] { "Microsoft.Extensions.DependencyInjection" },
            ExcludeAttributes = new[] { "System.Diagnostics.DebuggerDisplayAttribute", "System.Diagnostics.CodeAnalysis.AllowNullAttribute" }
        }) + Environment.NewLine)).ToArray();

        if (publicApi.DistinctBy(item => item.content).Count() == 1)
        {
            AutoApproveOrFail(publicApi[0].content, "");
        }
        else
        {
            foreach (var item in publicApi.ToLookup(item => item.content))
            {
                AutoApproveOrFail(item.Key, string.Join("+", item.Select(x => x.tfm).OrderBy(x => x)));
            }
        }

        // Approval test should (re)generate approved.txt files locally if needed.
        // Approval test should fail on CI.
        // https://docs.github.com/en/actions/learn-github-actions/environment-variables#default-environment-variables
        void AutoApproveOrFail(string publicApi, string folder)
        {
            string file = null!;

            try
            {
                publicApi.ShouldMatchApproved(options => options.SubFolder(folder).NoDiff().WithFilenameGenerator((testMethodInfo, discriminator, fileType, fileExtension) => file = $"{type.Assembly.GetName().Name}.{fileType}.{fileExtension}"));
            }
            catch (ShouldMatchApprovedException) when (Environment.GetEnvironmentVariable("CI") == null)
            {
                string? received = Path.Combine(testDir, folder, file);
                string? approved = received.Replace(".received.txt", ".approved.txt");
                if (File.Exists(received) && File.Exists(approved))
                {
                    File.Copy(received, approved, overwrite: true);
                    File.Delete(received);
                }
                else
                {
                    throw;
                }
            }
        }
    }
}
