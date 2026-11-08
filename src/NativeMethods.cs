using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Runtime.Versioning;

namespace Nkf.Net
{
    /// <summary>
    /// nkf32.dll を呼び出すための API
    /// </summary>
    internal static class NativeMethods
    {
        /// <summary>
        /// nkf32.dll のファイル名
        /// </summary>
        private const string nkfdll = "nkf32";

        // Some Linux native state is shared even though conversion options are thread-local.
        private static readonly object SyncRoot = new object();

        //LPWSTR verStr,DWORD nBufferLength /*in TCHARs*/,LPDWORD lpTCHARsReturned /*in TCHARs*/

        /// <summary>
        /// GetNkfVersionSafe のラッパ
        /// </summary>
        /// <param name="varStr"></param>
        /// <param name="nBufferLength"></param>
        /// <param name="lpTCHARsReturned"></param>
        [DllImport(nkfdll, EntryPoint = "GetNkfVersionSafe",
            CallingConvention = CallingConvention.Winapi,
            CharSet = CharSet.Auto,
            BestFitMapping = true,
            ThrowOnUnmappableChar = false)]
        private static extern void GetNkfVersionSafeNative(
            StringBuilder varStr,
            int nBufferLength,
            out int lpTCHARsReturned);

        internal static void GetNkfVersionSafe(StringBuilder verStr, int length, out int returned)
        {
            lock (SyncRoot)
            {
                GetNkfVersionSafeNative(verStr, length, out returned);
            }
        }

        // LPSTR outStr,DWORD nOutBufferLength /*in Bytes*/,LPDWORD lpBytesReturned /*in Bytes*/, LPCSTR inStr,DWORD nInBufferLength /*in Bytes*/);
        [DllImport(nkfdll, EntryPoint = "NkfConvertSafe")]
        private static extern bool NkfConvertSafeNative(
            IntPtr outStr, 
            int nOutBufferLength,
            out int lpBytesReturned,
            IntPtr inStr, 
            int nInBufferLength);

        internal static bool NkfConvertSafe(
            IntPtr output, int capacity, out int returned, IntPtr input, int length, string option)
        {
            WrapNkf.ValidateNkfOption(option, "nkfOption");
            lock (SyncRoot)
            {
                SetNkfOptionNative(option);
                bool result = NkfConvertSafeNative(output, capacity, out returned, input, length);
                if (returned < 0)
                {
                    throw new InvalidOperationException("Native nkf conversion returned an invalid output length.");
                }
                return result;
            }
        }

        [DllImport(nkfdll, EntryPoint = "NkfGetKanjiCode")]
        private static extern int NkfGetKanjiCodeNative();

        internal static int NkfGetKanjiCode()
        {
            lock (SyncRoot)
            {
                return NkfGetKanjiCodeNative();
            }
        }

        [DllImport(nkfdll, EntryPoint = "SetNkfOption", CharSet = CharSet.Ansi)]
        private static extern int SetNkfOptionNative(string option);

        // LPCWSTR fName,DWORD nBufferLength 
        [DllImport(nkfdll, EntryPoint = "NkfFileConvert1Safe", CharSet = CharSet.Auto)]
        private static extern bool NkfFileConvert1SafeNative(String fName,int nBufferLength);

        internal static bool NkfFileConvert1Safe(string fileName, int length, string option)
        {
            WrapNkf.ValidateNkfOption(option, "nkfOption");
            lock (SyncRoot)
            {
                SetNkfOptionNative(option);
                return NkfFileConvert1SafeNative(fileName, length);
            }
        }

        [DllImport(nkfdll, EntryPoint = "GetNkfGuess", CharSet = CharSet.Auto)]
        private static extern bool GetNkfGuessNative(
            StringBuilder outStr,
            int nBufferLength/*in TCHARs*/,
            out int lpTCHARsReturned);

        internal static bool GetNkfGuess(StringBuilder output, int length, out int returned)
        {
            lock (SyncRoot)
            {
                return GetNkfGuessNative(output, length, out returned);
            }
        }

        //        BOOL WINAPI GetNkfGuessW(LPWSTR outStr,DWORD nBufferLength /*in TCHARs*/,LPDWORD lpTCHARsReturned /*in TCHARs*/)

        [StructLayout(LayoutKind.Sequential, Pack=8)]
        public struct MLocation {
            public int size;
            public IntPtr copyrightA;
            public IntPtr versionA;
            public IntPtr dateA;
            public int functions;
        };

        [DllImport(nkfdll, EntryPoint = "GetNkfSupportFunctions", CharSet = CharSet.Ansi)]
        private static extern bool GetNkfSupportFunctionsNative(
            ref MLocation outMLocation,
            int nBufferLength ,
            out int lpBytesReturned);

        internal static bool GetNkfSupportFunctions(ref MLocation output, int length, out int returned)
        {
            lock (SyncRoot)
            {
                return GetNkfSupportFunctionsNative(ref output, length, out returned);
            }
        }

        //NkfUsage(LPSTR outStr,DWORD nBufferLength /*in Bytes*/,LPDWORD lpBytesReturned /*in Bytes*/)

        [DllImport(nkfdll, EntryPoint = "NkfUsage", CharSet = CharSet.Ansi)]
        private static extern bool NkfUsageNative(
            StringBuilder outStr,
            int nBufferLength/*in Bytes*/,
            out int lpBytesReturned);

        internal static bool NkfUsage(StringBuilder output, int length, out int returned)
        {
            lock (SyncRoot)
            {
                return NkfUsageNative(output, length, out returned);
            }
        }
        
        //BOOL WINAPI NkfFileConvert2SafeA(LPCSTR fInName,DWORD fInBufferLength /*in TCHARs*/,LPCSTR fOutName,DWORD fOutBufferLength /*in TCHARs*/)
        [DllImport(nkfdll, EntryPoint = "NkfFileConvert2Safe", CharSet = CharSet.Auto)]
        private static extern bool NkfFileConvert2SafeNative(
            string fInName,
            int fInBufferLength /*in TCHARs*/,
            string fOutName,
            int fOutBufferLength /*in TCHARs*/
            );

        internal static bool NkfFileConvert2Safe(
            string input, int inputLength, string output, int outputLength, string option)
        {
            WrapNkf.ValidateNkfOption(option, "nkfOption");
            lock (SyncRoot)
            {
                SetNkfOptionNative(option);
                return NkfFileConvert2SafeNative(input, inputLength, output, outputLength);
            }
        }

        // Linux 用初期化関数
        [DllImport(nkfdll, EntryPoint = "Init")]
        private static extern void InitNative();

        private static void Init()
        {
            lock (SyncRoot)
            {
                InitNative();
            }
        }

        /// <summary>
        /// 任意のバイト配列を文字列に変換する
        /// </summary>
        /// <param name="data"></param>
        /// <param name="startIndex"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        internal static string NkfConvert(byte[] data, int startIndex, int length)
        {
            if (data == null) throw new ArgumentNullException("data");
            if (startIndex < 0 || startIndex > data.Length) throw new ArgumentOutOfRangeException("startIndex");
            if (length < 0 || length > data.Length - startIndex) throw new ArgumentOutOfRangeException("length");

            if (length == 0)
            {
                return "";
            }

            int dataLen = checked(length * 4);
            int allocationSize = checked(dataLen + 1);

            if (Environment.OSVersion.Platform == PlatformID.Unix)
            {
                // モジュール初期化
                Init();
            }

            GCHandle gcData = default(GCHandle);
            IntPtr outStrPtr = IntPtr.Zero;
            try
            {
                gcData = GCHandle.Alloc(data, GCHandleType.Pinned);
                IntPtr inStrPtr = IntPtr.Add(gcData.AddrOfPinnedObject(), startIndex);

                outStrPtr = Marshal.AllocHGlobal(allocationSize);

                int lpBytesReturned;
                bool result = NkfConvertSafe(outStrPtr, dataLen, out lpBytesReturned, inStrPtr, length, "-w");
                if (!result)
                {
                    throw new InvalidOperationException("Native nkf conversion failed.");
                }
                if (lpBytesReturned < 0 || lpBytesReturned > dataLen)
                {
                    throw new InvalidOperationException("Native nkf conversion returned an invalid output length.");
                }

                byte[] strData = new byte[lpBytesReturned];
                Marshal.Copy(outStrPtr, strData, 0, lpBytesReturned);
                return System.Text.Encoding.UTF8.GetString(strData, 0, lpBytesReturned);
            }
            finally
            {
                if (outStrPtr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(outStrPtr);
                }
                if (gcData.IsAllocated)
                {
                    gcData.Free();
                }
            }
        }

        /// <summary>
        /// スタティックコンストラクタ。
        /// このクラスが最初に利用されるタイミングで1回だけ実行される。
        /// </summary>
#pragma warning disable CA1416 // プラットフォームの互換性を検証
        static NativeMethods()
        {
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                if (FindInSearchPath(nkfdll) == false)
                {
                    string originalAssemblypath = new Uri(Assembly.GetExecutingAssembly().EscapedCodeBase).LocalPath;

                    string currentArchSubPath = ProcessorArchitecture;

                    string path = Path.Combine(Path.GetDirectoryName(originalAssemblypath), currentArchSubPath);

                    // DLL 読み込みディレクトリを追加する方法
                    // （Win7 以降では AddDllDirectory を使う） 
                    IntPtr intPtr = GetProcAddress(GetModuleHandle("kernel32.dll"), "AddDllDirectory");
                    if (intPtr == IntPtr.Zero)
                    {
                        SetDllDirectory(path);
                    }
                    else
                    {
                        AddDllDirectory(path);
                    }
                }
            } 
            else if (Environment.OSVersion.Platform == PlatformID.Unix)
            {
                // 初期化
                Init();
            }
        }
#pragma warning restore CA1416 // プラットフォームの互換性を検証

        /// <summary>
        /// IntPtr のサイズを調べることで x86 x64 を判定する。
        /// </summary>
        const int IntPtrSize_x64 = 8;
        const int IntPtrSize_x86 = 4;

        public static string ProcessorArchitecture
        {
            get
            {
                bool isX64 = false;
                if (IntPtr.Size == IntPtrSize_x64)
                {
                    isX64 = true;
                }
                if (isX64)
                {
                    return "x64";
                }

                return "x86";
            }
        }


        /// <summary>
        /// Path の中で目的の EXE または DLL が見つかるか？判定する。
        /// </summary>
        /// <param name="exeOrDllName">
        /// 検索するディレクトリ名を指定
        /// </param>
        /// <remarks>
        /// Win32 API Search Path を使って 目的のファイルを検索する。
        /// </remarks>
        /// <returns>
        /// 検索できたら true を返す。
        /// 見つからない場合は false を返す。
        /// </returns>
#if NET5_0_OR_GREATER
        [SupportedOSPlatform("windows")]
#endif
        private static bool FindInSearchPath(string exeOrDllName)
        {
            string findFileName = exeOrDllName;

            StringBuilder sb = new StringBuilder(260);
            IntPtr ptr = new IntPtr();

            uint getStrLength = SearchPath(null,
                    findFileName,
                    null,
                    sb.Capacity,
                    sb,
                    out ptr);

            if (getStrLength > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

#if NET5_0_OR_GREATER
        [SupportedOSPlatform("windows")]
#endif
        // http://msdn.microsoft.com/en-us/library/windows/desktop/ms684175(v=vs.85).aspx
        [DllImport("kernel32",
          CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Auto,
          BestFitMapping = false,
          ThrowOnUnmappableChar = true,
          SetLastError = true)]
        private static extern IntPtr LoadLibrary(
            [MarshalAs(UnmanagedType.LPStr)]string lpFileName);

        /*
        [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Ansi)]
        static extern IntPtr LoadLibrary([MarshalAs(UnmanagedType.LPStr)]string lpFileName);
        */
#if NET5_0_OR_GREATER
        [SupportedOSPlatform("windows")]
#endif
        // http://msdn.microsoft.com/en-us/library/windows/desktop/aa365527(v=vs.85).aspx
        [DllImport("kernel32.dll",
            SetLastError = true,
            BestFitMapping = false, ThrowOnUnmappableChar = true,
            CharSet = CharSet.Auto)]
        public static extern uint SearchPath(string lpPath,
                            string lpFileName,
                            string lpExtension,
                            int nBufferLength,
                            [MarshalAs(UnmanagedType.LPTStr)]StringBuilder lpBuffer,
                            out IntPtr lpFilePart);
#if NET5_0_OR_GREATER
        [SupportedOSPlatform("windows")]
#endif
        [DllImport("kernel32", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
        static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

#if NET5_0_OR_GREATER
        [SupportedOSPlatform("windows")]
#endif
        // https://docs.microsoft.com/en-us/windows/win32/api/libloaderapi/nf-libloaderapi-adddlldirectory
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetDllDirectory(string lpPathName);

#if NET5_0_OR_GREATER
        [SupportedOSPlatform("windows")]
#endif
        // https://docs.microsoft.com/en-us/windows/win32/api/libloaderapi/nf-libloaderapi-adddlldirectory
        // DLL_DIRECTORY_COOKIE AddDllDirectory(PCWSTR NewDirectory);
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern int AddDllDirectory(string lpPathName);

#if NET5_0_OR_GREATER
        [SupportedOSPlatform("windows")]
#endif
        // https://docs.microsoft.com/en-us/windows/win32/api/libloaderapi/nf-libloaderapi-getmodulehandlew
        // HMODULE GetModuleHandleW(LPCWSTR lpModuleName);
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern IntPtr GetModuleHandle(string lpModuleName);
    }
}
