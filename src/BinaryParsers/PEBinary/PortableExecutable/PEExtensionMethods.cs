﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Reflection.PortableExecutable;

namespace Microsoft.CodeAnalysis.BinaryParsers.PortableExecutable
{
    internal static class PortableExecutableExtensionMethods
    {
        public static object SafePointerToType(this SafePointer sp, ImageFieldData fi)
        {
            object res;

            switch (fi.Type)
            {
                case Type.BYTE: res = (byte)sp; break;
                case Type.SBYTE: res = (sbyte)(byte)sp; break;
                case Type.UINT16: res = (ushort)sp; break;
                case Type.INT16: res = (short)(ushort)sp; break;
                case Type.UINT32: res = (uint)sp; break;
                case Type.UINT64: res = (ulong)sp; break;
                case Type.INT32: res = (int)(uint)sp; break;
                case Type.INT64: res = (long)(ulong)sp; break;
                case Type.POINTER: res = new SafePointer(sp.array, sp.stream, (int)(uint)sp); break;
                case Type.HEADER: res = fi.Header.Create(fi.ParentHeader, sp); break;
                case Type.NATIVEINT:
                    PEHeader ioh = fi.ParentHeader;
                    res = ((ioh != null) && (ioh.Magic == PEMagic.PE32Plus))
                        ? (ulong)sp
                        : (uint)sp;
                    break;
                default: throw new InvalidOperationException("Unknown type");
            }
            return res;
        }

        public static int GetTypeLen(this ImageFieldData fi)
        {
            int res = 0;
            bool b64bit = false;
            PEHeader ioh = fi.ParentHeader;
            if (ioh != null)
            {
                b64bit = (ioh.Magic == PEMagic.PE32Plus);
            }

            if (b64bit && fi.Is32BitOnly)
            {
                return 0;
            }

            switch (fi.Type)
            {
                case Type.BYTE: res = 1; break;
                case Type.SBYTE: res = 1; break;
                case Type.UINT16: res = 2; break;
                case Type.INT16: res = 2; break;
                case Type.UINT32: res = 4; break;
                case Type.INT32: res = 4; break;
                case Type.UINT64: res = 8; break;
                case Type.INT64: res = 8; break;
                case Type.POINTER: res = 4; break;
                case Type.HEADER: res = fi.Header.Size; break;
                case Type.NATIVEINT: res = b64bit ? 8 : 4; break;

                default: throw new Exception("Unknown type");
            }

            return res;
        }
    }
}
