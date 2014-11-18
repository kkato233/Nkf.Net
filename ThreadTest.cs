using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Concurrent;
using System.Threading;
using System.Collections.Generic;

namespace Nkf.Net.Test
{
    /// <summary>
    /// マルチスレッドで正常に動作することを確認する。
    /// </summary>
    [TestClass]
    public class ThreadTest
    {
        [TestMethod]
        public void Test_マルチスレッドでの動作確認_WrapNKF()
        {
            Work work1 = new Work();
            work1.bag.Add(new WorkItem() {
                CmdNo = 1,
                SetOption = "-j",
            });
            Work work2 = new Work();
            work2.bag.Add(new WorkItem() {
                CmdNo = 1,
                SetOption = "-w",
            });

            Thread th1 = new Thread(work1.WorkLoop);
            th1.IsBackground = true;
            th1.Start();

            Thread th2 = new Thread(work2.WorkLoop);
            th2.IsBackground = true;
            th2.Start();

            work1.ev.WaitOne();
            work2.ev.WaitOne();
            work1.ev.Reset();
            work2.ev.Reset();

            work2.bag.Add(new WorkItem() {
                CmdNo = 2,
                SetText = "漢字UTF8変換テスト",
            });

            work1.bag.Add(new WorkItem() {
                CmdNo = 2,
                SetText = "漢字変換テスト１",
            });

            work1.ev.WaitOne();
            work2.ev.WaitOne();

            string s1 = string.Join(" ",work1.values);
            string s2 = string.Join(" ",work2.values);

            Console.WriteLine(s1);
            Console.WriteLine(s2);

            Assert.IsTrue(s1.Contains("err:") == false);
            Assert.IsTrue(s2.Contains("err:") == false);
        }

        class WorkItem
        {
            /// <summary>
            /// 1. SetOption
            /// 2. SetText
            /// </summary>
            public int CmdNo;
            public string SetOption;
            public string SetText;
            public byte[] result;
        }

        class Work
        {
            public ConcurrentBag<WorkItem> bag = new ConcurrentBag<WorkItem>();
            public List<string> values = new List<string>();

            public System.Threading.AutoResetEvent ev = new System.Threading.AutoResetEvent(false);

            public void WorkLoop(object data)
            {
                for (; ; )
                {
                    System.Threading.Thread.Sleep(100);
                    WorkItem result;
                    if (bag.TryTake(out result))
                    {
                        // 指示された処理をする。
                        if (result.CmdNo == 1)
                        {
                            Nkf.Net.WrapNkf.SetNkfOption(result.SetOption);
                            this.values.Add("OK");
                            ev.Set();

                        }
                        else
                        {
                            byte[] d = System.Text.Encoding.UTF8.GetBytes(result.SetText);
                            byte[] outB = new byte[250];
                            int convLen;
                            Nkf.Net.WrapNkf.NkfConvertSafe(outB, 250, out convLen, d, d.Length);
                            this.values.Add("conv: " + convLen);

                            for (int i = 0; i < 100000; i++)
                            {
                                string s = Nkf.Net.WrapNkf.NkfConvert(outB, 0, convLen);
                                if (s != result.SetText)
                                {
                                    this.values.Add("err:" + s);
                                }
                            }
                            ev.Set();

                        }
                    }
                }
            }
        }
    }
}
