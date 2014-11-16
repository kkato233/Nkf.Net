using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Nkf.Net
{
    /// <summary>
    /// nkf32.dll の機能を .NET から利用するための簡単なラッパークラス。
    /// </summary>
    /// <remarks>
    /// （１） 複数のスレッドからアクセスされた場合に処理が終了するまで
    ///         別スレッドを待たせる必要がある。
    /// （２） SetNkfOption は 最後に設定したもが
    /// </remarks>
    public class WrapNkf
    {
        /// <summary>
        /// nkf32.dll のバージョン情報を取得する。
        /// </summary>
        /// <returns></returns>
        public static String GetNkfVersion()
        {
            StringBuilder sb = new StringBuilder(256);
            int len;
            NativeMethods.GetNkfVersionSafe(sb, sb.Capacity, out len);

            len = Math.Min(len, sb.Length);

            if (len > 0 && sb[len - 1] == '\0')
            {
                len--;
            }
            return sb.ToString(0, len);
        }

        /// <summary>
        /// Nkf を使ったコンバート処理の呼び出し
        /// </summary>
        /// <param name="outStr"></param>
        /// <param name="length"></param>
        /// <param name="bytes"></param>
        /// <param name="data"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static bool NkfConvertSafe(byte[] outStr, int length, out int bytes, byte[] data, int len)
        {
            return NkfConvertSafeWithOption(outStr, length, out bytes, data, len, null);
        }
        
        /// <summary>
        /// Nkf を使ったコンバート処理の呼び出し。
        /// 明示的に今回だけ利用するオプションを指定できる。
        /// </summary>
        /// <param name="outStr"></param>
        /// <param name="length"></param>
        /// <param name="bytes"></param>
        /// <param name="data"></param>
        /// <param name="len"></param>
        /// <param name="nkfOption"></param>
        /// <returns></returns>
        public static bool NkfConvertSafeWithOption(byte[] outStr, int length, out int bytes, byte[] data, int len,string nkfOption)
        {
            return NkfConvertSafeByGCHandle(outStr, length, out bytes, data, len, nkfOption);
        }

        private static bool NkfConvertSafeByAllocHGlobal(byte[] outStr, int length, out int bytes, byte[] data, int len, string nkfOption)
        {
            // 最後に設定された SetNkfOption の値を利用する
            if (nkfOption != null)
            {
                NativeMethods.SetNkfOption(nkfOption);
            }
            else
            {
                SetNkfOptionAtLast();
            }

            // 指定した情報を使ってコンバート実行
            IntPtr p = Marshal.AllocHGlobal(length);
            IntPtr pData = Marshal.AllocHGlobal(len);
            Marshal.Copy(data, 0, pData, len);

            bool result = NativeMethods.NkfConvertSafe(p, length, out bytes, pData, len);

            Marshal.Copy(p, outStr, 0, bytes);

            Marshal.FreeHGlobal(p);
            Marshal.FreeHGlobal(pData);

            return result;
        }

        private static bool NkfConvertSafeByGCHandle(byte[] outStr, int length, out int bytes, byte[] data, int len, string nkfOption)
        {
            // 最後に設定された SetNkfOption の値を利用する
            if (nkfOption != null)
            {
                NativeMethods.SetNkfOption(nkfOption);
            }
            else
            {
                SetNkfOptionAtLast();
            }

            // 指定した情報を使ってコンバート実行
            GCHandle gcBytes = GCHandle.Alloc(data, GCHandleType.Pinned);
            GCHandle gcOutStr = GCHandle.Alloc(outStr, GCHandleType.Pinned);
            IntPtr inBuffer = gcBytes.AddrOfPinnedObject();
            IntPtr outBuffer = gcOutStr.AddrOfPinnedObject();
            try
            {
                int convertBytes;
                bool result = NativeMethods.NkfConvertSafe(outBuffer, length, out convertBytes, inBuffer, len);

                bytes = convertBytes;

                return result;
            }
            finally
            {
                gcBytes.Free();
                gcOutStr.Free();
            }
            
        }

        /// <summary>
        /// パラメータで指定された日本語を自動的に解析して文字列として出力する。
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="startIndex"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string NkfConvert(byte[] buffer, int startIndex, int length)
        {
            return NkfConvert(buffer, startIndex, length, null);
        }

        internal static string NkfConvert(byte[] buffer, int startIndex, int length,string nkfOption)
        {
            byte []data = new byte[length * sizeof(char) + 10];

            byte[] inData;
            if (startIndex == 0) {
                inData = buffer;
            } else {
                inData = new byte[length];
                for(int i=0;i<length;i++) {
                    inData[i] = buffer[startIndex + i];
                }
            }
            int convertBytes;
            if (nkfOption == null)
            {
                nkfOption = "-w";
            }
            else if (nkfOption.Contains("-w") == false)
            {
                nkfOption = nkfOption + " -w";
            }
            bool result = NkfConvertSafeWithOption(data, data.Length, out convertBytes, inData, length, nkfOption);

            string s = System.Text.Encoding.UTF8.GetString(data, 0, convertBytes);

            return s;
        }

        /// <summary>
        /// Nkf を呼び出すときに利用するオプションを設定
        /// </summary>
        /// <param name="option"></param>
        public static void SetNkfOption(string option)
        {
            _nkfOption = option;
        }
        /// <summary>
        /// 最後に設定された NkfOption を保存しておく。
        /// (デフォルトは UTF8）
        /// </summary>
        static string _nkfOption = "-w";

        private static void SetNkfOptionAtLast()
        {
            if (_nkfOption != null)
            {
                NativeMethods.SetNkfOption(_nkfOption);
            }
        }
        /// <summary>
        /// 指定のファイルを日本語変換を行う。
        /// </summary>
        /// <param name="fileName">ファイル名を指定する</param>
        /// <returns></returns>
        public static bool FileConvert1(string fileName)
        {
            SetNkfOptionAtLast();

            return NativeMethods.NkfFileConvert1Safe(fileName, fileName.Length + 1);
        }

        /// <summary>
        /// 入力ファイルと出力ファイルを指定してコンバート
        /// </summary>
        /// <param name="inFileName"></param>
        /// <param name="outFileName"></param>
        /// <returns></returns>
        public static bool FileConvert2(string inFileName, string outFileName)
        {
            SetNkfOptionAtLast();

            return NativeMethods.NkfFileConvert2SafeW(inFileName, inFileName.Length + 1, outFileName, outFileName.Length + 1);
        }

        public static string GetGuess()
        {
            StringBuilder sb = new StringBuilder(1024);
            int len;
            NativeMethods.GetNkfGuess(sb, sb.Capacity, out len);
            len = Math.Min(len, sb.Length);

            return sb.ToString(0, len);
        }

        public static string GetSupportFunctions()
        {
            int len;
            NativeMethods.MLocation ml = new NativeMethods.MLocation();

            bool result = NativeMethods.GetNkfSupportFunctions(ref ml, 40, out len);

            StringBuilder sb = new StringBuilder();
            if (result)
            {
                sb.Append("構造体の長さ：" + ml.size);
                sb.Append("\r\n");
                sb.Append("著作権文字列：\r\n");
                sb.Append(Marshal.PtrToStringAnsi(ml.copyrightA));
                sb.Append("\r\n");
                sb.Append("バージョン：");
                sb.Append(Marshal.PtrToStringAnsi(ml.versionA));
                sb.Append("\r\n");
                sb.Append("日付:");
                sb.Append(Marshal.PtrToStringAnsi(ml.dateA));
                sb.Append("\r\n");

                sb.Append("互換性：");
                if ((ml.functions & 0x01) != 0)
                {
                    sb.Append(" nkf32103a.lzh 1.03と互換機能あり");
                }
                if ((ml.functions & 0x02) == 0)
                {
                    sb.Append(" nkf32dll.zip 0.91と互換機能なし");
                }
                if ((ml.functions & 0x04) != 0)
                {
                    sb.Append(" nkf32204.zip 2.0.4.0と互換機能あり");
                }
                sb.Append("UNICODEのサポート：");
                if ((ml.functions & 0x80000000) != 0)
                {
                    sb.Append("あり");
                }
                else
                {
                    sb.Append("なし");
                }
            }

            return sb.ToString();
        }

        public static string Usage()
        {
            StringBuilder sb = new StringBuilder(1024 * 10);

            int len;
            if (NativeMethods.NkfUsage(sb, sb.Capacity, out len))
            {
                len = Math.Min(len, sb.Length);

                if (sb[len - 1] == '\0')
                {
                    len = len - 1;
                }

                return sb.ToString(0, len);
            }

            return "";
        }
    }
}
