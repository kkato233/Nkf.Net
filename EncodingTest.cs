using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Nkf.Net.Test
{
    [TestClass]
    public class EncodingTest
    {
        [TestMethod]
        public void Test1()
        {
            Nkf.Net.NkfEncoding enc = new NkfEncoding();
            string s = "漢字テスト";

            byte[] bUTF8 = System.Text.Encoding.UTF8.GetBytes(s);
            byte[] bSJIS = System.Text.Encoding.GetEncoding("SJIS").GetBytes(s);
            byte[] bEUC = System.Text.Encoding.GetEncoding("EUC-JP").GetBytes(s);

            string s1 = enc.GetString(bUTF8);
            string s2 = enc.GetString(bSJIS);
            string s3 = enc.GetString(bEUC);

            Assert.AreEqual(s, s1);
            Assert.AreEqual(s, s2);
            Assert.AreEqual(s, s3);
        }

        [TestMethod]
        public void TestNoError()
        {
            // 簡易実装したメソッドがエラーにならないことのチェック
            Nkf.Net.NkfEncoding enc = new NkfEncoding();

            int n0 = enc.GetByteCount("test");
            int n1 = enc.GetMaxByteCount(3);
            int n2 = enc.GetMaxCharCount(2);

            Assert.AreEqual(4, n0);
        }
    }
}
