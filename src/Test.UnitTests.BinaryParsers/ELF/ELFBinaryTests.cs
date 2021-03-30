// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;

using Xunit;

namespace Microsoft.CodeAnalysis.BinaryParsers.ELF
{
    public class ELFBinaryTests
    {
        [Fact]
        public void Load()
        {
            string filename = @"C:\Microsoft\drops\2020-03-23\retail\amd64\Polybase\jre\lib\security\cacerts";
            var binary = new ELFBinary(new Uri(filename));
            var version = binary.GetVersion();
            var command = binary.GetDwarfCompilerCommand();
            var language = binary.GetLanguage();
        }
    }
}
