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

        //LPWSTR verStr,DWORD nBufferLength /*in TCHARs*/,LPDWORD lpTCHARsReturned /*in TCHARs*/

        /// <summary>
        /// GetNkfVersionSafe のラッパ
        /// </summary>
        /// <param name="varStr"></param>
        /// <param name="nBufferLength"></param>
        /// <param name="lpTCHARsReturned"></param>
        [DllImport(nkfdll,
            CallingConvention = CallingConvention.Winapi,
            CharSet = CharSet.Auto,
            BestFitMapping = true,
            ThrowOnUnmappableChar = false)]
        internal static extern void GetNkfVersionSafe(
            StringBuilder varStr,
            int nBufferLength,
            out int lpTCHARsReturned);

        // LPSTR outStr,DWORD nOutBufferLength /*in Bytes*/,LPDWORD lpBytesReturned /*in Bytes*/, LPCSTR inStr,DWORD nInBufferLength /*in Bytes*/);
        [DllImport(nkfdll)]
        internal static extern bool NkfConvertSafe(
            IntPtr outStr, 
            int nOutBufferLength,
            out int lpBytesReturned,
            IntPtr inStr, 
            int nInBufferLength);

        [DllImport(nkfdll)]
        internal static extern int NkfGetKanjiCode();

        [DllImport(nkfdll, CharSet = CharSet.Ansi)]
        internal static extern bool SetNkfOption(string option);

        // LPCWSTR fName,DWORD nBufferLength 
        [DllImport(nkfdll, CharSet = CharSet.Auto)]
        internal static extern bool NkfFileConvert1Safe(String fName,int nBufferLength);

        [DllImport(nkfdll, CharSet = CharSet.Auto)]
        internal static extern bool GetNkfGuess(
            StringBuilder outStr,
            int nBufferLength/*in TCHARs*/,
            out int lpTCHARsReturned);

        //        BOOL WINAPI GetNkfGuessW(LPWSTR outStr,DWORD nBufferLength /*in TCHARs*/,LPDWORD lpTCHARsReturned /*in TCHARs*/)

        [StructLayout(LayoutKind.Sequential, Pack=8)]
        public struct MLocation {
            public int size;
            public IntPtr copyrightA;
            public IntPtr versionA;
            public IntPtr dateA;
            public int functions;
        };

        [DllImport(nkfdll, CharSet = CharSet.Ansi)]
        internal static extern bool GetNkfSupportFunctions(
            ref MLocation outMLocation,
            int nBufferLength ,
            out int lpBytesReturned);

        //NkfUsage(LPSTR outStr,DWORD nBufferLength /*in Bytes*/,LPDWORD lpBytesReturned /*in Bytes*/)

        [DllImport(nkfdll, CharSet = CharSet.Ansi)]
        internal static extern bool NkfUsage(
            StringBuilder outStr,
            int nBufferLength/*in Bytes*/,
            out int lpBytesReturned);
        
        //BOOL WINAPI NkfFileConvert2SafeA(LPCSTR fInName,DWORD fInBufferLength /*in TCHARs*/,LPCSTR fOutName,DWORD fOutBufferLength /*in TCHARs*/)
        [DllImport(nkfdll, CharSet = CharSet.Auto)]
        internal static extern bool NkfFileConvert2Safe(
            string fInName,
            int fInBufferLength /*in TCHARs*/,
            string fOutName,
            int fOutBufferLength /*in TCHARs*/
            );

        // Linux 用初期化関数
        [DllImport(nkfdll)]
        internal static extern void Init();

        /// <summary>
        /// 任意のバイト配列を文字列に変換する
        /// </summary>
        /// <param name="data"></param>
        /// <param name="startIndex"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        internal static string NkfConvert(byte[] data, int startIndex, int length)
        {
            // 最低限のチェック
            if (data == null || startIndex > data.Length)
            {
                return null;
            }
            if (data.Length == 0 || length == 0)
            {
                return "";
            }

            if (Environment.OSVersion.Platform == PlatformID.Unix)
            {
                // モジュール初期化
                Init();
            }

            // 領域オーバーする場合は事前にカット
            if (length + startIndex > data.Length)
            {
                length = data.Length - startIndex;
            }

            // 配列の指定の位置を取得
            IntPtr inStrPtr = Marshal.UnsafeAddrOfPinnedArrayElement(data, startIndex);
            // GC されないように ピン を押す
            GCHandle gchpointData = GCHandle.FromIntPtr(inStrPtr);

            SetNkfOption("-w"); // UTF8出力

            // 戻りの領域を確保 入力の4倍の領域
            int dataLen = data.Length * 4;
            IntPtr outStrPtr = Marshal.AllocHGlobal(dataLen + 1);

            int lpBytesReturned;
            bool result = NkfConvertSafe(outStrPtr, dataLen, out lpBytesReturned, inStrPtr, length);

            // 変換結果を 文字列に変換する

            String sAns;
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
#if NETCOREAPP
                sAns = Marshal.PtrToStringUTF8(outStrPtr);
#else
                byte[] strData = new byte[lpBytesReturned];
                Marshal.Copy(outStrPtr, strData, 0, lpBytesReturned);
                sAns = System.Text.Encoding.UTF8.GetString(strData, 0, lpBytesReturned);
#endif
            }
            else
            {
                sAns = Marshal.PtrToStringAuto(outStrPtr);
            }
            
            Marshal.FreeHGlobal(outStrPtr);

            return sAns;
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
