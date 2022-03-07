using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Nkf.Net.Core.Test
{
    [TestClass]
    public class CoreTest01
    {
        [TestMethod]
        public void GetVersion()
        {
            string ver = Nkf.Net.WrapNkf.GetNkfVersion();
            Console.WriteLine($"ver {ver}");
            // ver 2.1.5.1 2
            Assert.AreEqual("2.1.5.1 2", ver);
        }

        [TestMethod]
        public void NkfConvertTest()
        {
            string s = "漢字テスト";
#if NET5_0_OR_GREATER
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
#endif
            byte[] bUTF8 = System.Text.Encoding.UTF8.GetBytes(s);
            byte[] bSJIS = System.Text.Encoding.GetEncoding("SJIS").GetBytes(s);
            byte[] bEUC = System.Text.Encoding.GetEncoding("EUC-JP").GetBytes(s);

            string s1 = NkfEncoding.NkfConvert(bUTF8, 0, bUTF8.Length);
            string s2 = NkfEncoding.NkfConvert(bSJIS, 0, bSJIS.Length);
            string s3 = NkfEncoding.NkfConvert(bEUC, 0, bEUC.Length);

            Assert.AreEqual(s, s1);
            Assert.AreEqual(s, s2);
            Assert.AreEqual(s, s3);
        }
    }
}
