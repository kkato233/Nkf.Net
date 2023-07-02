using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nkf.Net
{
    /// <summary>
    /// NKFを呼び出す自動的にエンコード判定を行うTextReader
    /// </summary>
    public class NkfTextReader : System.IO.TextReader
    {
        /// <summary>
        /// ReadLine で利用する最大バッファーサイズ
        /// </summary>
        /// <remarks>
        /// 文書の中に 改行コードが 存在しない場合 無限にメモリーを消費しないように
        /// 一定のバイト数で１行を切り取るためのバイト数。
        /// </remarks>
        public int ReadLineMaxBufferSize { get; set; } = 64 * 1024;

        /// <summary>
        /// 最後の ReadLine で利用した 改行コードの文字コード
        /// </summary>
        /// <remarks>
        /// CR LF  (Windows で主に使われる改行コード）
        /// LF     (Linux で主に使われる改行コード）
        /// CR     (Mac で主に使われる改行コード）
        /// </remarks>
        public string LastEOL { get; private set; } = string.Empty;


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

        /// <summary>
        /// 行データバッファ
        /// </summary>
        Queue<string> lineBuffer = new Queue<string>();
        /// <summary>
        /// 改行コードバッファ
        /// </summary>
        Queue<string> eolBuffer = new Queue<string>();
        /// <summary>
        /// 次に変換するデータを保持するバッファ。
        /// </summary>
        /// <remarks>
        /// 改行コード単位で変換するため 前回未変換の文字が格納されている
        /// </remarks>
        List<byte> dataBuffer = new List<byte>();
        /// <summary>
        /// 1行のデータを取得する
        /// </summary>
        /// <returns></returns>
        public override string ReadLine()
        {
            // 前回 解析分の行データが残っている場合
            if (lineBuffer.Count > 0)
            {
                string s = lineBuffer.Dequeue();
                if (eolBuffer.Any())
                {
                    LastEOL = eolBuffer.Dequeue();
                }
                return s;
            }

            // 次の1行を読み込む
            if (Eof)
            {
                return null;
            }

            // データの中の 改行 の場所を見つける

            List<byte> tmpBuffer = new List<byte>();
            
            // 前回までの解析結果保持
            if (dataBuffer.Count > 0)
            {
                tmpBuffer.AddRange(dataBuffer);
                dataBuffer.Clear();
            }

            int buffer_size = 4096;
            byte[] buffer = new byte[buffer_size+1];
            int read_n = _st.Read(buffer, 0, buffer_size);

            // 改行コードまたは 文書終了まで繰り返す
            while(true)
            {
                // ファイルの読み込み終了判定
                if (read_n == 0)
                {
                    if (tmpBuffer.Count == 0)
                    {
                        // ファイルが終わり
                        Eof = true;
                        return null;
                    } 
                    else
                    {
                        // ファイル読み込みを終了して文字列解析に行く
                        dataBuffer.AddRange(tmpBuffer);
                        tmpBuffer.Clear();
                        break;
                    }
                }
                buffer[read_n] = 0; // 文字列区切りを意図的に挿入
                int find_ln_count = 0;
                // 改行コードの場所検索
                for (int i = 0; i < read_n; i++)
                {
                    string last_eol = null;
                    if (buffer[i] == '\r' && buffer[i+1] == '\n')
                    {
                        last_eol = "\r\n"; // CR LF 改行
                        tmpBuffer.Add(buffer[i]);
                        tmpBuffer.Add(buffer[i+1]);
                        i++; // １文字読み込み SKIP
                    }
                    else if(buffer[i] == '\n')
                    {
                        last_eol = "\n"; // LF 改行
                        tmpBuffer.Add(buffer[i]);
                    }
                    else if (buffer[i] == '\r')
                    {
                        last_eol = "\r"; // CR 改行
                        tmpBuffer.Add(buffer[i]);
                    }
                    else
                    {
                        // 改行以外の文字を蓄積
                        tmpBuffer.Add(buffer[i]);
                    }

                    if (last_eol != null)
                    {
                        dataBuffer.AddRange(tmpBuffer);
                        tmpBuffer.Clear();
                        find_ln_count++;
                        eolBuffer.Enqueue(last_eol);
                    }
                }

                // dataBuffer : 改行コードまでの文字が格納
                // tmpBuffer : 改行コード以降の 未処理文字が格納

                if (find_ln_count > 0) {
                    // 今回の buffer の中に改行コードが含まれている場合
                    break;
                } 
                else if (tmpBuffer.Count > ReadLineMaxBufferSize)
                {
                    // 今回の buffer の中には 改行コードが含まれていないが あきらめて変換する場合
                    dataBuffer.AddRange(tmpBuffer);
                    tmpBuffer.Clear();
                    LastEOL = String.Empty;
                    break;
                } 

                // 続きの読み込み
                read_n = _st.Read(buffer, 0, buffer_size);
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
                if (eolBuffer.Any())
                {
                    LastEOL = eolBuffer.Dequeue();
                }
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
