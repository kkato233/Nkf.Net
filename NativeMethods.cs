using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

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
        private const string nkfdll = "nkf32.dll";

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
        [DllImport(nkfdll, CharSet = CharSet.Unicode)]
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
        [DllImport(nkfdll, CharSet = CharSet.Unicode)]
        internal static extern bool NkfFileConvert2SafeW(
            string fInName,
            int fInBufferLength /*in TCHARs*/,
            string fOutName,
            int fOutBufferLength /*in TCHARs*/
            );       
        
        /// <summary>
        /// スタティックコンストラクタ。
        /// このクラスが最初に利用されるタイミングで1回だけ実行される。
        /// </summary>
        static NativeMethods()
        {
            if (FindInSearchPath(nkfdll) == false)
            {
                string originalAssemblypath = new Uri(Assembly.GetExecutingAssembly().EscapedCodeBase).LocalPath;

                string currentArchSubPath = "NativeBinaries/" + ProcessorArchitecture;

                string path = Path.Combine(Path.GetDirectoryName(originalAssemblypath), currentArchSubPath);


#if false
                // PATH を指定して DLL 読み込みを行う方法
                const string pathEnvVariable = "PATH";
                Environment.SetEnvironmentVariable(pathEnvVariable,
                                                    String.Format(CultureInfo.InvariantCulture, "{0}{1}{2}", path, Path.PathSeparator, Environment.GetEnvironmentVariable(pathEnvVariable)));
#endif
                // DLL 読み込みディレクトリを追加する方法
                // （Win7 以降では AddDllDirectory を使うという方法もある） 
#if true
                SetDllDirectory(path);
#endif         
#if false
                // 明示的に指定のディレクトリを読み込む その他の関連するディレクトリは読み込まない。
                LoadLibrary(Path.Combine(path, nkfdll));
#endif
            }
        }

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

        [DllImport("kernel32", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
        static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetDllDirectory(string lpPathName);

    }
}
