using System;
using System.Collections.Generic;
using System.Text;

namespace Nkf.Net
{
    /// <summary>
    /// NKFを呼び出す自動的にエンコード判定を行うTextReader
    /// </summary>
    public class NkfTextReader : System.IO.TextReader
    {
        System.IO.Stream _disposeStream = null;
        System.IO.Stream _st = null;
        /// <summary>
        /// ファイル名を指定して 内容のデータを取得する
        /// </summary>
        /// <param name="fileName"></param>
        public NkfTextReader(string fileName)
        {
            _disposeStream = new System.IO.FileStream(fileName,
                System.IO.FileMode.Open, System.IO.FileAccess.Read);

            _st = _disposeStream;
        }

        /// <summary>
        /// Stream からデータを読み取る
        /// </summary>
        /// <param name="st"></param>
        public NkfTextReader(System.IO.Stream st)
        {
            _st = st;
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

            this.currentNkfOption = sb.ToString();
        }
        private string currentNkfOption = "";

        /// <summary>
        /// ファイルが終了したか？
        /// </summary>
        bool Eof = false;

        Queue<string> lineBuffer = new Queue<string>();
        List<byte> dataBuffer = new List<byte>();
        /// <summary>
        /// 1行のデータを取得する
        /// </summary>
        /// <returns></returns>
        public override string ReadLine()
        {
            if (lineBuffer.Count > 0)
            {
                string s = lineBuffer.Dequeue();
                return s;
            }

            // 次の1行を読み込む
            if (Eof)
            {
                return null;
            }

            byte[] buffer = new byte[4096];
            int read_n = _st.Read(buffer, 0, buffer.Length);
            if (read_n == 0 && dataBuffer.Count == 0)
            {
                // ファイルが終わり
                Eof = true;
                return null;
            }

            // データの中の最後の \n の場所を見つける

            List<byte> tmpBuffer = new List<byte>();
            int find_ln_count = 0;
            for (int i = 0; i < read_n; i++)
            {
                tmpBuffer.Add(buffer[i]);
                if (buffer[i] == '\n') { 
                    dataBuffer.AddRange(tmpBuffer);
                    tmpBuffer.Clear();
                    find_ln_count++;
                }
            }
            if (find_ln_count == 0 && tmpBuffer.Count > 0)
            {
                dataBuffer.AddRange(tmpBuffer);
                tmpBuffer.Clear();
            }

            // dataBuffer にあるデータを文字列に変換する
            string sData = WrapNkf.NkfConvert(dataBuffer.ToArray(), 0, dataBuffer.Count, this.currentNkfOption);
            
            // 未処理データを dataBuffer に蓄積
            dataBuffer.Clear();
            dataBuffer.AddRange(tmpBuffer);

            // 文字列を改行コードで区切って読み込み
            using (System.IO.StringReader sr = new System.IO.StringReader(sData))
            {
                string ss = sr.ReadLine();
                while (ss != null)
                {
                    lineBuffer.Enqueue(ss);
                    ss = sr.ReadLine();
                }
            }

            // バッファーから1行だけ返す
            if (lineBuffer.Count > 0)
            {
                return lineBuffer.Dequeue();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 文字列を最後まで読み込む
        /// </summary>
        /// <returns></returns>
        public override string ReadToEnd()
        {
            List<string> line = new List<string>();
            string s = ReadLine();
            while (s != null)
            {
                line.Add(s);
                s = ReadLine();
            }

            return string.Join("\r\n", line.ToArray());
        }

        public override int Peek()
        {
            throw new NotImplementedException("この関数は利用できません。");
        }

        public override int Read()
        {
            throw new NotImplementedException("この関数は利用できません。");
        }

        public override int Read(char[] buffer, int index, int count)
        {
            throw new NotImplementedException("この関数は利用できません。");
        }
        public override int ReadBlock(char[] buffer, int index, int count)
        {
            throw new NotImplementedException("この関数は利用できません。");
        }
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                if (_disposeStream != null)
                {
                    _disposeStream.Dispose();
                    _disposeStream = null;
                }
            }
        }

    }
}
