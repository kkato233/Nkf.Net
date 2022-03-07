using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Nkf.Net
{
    /// <summary>
    /// 自動的に日本語文字コードを認識して文字列に変換してくれるエンコーダー
    /// 任意の文字コードのバイト配列 -> 文字列 への変換は nkf32.dll を利用します。
    /// 文字列 -> バイト配列 の変換は UTF8 を利用します。
    /// </summary>
    public class NkfEncoding : Encoding
    {
        private Encoding _baseEncoding = Encoding.UTF8;

        public NkfEncoding()
            :base()
        {
            this.currentNkfOption = defaultNkfOption;
        }

        public NkfEncoding(string nkfOption)
            : base()
        {
            if (nkfOption == null)
            {
                this.currentNkfOption = defaultNkfOption;
                return;
            }
            SetNkfOption(nkfOption);
        }

        /// <summary>
        /// 解析で利用する NKF のオプションを指定する。
        /// ただし、出力エンコード指定 -j -e -s -w は指定しても無視される。
        /// </summary>
        /// <param name="nkfOption"></param>
        public void SetNkfOption(string nkfOption)
        {
            string[] options = nkfOption.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            System.Text.StringBuilder sb = new StringBuilder();

            // -w と競合するオプションは取り除く
            List<String> optionList = new List<string>();
            foreach (string s in options)
            {
                if (s.StartsWith("-j") ||
                    s.StartsWith("-e") ||
                    s.StartsWith("-s") ||
                    s.StartsWith("-w") ||
                    s.StartsWith("-g") ||   // コードの情報表示
                    s.StartsWith("-v")      // バージョン表示
                    )
                {
                    // 対象外
                    continue;
                }

                if (sb.Length > 0)
                {
                    sb.Append(" ");
                }

                sb.Append(s);
            }
            if (sb.Length > 0)
            {
                sb.Append(" ");
            }
            sb.Append(defaultNkfOption);

            this.currentNkfOption = sb.ToString();
        }
        /// <summary>
        /// UCS2 の BOM なし エンコード
        /// </summary>
        private const string defaultNkfOption = "-w16L0";

        private string currentNkfOption = "";

        /// <summary>
        /// バイト配列を文字列に変換する
        /// </summary>
        /// <param name="data"></param>
        /// <param name="startIndex"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string NkfConvert(byte[] data, int startIndex, int length)
        {
            string s = NativeMethods.NkfConvert(data, startIndex, length);

            return s;
        }
        /// <summary>
        /// 指定した文字配列に格納されているすべての文字をエンコードすることによって
        /// 生成されるバイト数を計算します。
        /// EncodingUTF8 と同じ動作をします。
        /// </summary>
        /// <param name="chars"></param>
        /// <param name="index"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public override int GetByteCount(char[] chars, int index, int count)
        {
            return this._baseEncoding.GetByteCount(chars, index, count);
        }

        /// <summary>
        /// 指定した文字配列に格納されている文字のセットを、指定したバイト配列にエンコードします。
        /// </summary>
        /// <param name="chars">
        /// エンコード対象の文字のセットを格納している文字配列。
        /// </param>
        /// <param name="charIndex">
        /// エンコードする最初の文字のインデックス。
        /// </param>
        /// <param name="charCount">
        /// エンコードする文字数。
        /// </param>
        /// <param name="bytes">
        /// 結果のバイト シーケンスを格納するバイト配列。
        /// </param>
        /// <param name="byteIndex">
        /// 結果のバイト シーケンスを書き込む開始位置のインデックス。
        /// </param>
        /// <returns>
        /// bytes に書き込まれた実際のバイト数。
        /// </returns>
        public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex)
        {
            return this._baseEncoding.GetBytes(chars, charIndex, charCount, bytes, byteIndex);

            if (chars == null) throw new ArgumentNullException("chars");
            if (bytes == null) throw new ArgumentNullException("chars");
            if (byteIndex <0 || byteIndex > bytes.Length) throw new ArgumentOutOfRangeException("byteIndex");
            if (charIndex <0 || charIndex > chars.Length) throw new ArgumentOutOfRangeException("charIndex");

            GCHandle gcChars = GCHandle.Alloc(chars, GCHandleType.Pinned);
            GCHandle gcBytes = GCHandle.Alloc(bytes, GCHandleType.Pinned);

            IntPtr outBuffer = AddIndex(gcBytes.AddrOfPinnedObject(),byteIndex);
            
            // 入力文字
            //return this._baseEncoding.GetBytes(chars, charIndex, charCount, bytes, byteIndex);
        }

        /// <summary>
        /// 非推奨：内部的には文字列変換してその文字数をカウントしているので性能が良くありません。
        /// 
        /// 指定したバイト配列に格納されているバイト シーケンスをデコードすることによって生成される文字数を計算します。
        /// </summary>
        /// <param name="bytes">デコード対象のバイト シーケンスが格納されたバイト配列。</param>
        /// <param name="index">デコードする最初のバイトのインデックス。</param>
        /// <param name="count">デコードするバイト数。</param>
        /// <returns>指定したバイト シーケンスをデコードすることによって生成される文字数。</returns>
        public override int GetCharCount(byte[] bytes, int index, int count)
        {
            int workSize = this.GetMaxCharCount(count);
            char[] workChars = new char[workSize + 1];
            int result = this.GetChars(bytes, index, count, workChars, 0);

            return result;
        }

        /// <summary>
        /// 指定したバイト配列に格納されているバイト シーケンスを文字のセットにデコードします。
        /// </summary>
        /// <param name="bytes">デコード対象のバイト シーケンスが格納されたバイト配列。</param>
        /// <param name="byteIndex">デコードする最初のバイトのインデックス。</param>
        /// <param name="byteCount">デコードするバイト数。</param>
        /// <param name="chars">結果の文字のセットを格納する文字配列。</param>
        /// <param name="charIndex">結果の文字のセットを書き込む開始位置のインデックス。</param>
        /// <returns>chars に書き込まれた実際の文字数。</returns>
        /// <exception cref="System.ArgumentNullException">bytes は null なので、 または chars は null </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// byteIndex、byteCount、または charIndex が 0 未満です。
        /// または byteindex および byteCount がbytes 内の有効な範囲を示していません。
        /// または charIndex が chars の有効なインデックスではありません。
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// chars には、charIndex から配列の末尾までに十分なサイズがなく、結果の文字を格納できません。
        /// </exception>
        public override int GetChars(
            byte[] bytes, int byteIndex, int byteCount,
            char[] chars, int charIndex)
        {

            if (chars == null) throw new ArgumentNullException("chars");
            if (bytes == null) throw new ArgumentNullException("chars");
            if (byteIndex < 0 || byteIndex > bytes.Length) throw new ArgumentOutOfRangeException("byteIndex");
            if (charIndex < 0 || charIndex > chars.Length) throw new ArgumentOutOfRangeException("charIndex");
            if (bytes.Length < byteIndex + byteCount) throw new ArgumentOutOfRangeException(" byteindex および byteCount がbytes 内の有効な範囲を示していません。");

            int workLength = (chars.Length - charIndex) * 3 + 5;

            GCHandle gcBytes = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            byte[] workBytes = new byte[workLength];
            GCHandle gcWorkBytes = GCHandle.Alloc(workBytes, GCHandleType.Pinned);
            GCHandle gcChars = GCHandle.Alloc(chars, GCHandleType.Pinned);

            try
            {
                IntPtr inBuffer = AddIndex(gcBytes.AddrOfPinnedObject(), byteIndex);

                int convertBytes;
#if false
                IntPtr outBuffer = gcWorkBytes.AddrOfPinnedObject();
                NativeMethods.SetNkfOption("-mQ -w");
                
                // UTF8 変換
                bool result = NativeMethods.NkfConvertSafe(outBuffer, workLength, out convertBytes, inBuffer, byteCount);
                // UTF8->char 変換
                int writeCharCount = this._baseEncoding.GetChars(workBytes, 0, convertBytes, chars, charIndex);
                if (result)
                {

                    return writeCharCount;
                }
#else
                IntPtr outBuffer = AddIndex(gcChars.AddrOfPinnedObject(),charIndex * sizeof(char));

                // コード変換オプション指定
                NativeMethods.SetNkfOption(this.currentNkfOption);

                workLength = (chars.Length - charIndex) * sizeof(char);
                // UTC2 変換
                bool result = NativeMethods.NkfConvertSafe(outBuffer, workLength, out convertBytes, inBuffer, byteCount);
                if (result)
                {
                    return convertBytes / sizeof(char);
                }
#endif
                return 0;
            }
            finally
            {
                gcChars.Free();
                gcBytes.Free();
                gcWorkBytes.Free();
            }
        }

        IntPtr AddIndex(IntPtr ptr,int bytes)
        {
#if NET20
            unsafe
            {
                byte* p = (byte*)ptr.ToPointer();
                p = p + bytes;
                IntPtr result = new IntPtr(p);

                return result;
            }
#else
            IntPtr result = ptr + bytes;
            return result;
#endif
        }

        /// <summary>
        /// 指定した文字数をエンコードすることによって生成される最大バイト数を計算します。
        /// </summary>
        /// <param name="charCount">エンコードする文字数。</param>
        /// <returns></returns>
        public override int GetMaxByteCount(int charCount)
        {
            return this._baseEncoding.GetMaxByteCount(charCount);
        }

        /// <summary>
        /// 指定したバイト数をデコードすることによって生成される最大文字数を計算します。
        /// </summary>
        /// <param name="byteCount">デコードするバイト数。</param>
        /// <returns>指定したバイト数をデコードすることによって生成される最大文字数。</returns>
        public override int GetMaxCharCount(int byteCount)
        {
            // デコードによって文字数が増えることは無いため・・

            return Math.Max(0, byteCount);  // ０ 以下の場合は ０ を返す
        }
    }
}
