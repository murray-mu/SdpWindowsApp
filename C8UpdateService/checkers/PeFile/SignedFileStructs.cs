/*
	Copyright NetFoundry Inc.

	Licensed under the Apache License, Version 2.0 (the "License");
	you may not use this file except in compliance with the License.
	You may obtain a copy of the License at

	https://www.apache.org/licenses/LICENSE-2.0

	Unless required by applicable law or agreed to in writing, software
	distributed under the License is distributed on an "AS IS" BASIS,
	WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
	See the License for the specific language governing permissions and
	limitations under the License.
*/

using System;
using System.Runtime.InteropServices;

namespace C8UpdateService.Checkers.PeFile {

    //defined by http://www.pinvoke.net/default.aspx/Structures/IMAGE_FILE_HEADER.html
    [StructLayout(LayoutKind.Sequential)]
    public struct IMAGE_FILE_HEADER {
        public UInt16 Machine;
        public UInt16 NumberOfSections;
        public UInt32 TimeDateStamp;
        public UInt32 PointerToSymbolTable;
        public UInt32 NumberOfSymbols;
        public UInt16 SizeOfOptionalHeader;
        public UInt16 Characteristics;
    }

    //see http://www.pinvoke.net/default.aspx/Structures/IMAGE_OPTIONAL_HEADER32.html
    //defined by http://www.pinvoke.net/default.aspx/Structures/IMAGE_OPTIONAL_HEADER32.html
    [StructLayout(LayoutKind.Sequential)]
    public struct IMAGE_DATA_DIRECTORY {
        public UInt32 VirtualAddress;
        public UInt32 Size;
    }

    public enum MachineType : ushort {
        Native = 0,
        I386 = 0x014c,
        Itanium = 0x0200,
        x64 = 0x8664
    }

    public sealed class MagicType {
        public const Int16 IMAGE_NT_OPTIONAL_HDR32_MAGIC = 0x10b;
        public const Int16 IMAGE_NT_OPTIONAL_HDR64_MAGIC = 0x20b;
    }

    public enum SubSystemType : ushort {
        IMAGE_SUBSYSTEM_UNKNOWN = 0,
        IMAGE_SUBSYSTEM_NATIVE = 1,
        IMAGE_SUBSYSTEM_WINDOWS_GUI = 2,
        IMAGE_SUBSYSTEM_WINDOWS_CUI = 3,
        IMAGE_SUBSYSTEM_POSIX_CUI = 7,
        IMAGE_SUBSYSTEM_WINDOWS_CE_GUI = 9,
        IMAGE_SUBSYSTEM_EFI_APPLICATION = 10,
        IMAGE_SUBSYSTEM_EFI_BOOT_SERVICE_DRIVER = 11,
        IMAGE_SUBSYSTEM_EFI_RUNTIME_DRIVER = 12,
        IMAGE_SUBSYSTEM_EFI_ROM = 13,
        IMAGE_SUBSYSTEM_XBOX = 14

    }

    public enum DllCharacteristicsType : ushort {
        RES_0 = 0x0001,
        RES_1 = 0x0002,
        RES_2 = 0x0004,
        RES_3 = 0x0008,
        IMAGE_DLL_CHARACTERISTICS_DYNAMIC_BASE = 0x0040,
        IMAGE_DLL_CHARACTERISTICS_FORCE_INTEGRITY = 0x0080,
        IMAGE_DLL_CHARACTERISTICS_NX_COMPAT = 0x0100,
        IMAGE_DLLCHARACTERISTICS_NO_ISOLATION = 0x0200,
        IMAGE_DLLCHARACTERISTICS_NO_SEH = 0x0400,
        IMAGE_DLLCHARACTERISTICS_NO_BIND = 0x0800,
        RES_4 = 0x1000,
        IMAGE_DLLCHARACTERISTICS_WDM_DRIVER = 0x2000,
        IMAGE_DLLCHARACTERISTICS_TERMINAL_SERVER_AWARE = 0x8000
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct IMAGE_OPTIONAL_HEADER32 {
        [FieldOffset(0)] public ushort Magic;

        [FieldOffset(2)] public byte MajorLinkerVersion;

        [FieldOffset(3)] public byte MinorLinkerVersion;

        [FieldOffset(4)] public uint SizeOfCode;

        [FieldOffset(8)] public uint SizeOfInitializedData;

        [FieldOffset(12)] public uint SizeOfUninitializedData;

        [FieldOffset(16)] public uint AddressOfEntryPoint;

        [FieldOffset(20)] public uint BaseOfCode;

        // PE32 contains this additional field
        [FieldOffset(24)] public uint BaseOfData;

        [FieldOffset(28)] public uint ImageBase;

        [FieldOffset(32)] public uint SectionAlignment;

        [FieldOffset(36)] public uint FileAlignment;

        [FieldOffset(40)] public ushort MajorOperatingSystemVersion;

        [FieldOffset(42)] public ushort MinorOperatingSystemVersion;

        [FieldOffset(44)] public ushort MajorImageVersion;

        [FieldOffset(46)] public ushort MinorImageVersion;

        [FieldOffset(48)] public ushort MajorSubsystemVersion;

        [FieldOffset(50)] public ushort MinorSubsystemVersion;

        [FieldOffset(52)] public uint Win32VersionValue;

        [FieldOffset(56)] public uint SizeOfImage;

        [FieldOffset(60)] public uint SizeOfHeaders;

        [FieldOffset(64)] public uint CheckSum;

        [FieldOffset(68)] public SubSystemType Subsystem;

        [FieldOffset(70)] public DllCharacteristicsType DllCharacteristics;

        [FieldOffset(72)] public uint SizeOfStackReserve;

        [FieldOffset(76)] public uint SizeOfStackCommit;

        [FieldOffset(80)] public uint SizeOfHeapReserve;

        [FieldOffset(84)] public uint SizeOfHeapCommit;

        [FieldOffset(88)] public uint LoaderFlags;

        [FieldOffset(92)] public uint NumberOfRvaAndSizes;

        [FieldOffset(96)] public IMAGE_DATA_DIRECTORY ExportTable;

        [FieldOffset(104)] public IMAGE_DATA_DIRECTORY ImportTable;

        [FieldOffset(112)] public IMAGE_DATA_DIRECTORY ResourceTable;

        [FieldOffset(120)] public IMAGE_DATA_DIRECTORY ExceptionTable;

        [FieldOffset(128)] public IMAGE_DATA_DIRECTORY CertificateTable;

        [FieldOffset(136)] public IMAGE_DATA_DIRECTORY BaseRelocationTable;

        [FieldOffset(144)] public IMAGE_DATA_DIRECTORY Debug;

        [FieldOffset(152)] public IMAGE_DATA_DIRECTORY Architecture;

        [FieldOffset(160)] public IMAGE_DATA_DIRECTORY GlobalPtr;

        [FieldOffset(168)] public IMAGE_DATA_DIRECTORY TLSTable;

        [FieldOffset(176)] public IMAGE_DATA_DIRECTORY LoadConfigTable;

        [FieldOffset(184)] public IMAGE_DATA_DIRECTORY BoundImport;

        [FieldOffset(192)] public IMAGE_DATA_DIRECTORY IAT;

        [FieldOffset(200)] public IMAGE_DATA_DIRECTORY DelayImportDescriptor;

        [FieldOffset(208)] public IMAGE_DATA_DIRECTORY CLRRuntimeHeader;

        [FieldOffset(216)] public IMAGE_DATA_DIRECTORY Reserved;
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct IMAGE_OPTIONAL_HEADER64 {
        [FieldOffset(0)] public ushort Magic;

        [FieldOffset(2)] public byte MajorLinkerVersion;

        [FieldOffset(3)] public byte MinorLinkerVersion;

        [FieldOffset(4)] public uint SizeOfCode;

        [FieldOffset(8)] public uint SizeOfInitializedData;

        [FieldOffset(12)] public uint SizeOfUninitializedData;

        [FieldOffset(16)] public uint AddressOfEntryPoint;

        [FieldOffset(20)] public uint BaseOfCode;

        [FieldOffset(24)] public ulong ImageBase;

        [FieldOffset(32)] public uint SectionAlignment;

        [FieldOffset(36)] public uint FileAlignment;

        [FieldOffset(40)] public ushort MajorOperatingSystemVersion;

        [FieldOffset(42)] public ushort MinorOperatingSystemVersion;

        [FieldOffset(44)] public ushort MajorImageVersion;

        [FieldOffset(46)] public ushort MinorImageVersion;

        [FieldOffset(48)] public ushort MajorSubsystemVersion;

        [FieldOffset(50)] public ushort MinorSubsystemVersion;

        [FieldOffset(52)] public uint Win32VersionValue;

        [FieldOffset(56)] public uint SizeOfImage;

        [FieldOffset(60)] public uint SizeOfHeaders;

        [FieldOffset(64)] public uint CheckSum;

        [FieldOffset(68)] public SubSystemType Subsystem;

        [FieldOffset(70)] public DllCharacteristicsType DllCharacteristics;

        [FieldOffset(72)] public ulong SizeOfStackReserve;

        [FieldOffset(80)] public ulong SizeOfStackCommit;

        [FieldOffset(88)] public ulong SizeOfHeapReserve;

        [FieldOffset(96)] public ulong SizeOfHeapCommit;

        [FieldOffset(104)] public uint LoaderFlags;

        [FieldOffset(108)] public uint NumberOfRvaAndSizes;

        [FieldOffset(112)] public IMAGE_DATA_DIRECTORY ExportTable;

        [FieldOffset(120)] public IMAGE_DATA_DIRECTORY ImportTable;

        [FieldOffset(128)] public IMAGE_DATA_DIRECTORY ResourceTable;

        [FieldOffset(136)] public IMAGE_DATA_DIRECTORY ExceptionTable;

        [FieldOffset(144)] public IMAGE_DATA_DIRECTORY CertificateTable;

        [FieldOffset(152)] public IMAGE_DATA_DIRECTORY BaseRelocationTable;

        [FieldOffset(160)] public IMAGE_DATA_DIRECTORY Debug;

        [FieldOffset(168)] public IMAGE_DATA_DIRECTORY Architecture;

        [FieldOffset(176)] public IMAGE_DATA_DIRECTORY GlobalPtr;

        [FieldOffset(184)] public IMAGE_DATA_DIRECTORY TLSTable;

        [FieldOffset(192)] public IMAGE_DATA_DIRECTORY LoadConfigTable;

        [FieldOffset(200)] public IMAGE_DATA_DIRECTORY BoundImport;

        [FieldOffset(208)] public IMAGE_DATA_DIRECTORY IAT;

        [FieldOffset(216)] public IMAGE_DATA_DIRECTORY DelayImportDescriptor;

        [FieldOffset(224)] public IMAGE_DATA_DIRECTORY CLRRuntimeHeader;

        [FieldOffset(232)] public IMAGE_DATA_DIRECTORY Reserved;
    }


    [StructLayout(LayoutKind.Explicit)]
    public struct IMAGE_SECTION_HEADER {
        //FROM: http://www.pinvoke.net/default.aspx/Structures/IMAGE_SECTION_HEADER.html
        [FieldOffset(0)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public char[] Name;

        [FieldOffset(8)] public UInt32 VirtualSize;

        [FieldOffset(12)] public UInt32 VirtualAddress;

        [FieldOffset(16)] public UInt32 SizeOfRawData;

        [FieldOffset(20)] public UInt32 PointerToRawData;

        [FieldOffset(24)] public UInt32 PointerToRelocations;

        [FieldOffset(28)] public UInt32 PointerToLinenumbers;

        [FieldOffset(32)] public UInt16 NumberOfRelocations;

        [FieldOffset(34)] public UInt16 NumberOfLinenumbers;

        [FieldOffset(36)] public DataSectionFlags Characteristics;

        public string Section {
            get { return new string(Name); }
        }
    }

    [Flags]
    public enum DataSectionFlags : uint {
        //FROM: http://www.pinvoke.net/default.aspx/Structures/IMAGE_SECTION_HEADER.html
        /// <summary>
        /// Reserved for future use.
        /// </summary>
        TypeReg = 0x00000000,

        /// <summary>
        /// Reserved for future use.
        /// </summary>
        TypeDsect = 0x00000001,

        /// <summary>
        /// Reserved for future use.
        /// </summary>
        TypeNoLoad = 0x00000002,

        /// <summary>
        /// Reserved for future use.
        /// </summary>
        TypeGroup = 0x00000004,

        /// <summary>
        /// The section should not be padded to the next boundary. This flag is obsolete and is replaced by IMAGE_SCN_ALIGN_1BYTES. This is valid only for object files.
        /// </summary>
        TypeNoPadded = 0x00000008,

        /// <summary>
        /// Reserved for future use.
        /// </summary>
        TypeCopy = 0x00000010,

        /// <summary>
        /// The section contains executable code.
        /// </summary>
        ContentCode = 0x00000020,

        /// <summary>
        /// The section contains initialized data.
        /// </summary>
        ContentInitializedData = 0x00000040,

        /// <summary>
        /// The section contains uninitialized data.
        /// </summary>
        ContentUninitializedData = 0x00000080,

        /// <summary>
        /// Reserved for future use.
        /// </summary>
        LinkOther = 0x00000100,

        /// <summary>
        /// The section contains comments or other information. The .drectve section has this type. This is valid for object files only.
        /// </summary>
        LinkInfo = 0x00000200,

        /// <summary>
        /// Reserved for future use.
        /// </summary>
        TypeOver = 0x00000400,

        /// <summary>
        /// The section will not become part of the image. This is valid only for object files.
        /// </summary>
        LinkRemove = 0x00000800,

        /// <summary>
        /// The section contains COMDAT data. For more information, see section 5.5.6, COMDAT Sections (Object Only). This is valid only for object files.
        /// </summary>
        LinkComDat = 0x00001000,

        /// <summary>
        /// Reset speculative exceptions handling bits in the TLB entries for this section.
        /// </summary>
        NoDeferSpecExceptions = 0x00004000,

        /// <summary>
        /// The section contains data referenced through the global pointer (GP).
        /// </summary>
        RelativeGP = 0x00008000,

        /// <summary>
        /// Reserved for future use.
        /// </summary>
        MemPurgeable = 0x00020000,

        /// <summary>
        /// Reserved for future use.
        /// </summary>
        Memory16Bit = 0x00020000,

        /// <summary>
        /// Reserved for future use.
        /// </summary>
        MemoryLocked = 0x00040000,

        /// <summary>
        /// Reserved for future use.
        /// </summary>
        MemoryPreload = 0x00080000,

        /// <summary>
        /// Align data on a 1-byte boundary. Valid only for object files.
        /// </summary>
        Align1Bytes = 0x00100000,

        /// <summary>
        /// Align data on a 2-byte boundary. Valid only for object files.
        /// </summary>
        Align2Bytes = 0x00200000,

        /// <summary>
        /// Align data on a 4-byte boundary. Valid only for object files.
        /// </summary>
        Align4Bytes = 0x00300000,

        /// <summary>
        /// Align data on an 8-byte boundary. Valid only for object files.
        /// </summary>
        Align8Bytes = 0x00400000,

        /// <summary>
        /// Align data on a 16-byte boundary. Valid only for object files.
        /// </summary>
        Align16Bytes = 0x00500000,

        /// <summary>
        /// Align data on a 32-byte boundary. Valid only for object files.
        /// </summary>
        Align32Bytes = 0x00600000,

        /// <summary>
        /// Align data on a 64-byte boundary. Valid only for object files.
        /// </summary>
        Align64Bytes = 0x00700000,

        /// <summary>
        /// Align data on a 128-byte boundary. Valid only for object files.
        /// </summary>
        Align128Bytes = 0x00800000,

        /// <summary>
        /// Align data on a 256-byte boundary. Valid only for object files.
        /// </summary>
        Align256Bytes = 0x00900000,

        /// <summary>
        /// Align data on a 512-byte boundary. Valid only for object files.
        /// </summary>
        Align512Bytes = 0x00A00000,

        /// <summary>
        /// Align data on a 1024-byte boundary. Valid only for object files.
        /// </summary>
        Align1024Bytes = 0x00B00000,

        /// <summary>
        /// Align data on a 2048-byte boundary. Valid only for object files.
        /// </summary>
        Align2048Bytes = 0x00C00000,

        /// <summary>
        /// Align data on a 4096-byte boundary. Valid only for object files.
        /// </summary>
        Align4096Bytes = 0x00D00000,

        /// <summary>
        /// Align data on an 8192-byte boundary. Valid only for object files.
        /// </summary>
        Align8192Bytes = 0x00E00000,

        /// <summary>
        /// The section contains extended relocations.
        /// </summary>
        LinkExtendedRelocationOverflow = 0x01000000,

        /// <summary>
        /// The section can be discarded as needed.
        /// </summary>
        MemoryDiscardable = 0x02000000,

        /// <summary>
        /// The section cannot be cached.
        /// </summary>
        MemoryNotCached = 0x04000000,

        /// <summary>
        /// The section is not pageable.
        /// </summary>
        MemoryNotPaged = 0x08000000,

        /// <summary>
        /// The section can be shared in memory.
        /// </summary>
        MemoryShared = 0x10000000,

        /// <summary>
        /// The section can be executed as code.
        /// </summary>
        MemoryExecute = 0x20000000,

        /// <summary>
        /// The section can be read.
        /// </summary>
        MemoryRead = 0x40000000,

        /// <summary>
        /// The section can be written to.
        /// </summary>
        MemoryWrite = 0x80000000
    }


    public class ImageOptionalHeaderWrapper {
        public IMAGE_OPTIONAL_HEADER32 h32;
        public IMAGE_OPTIONAL_HEADER64 h64;
        public bool IsPe;


        public uint CertificateTableVirtualAddress {
            get {
                if (IsPe) {
                    return h32.CertificateTable.VirtualAddress;
                } else {
                    return h64.CertificateTable.VirtualAddress;
                }
            }
        }

        public uint CertificateTableSize {
            get {
                if (IsPe) {
                    return h32.CertificateTable.Size;
                } else {
                    return h64.CertificateTable.Size;
                }
            }
        }

        public uint SizeOfHeaders {
            get {
                if (IsPe) {
                    return h32.SizeOfHeaders;
                } else {
                    return h64.SizeOfHeaders;
                }
            }
        }
    }
}