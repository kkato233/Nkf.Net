using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Nkf.Net.Test
{
    /// <summary>
    /// 基本的な動作テスト
    /// </summary>
    [TestClass]
    public class NkfBaseTest
    {
        [TestMethod]
        public void Test_Convert()
        {
            LoadTestData();

            Test("JIS to JIS ...", "-j", example["jis"], example["jis"]);
            Test("JIS to SJIS...", "-s", example["jis"], example["sjis"]);
            Test("JIS to EUC ...", "-e", example["jis"], example["euc"]);
            Test("JIS to UTF8...", "-w", example["jis"], example["utf8N"]);
            Test("JIS to U16L...", "-w16L", example["jis"], example["u16L"]);
            Test("JIS to U16B...", "-w16B", example["jis"], example["u16B"]);
            Test("JIS to JIS ...", "--ic=iso-2022-jp --oc=iso-2022-jp", example["jis"], example["jis"]);
            Test("JIS to SJIS...", "--ic=iso-2022-jp --oc=shift_jis", example["jis"], example["sjis"]);
            Test("JIS to EUC ...", "--ic=iso-2022-jp --oc=euc-jp", example["jis"], example["euc"]);
            Test("JIS to UTF8...", "--ic=iso-2022-jp --oc=utf-8n", example["jis"], example["utf8N"]);
            Test("JIS to U16L...", "--ic=iso-2022-jp --oc=utf-16le-bom", example["jis"], example["u16L"]);
            Test("JIS to U16B...", "--ic=iso-2022-jp --oc=utf-16be-bom", example["jis"], example["u16B"]);

            // From SJIS

            Test("SJIS to JIS ...", "-j", example["sjis"], example["jis"]);
            Test("SJIS to SJIS...", "-s", example["sjis"], example["sjis"]);
            Test("SJIS to EUC ...", "-e", example["sjis"], example["euc"]);
            Test("SJIS to UTF8...", "-w", example["sjis"], example["utf8N"]);
            Test("SJIS to U16L...", "-w16L", example["sjis"], example["u16L"]);
            Test("SJIS to U16B...", "-w16B", example["sjis"], example["u16B"]);
            Test("SJIS to JIS ...", "--ic=shift_jis --oc=iso-2022-jp", example["sjis"], example["jis"]);
            Test("SJIS to SJIS...", "--ic=shift_jis --oc=shift_jis", example["sjis"], example["sjis"]);
            Test("SJIS to EUC ...", "--ic=shift_jis --oc=euc-jp", example["sjis"], example["euc"]);
            Test("SJIS to UTF8...", "--ic=shift_jis --oc=utf-8n", example["sjis"], example["utf8N"]);
            Test("SJIS to U16L...", "--ic=shift_jis --oc=utf-16le-bom", example["sjis"], example["u16L"]);
            Test("SJIS to U16B...", "--ic=shift_jis --oc=utf-16be-bom", example["sjis"], example["u16B"]);

            // From EUC
            
            Test("EUC to JIS ...", "-j", example["euc"], example["jis"]);
            Test("EUC to SJIS...", "-s", example["euc"], example["sjis"]);
            Test("EUC to EUC ...", "-e", example["euc"], example["euc"]);
            Test("EUC to UTF8...", "-w", example["euc"], example["utf8N"]);
            Test("EUC to U16L...", "-w16L", example["euc"], example["u16L"]);
            Test("EUC to U16B...", "-w16B", example["euc"], example["u16B"]);
            Test("EUC to JIS ...", "--ic=euc-jp --oc=iso-2022-jp", example["euc"], example["jis"]);
            Test("EUC to SJIS...", "--ic=euc-jp --oc=shift_jis", example["euc"], example["sjis"]);
            Test("EUC to EUC ...", "--ic=euc-jp --oc=euc-jp", example["euc"], example["euc"]);
            Test("EUC to UTF8...", "--ic=euc-jp --oc=utf-8n", example["euc"], example["utf8N"]);
            Test("EUC to U16L...", "--ic=euc-jp --oc=utf-16le-bom", example["euc"], example["u16L"]);
            Test("EUC to U16B...", "--ic=euc-jp --oc=utf-16be-bom", example["euc"], example["u16B"]);

            // # From UTF8
                        
            Test("UTF8 to JIS ...","-j",	example["utf8N"],example["jis"]);
            Test("UTF8 to SJIS...","-s",	example["utf8N"],example["sjis"]);
            Test("UTF8 to EUC ...","-e",	example["utf8N"],example["euc"]);
            Test("UTF8 to UTF8N..","-w",	example["utf8N"],example["utf8N"]);
            Test("UTF8 to UTF8...","-w8",	example["utf8N"],example["utf8"]);
            Test("UTF8 to UTF8N..","-w80",	example["utf8N"],example["utf8N"]);
            Test("UTF8 to U16L...","-w16L",	example["utf8N"],example["u16L"]);
            Test("UTF8 to U16L0..","-w16L0",	example["utf8N"],example["u16L0"]);
            Test("UTF8 to U16B...","-w16B",	example["utf8N"],example["u16B"]);
            Test("UTF8 to U16B0..","-w16B0",	example["utf8N"],example["u16B0"]);
            Test("UTF8 to JIS ...","--ic=utf-8 --oc=iso-2022-jp",	example["utf8N"],example["jis"]);
            Test("UTF8 to SJIS...","--ic=utf-8 --oc=shift_jis",	example["utf8N"],example["sjis"]);
            Test("UTF8 to EUC ...","--ic=utf-8 --oc=euc-jp",		example["utf8N"],example["euc"]);
            Test("UTF8 to UTF8N..","--ic=utf-8 --oc=utf-8",		example["utf8N"],example["utf8N"]);
            Test("UTF8 to UTF8BOM","--ic=utf-8 --oc=utf-8-bom",	example["utf8N"],example["utf8"]);
            Test("UTF8 to UTF8N..","--ic=utf-8 --oc=utf-8n",		example["utf8N"],example["utf8N"]);
            Test("UTF8 to U16L...","--ic=utf-8 --oc=utf-16le-bom",	example["utf8N"],example["u16L"]);
            Test("UTF8 to U16L0..","--ic=utf-8 --oc=utf-16le",		example["utf8N"],example["u16L0"]);
            Test("UTF8 to U16B...","--ic=utf-8 --oc=utf-16be-bom",	example["utf8N"],example["u16B"]);
            Test("UTF8 to U16B0..","--ic=utf-8 --oc=utf-16be",		example["utf8N"],example["u16B0"]);

            Test("UTF8 to UTF8...","-w","\xf0\xa0\x80\x8b","\xf0\xa0\x80\x8b");

            // From JIS
            
            Test("JIS  to JIS ...","-j",example["jis1"],example["jis1"]);
            Test("JIS  to SJIS...","-s",example["jis1"],example["sjis1"]);
            Test("JIS  to EUC ...","-e",example["jis1"],example["euc1"]);
            Test("JIS  to UTF8...","-w",example["jis1"],example["utf1"]);

            // From SJIS
            /*
            print "SJIS to JIS ...";&test("$nkf -j",$example{'sjis1'},$example{'jis1'});
            print "SJIS to SJIS...";&test("$nkf -s",$example{'sjis1'},$example{'sjis1'});
            print "SJIS to EUC ...";&test("$nkf -e",$example{'sjis1'},$example{'euc1'});
            print "SJIS to UTF8...";&test("$nkf -w",$example{'sjis1'},$example{'utf1'});

            // From EUC

            print "EUC  to JIS ...";&test("$nkf -j",$example{'euc1'},$example{'jis1'});
            print "EUC  to SJIS...";&test("$nkf -s",$example{'euc1'},$example{'sjis1'});
            print "EUC  to EUC ...";&test("$nkf -e",$example{'euc1'},$example{'euc1'});
            print "EUC  to UTF8...";&test("$nkf -w",$example{'euc1'},$example{'utf1'});

            // From UTF8
        
            print "UTF8 to JIS ...";&test("$nkf -j",$example{'utf1'},$example{'jis1'});
            print "UTF8 to SJIS...";&test("$nkf -s",$example{'utf1'},$example{'sjis1'});
            print "UTF8 to EUC ...";&test("$nkf -e",$example{'utf1'},$example{'euc1'});
            print "UTF8 to UTF8...";&test("$nkf -w",$example{'utf1'},$example{'utf1'});
            */
        
        }
        private void Test(string title, string nkfOption, string sIn, string sOut)
        {
            List<byte> dataIn = new List<byte>();
            dataIn.AddRange(System.Text.Encoding.ASCII.GetBytes(sIn));
            List<byte> dataOut = new List<byte>();
            dataOut.AddRange(System.Text.Encoding.ASCII.GetBytes(sOut));
            Test(title, nkfOption, dataIn, dataOut);
        }
        private void Test(string title, string nkfOption, List<byte> dataIn, List<byte> dataOut)
        {
            int dataSize = Math.Max(dataIn.Count,dataOut.Count) * 5;
            byte []data = new byte[dataSize];
            int convertLen ;
            Nkf.Net.WrapNkf.SetNkfOption(nkfOption);
            bool result = Nkf.Net.WrapNkf.NkfConvertSafe(data,dataSize,out convertLen,dataIn.ToArray(),dataIn.Count);
            Assert.IsTrue(result,title);
            if (convertLen >= dataOut.Count)
            {
                // オーバー領域は ０ であることのチェック
                for (int i = dataOut.Count; i < convertLen; i++)
                {
                    if (data[i] != 0)
                    {
                        Assert.Fail();
                    }
                }
            }
            for (int i = 0; i < Math.Min(convertLen,dataOut.Count); i++)
            {
                if (data[i] != dataOut[i])
                {
                    Assert.Fail(title);
                }
            }
        }

        private void LoadTestData()
        {
            example.Clear();

            using (System.IO.StreamReader sr = new System.IO.StreamReader(GetTestDataFileName()))
            {
                string s = sr.ReadLine();
                while (s != null)
                {
                    if (s.StartsWith("@"))
                    {
                        string key = s.Substring(1);
                        s = sr.ReadLine().Trim();
                        List<Byte> bytes = new List<byte>();
                        while (s == "")
                        {
                            s = sr.ReadLine().Trim();
                        }
                        while (string.IsNullOrEmpty(s) == false)
                        {
                            bytes.AddRange(UUDecode(s));
                            s = sr.ReadLine();
                        }
                        example[key] = bytes;
                    }

                    s = sr.ReadLine();
                }
            }
        }

        private Dictionary<string, List<Byte>> example = new Dictionary<string, List<byte>>(StringComparer.OrdinalIgnoreCase);

        private IEnumerable<byte> UUDecode(string s)
        {
            UUCodec.UUCodec dec = new UUCodec.UUCodec();

            return dec.EncodeLine(s);

            List<byte> inBytes = new List<byte>();
            for (int i = 0; i < s.Length; i++)
            {
                inBytes.Add((byte)s[i]);
            }

            byte[] outBytes = new byte[s.Length * 2];
            int convertLen = dec.DecodeLine(inBytes.ToArray(),inBytes.Count/2,outBytes);

            List<byte> ans = new List<byte>();
            for (int i = 0; i < convertLen; i++)
            {
                ans.Add(outBytes[i]);
            }
            return ans;
                for (int i = 0; i < s.Length; i++)
                {
                    char c = s[i];

                    //int b = ((c) - ' ') & 077;
                    int b = ((char)(((int)c - 32) * 4 + (c - 32) / 16));

                    ans.Add((byte)b);
                }
            return ans;
        }

        private string GetTestDataFileName()
        {
            string file = "nkfTestData.txt";
            string fileName = "TestData\\" + file;
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
