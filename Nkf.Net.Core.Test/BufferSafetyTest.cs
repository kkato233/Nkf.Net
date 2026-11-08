using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Nkf.Net.Core.Test
{
    [TestClass]
    public class BufferSafetyTest
    {
        [DataTestMethod]
        [DataRow(false)]
        [DataRow(true)]
        public void SafeConversionRejectsInvalidArraysAndLengths(bool withOption)
        {
            byte[] input = { 65, 66, 67, 68 };
            byte[] output = new byte[8];

            Assert.AreEqual("outStr", Assert.ThrowsException<ArgumentNullException>(
                () => Convert(withOption, null!, 0, input, 1)).ParamName);
            Assert.AreEqual("data", Assert.ThrowsException<ArgumentNullException>(
                () => Convert(withOption, output, output.Length, null!, 0)).ParamName);

            foreach (int length in new[] { -1, int.MinValue, output.Length + 1, int.MaxValue })
            {
                Assert.AreEqual("length", Assert.ThrowsException<ArgumentOutOfRangeException>(
                    () => Convert(withOption, output, length, input, input.Length)).ParamName);
            }
            foreach (int count in new[] { -1, int.MinValue, input.Length + 1, int.MaxValue })
            {
                Assert.AreEqual("len", Assert.ThrowsException<ArgumentOutOfRangeException>(
                    () => Convert(withOption, output, output.Length, input, count)).ParamName);
            }
        }

        private static bool Convert(bool withOption, byte[] output, int capacity, byte[] input, int count)
        {
            int bytes;
            if (withOption)
            {
                return WrapNkf.NkfConvertSafeWithOption(output, capacity, out bytes, input, count, "-w");
            }
            return WrapNkf.NkfConvertSafe(output, capacity, out bytes, input, count);
        }

        [TestMethod]
        public void StringConversionRejectsNullInput()
        {
            Assert.AreEqual("buffer", Assert.ThrowsException<ArgumentNullException>(
                () => WrapNkf.NkfConvert(null!, 0, 0)).ParamName);
            Assert.AreEqual("data", Assert.ThrowsException<ArgumentNullException>(
                () => NkfEncoding.NkfConvert(null!, 0, 0)).ParamName);
        }

        [DataTestMethod]
        [DataRow(-1, 1, "startIndex")]
        [DataRow(int.MinValue, 0, "startIndex")]
        [DataRow(5, 0, "startIndex")]
        [DataRow(int.MaxValue, 1, "startIndex")]
        [DataRow(0, -1, "length")]
        [DataRow(0, int.MinValue, "length")]
        [DataRow(0, 5, "length")]
        [DataRow(1, int.MaxValue, "length")]
        [DataRow(4, 1, "length")]
        public void StringConversionRejectsInvalidSlices(int startIndex, int length, string parameter)
        {
            byte[] input = { 65, 66, 67, 68 };
            Assert.AreEqual(parameter, Assert.ThrowsException<ArgumentOutOfRangeException>(
                () => WrapNkf.NkfConvert(input, startIndex, length)).ParamName);
            Assert.AreEqual(parameter, Assert.ThrowsException<ArgumentOutOfRangeException>(
                () => NkfEncoding.NkfConvert(input, startIndex, length)).ParamName);
            Assert.AreEqual(parameter == "startIndex" ? "index" : "count",
                Assert.ThrowsException<ArgumentOutOfRangeException>(
                    () => new NkfEncoding().GetCharCount(input, startIndex, length)).ParamName);
        }

        [TestMethod]
        public void GetCharsRejectsNullArrays()
        {
            NkfEncoding encoding = new NkfEncoding();
            Assert.AreEqual("bytes", Assert.ThrowsException<ArgumentNullException>(
                () => encoding.GetChars(null!, 0, 0, new char[4], 0)).ParamName);
            Assert.AreEqual("chars", Assert.ThrowsException<ArgumentNullException>(
                () => encoding.GetChars(new byte[4], 0, 0, null!, 0)).ParamName);
            Assert.AreEqual("bytes", Assert.ThrowsException<ArgumentNullException>(
                () => encoding.GetCharCount(null!, 0, 0)).ParamName);
        }

        [DataTestMethod]
        [DataRow(-1, 1, 0, "byteIndex")]
        [DataRow(int.MinValue, 0, 0, "byteIndex")]
        [DataRow(5, 0, 0, "byteIndex")]
        [DataRow(int.MaxValue, 1, 0, "byteIndex")]
        [DataRow(0, -1, 0, "byteCount")]
        [DataRow(0, int.MinValue, 0, "byteCount")]
        [DataRow(0, 5, 0, "byteCount")]
        [DataRow(1, int.MaxValue, 0, "byteCount")]
        [DataRow(4, 1, 0, "byteCount")]
        [DataRow(0, 1, -1, "charIndex")]
        [DataRow(0, 1, int.MinValue, "charIndex")]
        [DataRow(0, 1, 5, "charIndex")]
        [DataRow(0, 1, int.MaxValue, "charIndex")]
        public void GetCharsRejectsInvalidRanges(int byteIndex, int byteCount, int charIndex, string parameter)
        {
            NkfEncoding encoding = new NkfEncoding();
            byte[] input = { 65, 66, 67, 68 };
            char[] output = new char[4];
            Assert.AreEqual(parameter, Assert.ThrowsException<ArgumentOutOfRangeException>(
                () => encoding.GetChars(input, byteIndex, byteCount, output, charIndex)).ParamName);
        }

        [TestMethod]
        public void EmptySlicesAreValidAtArrayEnd()
        {
            foreach (byte[] input in new[] { new byte[0], new byte[] { 65, 66 } })
            {
                Assert.AreEqual("", WrapNkf.NkfConvert(input, input.Length, 0));
                Assert.AreEqual("", NkfEncoding.NkfConvert(input, input.Length, 0));
                char[] chars = new char[4];
                Assert.AreEqual(0, new NkfEncoding().GetChars(input, input.Length, 0, chars, chars.Length));
                Assert.AreEqual(0, new NkfEncoding().GetCharCount(input, input.Length, 0));
            }

            int count;
            Assert.IsTrue(WrapNkf.NkfConvertSafeWithOption(new byte[0], 0, out count, new byte[0], 0, "-w"));
            Assert.AreEqual(0, count);
            Assert.AreEqual(0, new NkfEncoding().GetChars(new byte[0], 0, 0, new char[0], 0));
        }

        [TestMethod]
        public void ConversionRespectsSliceAndOutputBounds()
        {
            const string expected = "a\u3042\u3044\u3046z";
            byte[] input = Encoding.UTF8.GetBytes("prefix" + expected + "suffix");
            int count = Encoding.UTF8.GetByteCount(expected);
            Assert.AreEqual(expected, WrapNkf.NkfConvert(input, 6, count));
            Assert.AreEqual(expected, NkfEncoding.NkfConvert(input, 6, count));

            char[] chars = new string('#', expected.Length + 1).ToCharArray();
            Assert.AreEqual(expected.Length, new NkfEncoding().GetChars(input, 6, count, chars, 1));
            Assert.AreEqual('#', chars[0]);
            Assert.AreEqual(expected, new string(chars, 1, expected.Length));
            Assert.AreEqual(expected.Length, new NkfEncoding().GetCharCount(input, 6, count));

            byte[] output = { 0xCC, 0xCC, 0xCC, 0xCC };
            int written;
            Assert.IsTrue(WrapNkf.NkfConvertSafeWithOption(output, 1, out written, new byte[] { 65, 66 }, 1, "-w"));
            Assert.AreEqual(1, written);
            CollectionAssert.AreEqual(new byte[] { 65, 0xCC, 0xCC, 0xCC }, output);
        }

        [TestMethod]
        public void StaticConversionPreservesEmbeddedNulls()
        {
            byte[] input = { 65, 0, 66 };
            Assert.AreEqual("A\0B", NkfEncoding.NkfConvert(input, 0, input.Length));
        }

        [TestMethod]
        public void StaticConversionSurvivesCompactingGc()
        {
            string expected = new string('A', 16 * 1024) + "\u3042\u3044\u3046";
            byte[] template = Encoding.UTF8.GetBytes("_" + expected + "_");
            NkfEncoding encoding = new NkfEncoding();
            using (CancellationTokenSource stop = new CancellationTokenSource())
            using (ManualResetEventSlim started = new ManualResetEventSlim())
            {
                Task collector = Task.Run(() =>
                {
                    GC.Collect(2, GCCollectionMode.Forced, true, true);
                    started.Set();
                    while (!stop.IsCancellationRequested)
                    {
                        GC.Collect(2, GCCollectionMode.Forced, true, true);
                        Thread.Yield();
                    }
                });
                try
                {
                    Assert.IsTrue(started.Wait(TimeSpan.FromSeconds(10)));
                    for (int i = 0; i < 100; i++)
                    {
                        byte[] input = (byte[])template.Clone();
                        Assert.AreEqual(expected, NkfEncoding.NkfConvert(input, 1, input.Length - 2));
                        Assert.AreEqual(expected, WrapNkf.NkfConvert(input, 1, input.Length - 2));
                        char[] chars = new char[expected.Length + 1];
                        Assert.AreEqual(expected.Length, encoding.GetChars(input, 1, input.Length - 2, chars, 1));
                        Assert.AreEqual(expected, new string(chars, 1, expected.Length));
                    }
                }
                finally
                {
                    stop.Cancel();
                    collector.GetAwaiter().GetResult();
                }
            }
        }
    }
}
