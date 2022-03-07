using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Nkf.Net.Test
{
    [TestClass]
    public class TestWrapNkf
    {
        [TestMethod]
        public void TestUsage()
        {
            string s = WrapNkf.Usage();

            Console.WriteLine(s);

            Assert.IsTrue(s.Length > 0);

        }

        [TestMethod]
        public void TestSupportFunctions()
        {
            string sss = WrapNkf.GetSupportFunctions();
            Console.WriteLine(sss);
            Assert.IsTrue(sss.Length > 0);
        }

        [TestMethod]
        public void TestGuess()
        {
            Nkf.Net.NkfEncoding enc = new NkfEncoding();
            string s = "漢字テスト";
#if NET5_0_OR_GREATER
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
#endif
            byte[] bUTF8 = System.Text.Encoding.UTF8.GetBytes(s);
            byte[] bSJIS = System.Text.Encoding.GetEncoding("SJIS").GetBytes(s);
            byte[] bEUC = System.Text.Encoding.GetEncoding("EUC-JP").GetBytes(s);

            string s1 = WrapNkf.NkfConvert(bUTF8, 0, bUTF8.Length);
            string guess = WrapNkf.GetGuess();
            Console.WriteLine(guess);
            Assert.IsTrue(guess.Contains("UTF-8"),guess);

            string s2 = enc.GetString(bSJIS);
            guess = WrapNkf.GetGuess();
            Console.WriteLine(guess);
            Assert.IsTrue(guess.Contains("Shift_JIS"), guess);

            string s3 = enc.GetString(bEUC);
            guess = WrapNkf.GetGuess();
            Console.WriteLine(guess);
            Assert.IsTrue(guess.Contains("EUC-JP"), guess);
        }

        [TestMethod]
        public void TestSetNkfOption()
        {
            Nkf.Net.NkfEncoding enc = new NkfEncoding();
            int n1 = enc.GetMaxCharCount(3);

            string s = @"=?euc-jp?Q?=A4=A2=A4=A4=A4=A6=A4=A8=A4=AA=AA?=
=?shift_jis?Q?=82=A0=82=A2=82=A4=82=A6=82=A8?=";

            byte[] data = System.Text.Encoding.ASCII.GetBytes(s);
            enc.SetNkfOption("-mQ");

            string ss = enc.GetString(data);
            // TODO: =?euc-jp? の部分の解析は不要なんだが・・

            Console.WriteLine(ss);

            Assert.IsTrue(ss.Contains("あいうえお"));
            
            Console.WriteLine(WrapNkf.GetGuess());

        }
        [TestMethod]
        public void TestVersion()
        {
            string s = Nkf.Net.WrapNkf.GetNkfVersion();

            Console.WriteLine(s);

            Assert.IsTrue(s.Length > 0);
        }

        [TestMethod]
        public void TestNullString()
        {
            byte[] data = new byte[0];

            string ss = WrapNkf.NkfConvert(data, 0, 0);

            Assert.AreEqual("", ss);
        }

        [TestMethod]
        public void TestNkfFile()
        {
            string outFile = System.IO.Path.GetTempFileName();

            string inFile = GetTestDataFileName("euc.txt");

            Assert.IsTrue(System.IO.File.Exists(inFile));

            WrapNkf.SetNkfOption("-w");

            bool result = WrapNkf.FileConvert2(inFile, outFile);

            Assert.IsTrue(result);

            System.IO.FileInfo f = new System.IO.FileInfo(outFile);
            var tr = f.OpenText();
            string s = tr.ReadToEnd();
            Console.WriteLine(s);
            Assert.IsTrue(s.Contains("漢字コード"));
            tr.Close();

            System.IO.File.Delete(outFile);
        }


        [TestMethod]
        public void TestNkfFile2()
        {
            string outFile = System.IO.Path.GetTempFileName();

            string inFile = GetTestDataFileName("euc.txt");

            System.IO.File.Copy(inFile, outFile, overwrite: true);

            Assert.IsTrue(System.IO.File.Exists(outFile));

            WrapNkf.SetNkfOption("-w");

            bool result = WrapNkf.FileConvert1(outFile);

            // TODO: なぜか正常に処理できない・・
            Assert.IsTrue(result);

            System.IO.FileInfo f = new System.IO.FileInfo(outFile);
            var tr = f.OpenText();
            string s = tr.ReadToEnd();
            Console.WriteLine(s);
            Assert.IsTrue(s.Contains("漢字コード"));
            tr.Close();

            System.IO.File.Delete(outFile);
        }

        private string GetTestDataFileName(string file)
        {

            string fileName = "TestData" + System.IO.Path.DirectorySeparatorChar + file;
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(System.Environment.CurrentDirectory);
            if (System.IO.File.Exists(fileName) == false)
            {
                dir = dir.Parent.Parent;
                dir = new System.IO.DirectoryInfo(System.IO.Path.Combine(dir.FullName, "TestData"));

                fileName = System.IO.Path.Combine(dir.FullName, file);
            }

            return fileName;
        }

    }
}
