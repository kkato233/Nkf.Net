using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Nkf.Net.Test
{
    [TestClass]
    public class TestTextReader
    {

        [TestMethod]
        public void TestNKFFileRead()
        {
            List<string> files = new List<string>()
            {
                "euc.txt","sjis.txt"
            };

            foreach (string file in files)
            {
                string fileName = "TestData" + System.IO.Path.DirectorySeparatorChar + file;
                System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(System.Environment.CurrentDirectory);
                if (System.IO.File.Exists(fileName) == false)
                {
                    dir = dir.Parent.Parent;
                    dir = new System.IO.DirectoryInfo(System.IO.Path.Combine(dir.FullName, "TestData"));

                    fileName = System.IO.Path.Combine(dir.FullName, file);
                }

                string s = LoadFromFileByNKF(fileName);

                Assert.IsTrue(s.Contains("EUC-JP互換表現"));
            }
        }


        [TestMethod]
        public void TestNKFFileRead2()
        {
            List<string> files = new List<string>()
            {
                "euc.txt","sjis.txt"
            };

            foreach (string file in files)
            {
                string fileName = "TestData" + System.IO.Path.DirectorySeparatorChar + file;
                System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(System.Environment.CurrentDirectory);
                if (System.IO.File.Exists(fileName) == false)
                {
                    dir = dir.Parent.Parent;
                    dir = new System.IO.DirectoryInfo(System.IO.Path.Combine(dir.FullName, "TestData"));

                    fileName = System.IO.Path.Combine(dir.FullName, file);
                }
                using (var sr = new NkfTextReader(fileName))
                {
                    string s1 = sr.ReadLine();
                    string s2 = sr.ReadLine();
                    string s3 = sr.ReadLine();
                    string s4 = sr.ReadLine();

                    Assert.AreEqual("「EUC」とは - End User Computingの略。 ベンダーにより提供されたアプリケーション以外に、目的に応じてエンドユーザーが独自にアプリケーションを作成すること。 ベンダーはEUCができるようにするための基本的な...", s1);
                    Assert.AreEqual("EUC-JP - ウィキペディア - Wikipedia", s2);
                    Assert.AreEqual("CP51932はマイクロソフトがWindowsで使用しているWindows-31JのEUC-JP互換表現。実装例はInternet Explorer4.0以降、EmEditor、秀丸エディタ等。このコードはNECのPC-9800シリーズの漢字コード（9区から12区の特殊文字を除外したもの）", s3);
                    Assert.IsNull(s4);
                }
            }
        }

        [TestMethod]
        public void TestNKFFileRead3()
        {
            // 文字列が途中で泣き別れている部分の動作確認
            List<string> files = new List<string>()
            {
                "sjis-big.txt"
            };

            foreach (string file in files)
            {
                string fileName = "TestData" + System.IO.Path.DirectorySeparatorChar + file;
                System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(System.Environment.CurrentDirectory);
                if (System.IO.File.Exists(fileName) == false)
                {
                    dir = dir.Parent.Parent;
                    dir = new System.IO.DirectoryInfo(System.IO.Path.Combine(dir.FullName, "TestData"));

                    fileName = System.IO.Path.Combine(dir.FullName, file);
                }
                using (var sr = new NkfTextReader(fileName))
                {
                    for (; ; )
                    {
                        string s1 = sr.ReadLine();
                        string s2 = sr.ReadLine();
                        string s3 = sr.ReadLine();

                        if (s1 == null) break;

                        Assert.AreEqual("「EUC」とは - End User Computingの略。 ベンダーにより提供されたアプリケーション以外に、目的に応じてエンドユーザーが独自にアプリケーションを作成すること。 ベンダーはEUCができるようにするための基本的な...", s1);
                        Assert.AreEqual("EUC-JP - ウィキペディア - Wikipedia", s2);
                        Assert.AreEqual("CP51932はマイクロソフトがWindowsで使用しているWindows-31JのEUC-JP互換表現。実装例はInternet Explorer4.0以降、EmEditor、秀丸エディタ等。このコードはNECのPC-9800シリーズの漢字コード（9区から12区の特殊文字を除外したもの）", s3);
                    }
                }
            }
        }

        [TestMethod]
        public void TestMiniText()
        {

            List<string> files = new List<string>()
            {
                "mini1.txt"
            };

            foreach (string file in files)
            {
                string fileName = "TestData" + System.IO.Path.DirectorySeparatorChar + file;
                System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(System.Environment.CurrentDirectory);
                if (System.IO.File.Exists(fileName) == false)
                {
                    dir = dir.Parent.Parent;
                    dir = new System.IO.DirectoryInfo(System.IO.Path.Combine(dir.FullName, "TestData"));

                    fileName = System.IO.Path.Combine(dir.FullName, file);
                }
                using (var sr = new Nkf.Net.NkfTextReader(fileName))
                {
                    string s = sr.ReadLine();

                    Assert.IsNotNull(s);
                    Assert.IsTrue(s.Length > 1);
                    Console.WriteLine(s);

                }
            }
        }

        [TestMethod]
        public void TestNKFFileRead3B()
        {
            List<string> files = new List<string>()
            {
                "sjis-big.txt"
            };

            foreach (string file in files)
            {
                string fileName = "TestData" + System.IO.Path.DirectorySeparatorChar + file;
                System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(System.Environment.CurrentDirectory);
                if (System.IO.File.Exists(fileName) == false)
                {
                    dir = dir.Parent.Parent;
                    dir = new System.IO.DirectoryInfo(System.IO.Path.Combine(dir.FullName, "TestData"));

                    fileName = System.IO.Path.Combine(dir.FullName, file);
                }
                using (var sr = new Nkf.Net.NkfTextReader(fileName))
                {
                    for (; ; )
                    {
                        string s1 = sr.ReadLine();
                        string s2 = sr.ReadLine();
                        string s3 = sr.ReadLine();

                        if (s1 == null) break;

                        Assert.AreEqual("「EUC」とは - End User Computingの略。 ベンダーにより提供されたアプリケーション以外に、目的に応じてエンドユーザーが独自にアプリケーションを作成すること。 ベンダーはEUCができるようにするための基本的な...", s1);
                        Assert.AreEqual("EUC-JP - ウィキペディア - Wikipedia", s2);
                        Assert.AreEqual("CP51932はマイクロソフトがWindowsで使用しているWindows-31JのEUC-JP互換表現。実装例はInternet Explorer4.0以降、EmEditor、秀丸エディタ等。このコードはNECのPC-9800シリーズの漢字コード（9区から12区の特殊文字を除外したもの）", s3);
                    }
                }
            }
        }

        [TestMethod]
        public void TestNKFFileRead4()
        {
            List<string> files = new List<string>()
            {
                "sjis-big.txt"
            };

            foreach (string file in files)
            {
                string fileName = "TestData" + System.IO.Path.DirectorySeparatorChar + file;
                System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(System.Environment.CurrentDirectory);
                if (System.IO.File.Exists(fileName) == false)
                {
                    dir = dir.Parent.Parent;
                    dir = new System.IO.DirectoryInfo(System.IO.Path.Combine(dir.FullName, "TestData"));

                    fileName = System.IO.Path.Combine(dir.FullName, file);
                }
                using (var sr = new NkfTextReader(fileName))
                {
                    string s = sr.ReadToEnd();

                    Assert.IsTrue(s.Length > 10);
                    Console.WriteLine(s);

                }
            }
        }

        [TestMethod]
        public void TestNKFFileRead4B()
        {
            List<string> files = new List<string>()
            {
                "sjis-big.txt"
            };

            foreach (string file in files)
            {
                string fileName = "TestData" + System.IO.Path.DirectorySeparatorChar + file;
                System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(System.Environment.CurrentDirectory);
                if (System.IO.File.Exists(fileName) == false)
                {
                    dir = dir.Parent.Parent;
                    dir = new System.IO.DirectoryInfo(System.IO.Path.Combine(dir.FullName, "TestData"));

                    fileName = System.IO.Path.Combine(dir.FullName, file);
                }
                using (var sr = new Nkf.Net.NkfTextReader(fileName))
                {
                    string s = sr.ReadToEnd();

                    Assert.IsTrue(s.Length > 10);
                    Console.WriteLine(s);

                }
            }
        }

        private string LoadFromFileByNKF(string fileName)
        {

            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            using (System.IO.FileStream fs = new System.IO.FileStream(fileName, System.IO.FileMode.Open, System.IO.FileAccess.Read))
            {
                List<byte> data = new List<byte>();
                int ch = fs.ReadByte();
                while (ch >= 0)
                {
                    while (ch == '\r')
                    {
                        ch = fs.ReadByte();
                    }
                    data.Add((byte)ch);
                    if (ch == '\n')
                    {
                        string ss = WrapNkf.NkfConvert(data.ToArray(), 0, data.Count);

                        sb.AppendLine(ss);
                    }

                    ch = fs.ReadByte();
                }
            }

            return sb.ToString();
        }
    }
}
