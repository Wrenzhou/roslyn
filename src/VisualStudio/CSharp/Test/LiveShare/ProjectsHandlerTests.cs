﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.LanguageServer;
using Xunit;
using CustomProtocol = Microsoft.VisualStudio.LanguageServices.LiveShare.CustomProtocol;

namespace Roslyn.VisualStudio.CSharp.UnitTests.LiveShare
{
    public class ProjectsHandlerTests : AbstractLiveShareRequestHandlerTests
    {
        [Fact]
        public async Task TestProjectsAsync()
        {
            var (solution, ranges) = CreateTestSolution(string.Empty);
            var expected = solution.Projects.Select(p => CreateLspProject(p)).ToArray();

            var results = await TestHandleAsync<object, CustomProtocol.Project[]>(solution, null);
            AssertCollectionsEqual(expected, results, AssertProjectsEqual);
        }

        private void AssertProjectsEqual(CustomProtocol.Project expected, CustomProtocol.Project actual)
        {
            Assert.Equal(expected.Language, actual.Language);
            Assert.Equal(expected.Name, actual.Name);
            AssertCollectionsEqual(expected.SourceFiles, actual.SourceFiles, Assert.Equal);
        }

        private static CustomProtocol.Project CreateLspProject(Project project)
            => new CustomProtocol.Project()
            {
                Language = project.Language,
                Name = project.Name,
                SourceFiles = project.Documents.Select(document => document.GetURI()).ToArray()
            };
    }
}