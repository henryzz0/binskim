﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

using FluentAssertions;

using Xunit;

namespace Microsoft.CodeAnalysis.BinaryParsers.ELF
{
    public class ELFBinaryTests
    {
        internal static string TestData = GetTestDirectory(@"Test.UnitTests.BinaryParsers" + Path.DirectorySeparatorChar + "TestsData");

        internal static string GetTestDirectory(string relativeDirectory)
        {
            var codeBaseUrl = new Uri(Assembly.GetExecutingAssembly().CodeBase);
            string codeBasePath = Uri.UnescapeDataString(codeBaseUrl.AbsolutePath);
            string dirPath = Path.GetDirectoryName(codeBasePath);
            dirPath = Path.Combine(dirPath, string.Format(@"..{0}..{0}..{0}..{0}src{0}", Path.DirectorySeparatorChar));
            dirPath = Path.GetFullPath(dirPath);
            return Path.Combine(dirPath, relativeDirectory);
        }

        [Fact]
        public void ValidateDwarfV4_WithO2()
        {
            // Hello.c compiled using: gcc -Wall -O2 -g -gdwarf-4 hello.c -o hello4
            string fileName = Path.Combine(TestData, @"Dwarf/hello-dwarf4-o2");
            using var binary = new ELFBinary(new Uri(fileName));
            binary.GetVersion().Should().Be(4);
            binary.GetDwarfCompilerCommand().Should().Contain("O2");
            binary.GetLanguage().Should().Be("C99");
        }

        [Fact]
        public void ValidateDwarfV5_WithO2()
        {
            // Hello.c compiled using: gcc -Wall -O2 -g -gdwarf-5 hello.c -o hello5
            string fileName = Path.Combine(TestData, @"Dwarf/hello-dwarf5-o2");
            using var binary = new ELFBinary(new Uri(fileName));
            binary.GetVersion().Should().Be(5);
            binary.GetDwarfCompilerCommand().Should().Contain("O2");
            binary.GetLanguage().Should().Be("C11");
        }

        [Fact]
        public void x()
        {
            // Hello.c compiled using: gcc -Wall -O2 -g -gdwarf-5 hello.c -o hello5
            using var binary = new ELFBinary(new Uri(@"C:\Users\ednakamu\Desktop\mmcli"));
            binary.GetVersion().Should().Be(5);
            binary.GetDwarfCompilerCommand().Should().Contain("O2");
            binary.GetLanguage().Should().Be("C99");
        }
    }
}
