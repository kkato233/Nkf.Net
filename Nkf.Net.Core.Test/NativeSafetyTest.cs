using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Nkf.Net.Core.Test
{
    [TestClass]
    public class NativeSafetyTest
    {
        [DataTestMethod]
        [DataRow(false)]
        [DataRow(true)]
        public void BufferConversionReportsRequiredCapacity(bool withOption)
        {
            byte[] input = Encoding.ASCII.GetBytes("ABC");
            WrapNkf.SetNkfOption("-w");
            try
            {
                for (int capacity = 0; capacity <= input.Length; capacity++)
                {
                    byte[] output = new byte[capacity + 1];
                    Array.Fill(output, (byte)0xCC);
                    int count;
                    bool result = withOption
                        ? WrapNkf.NkfConvertSafeWithOption(output, capacity, out count, input, input.Length, "-w")
                        : WrapNkf.NkfConvertSafe(output, capacity, out count, input, input.Length);
                    Assert.AreEqual(input.Length, count);
                    Assert.AreEqual(capacity == input.Length, result);
                    Assert.AreEqual((byte)0xCC, output[capacity]);
                    for (int i = 0; i < capacity; i++)
                    {
                        Assert.AreEqual(input[i], output[i]);
                    }
                }
            }
            finally
            {
                WrapNkf.SetNkfOption(null!);
            }
        }

        [TestMethod]
        public void StringAndReaderConversionGrowForShiftJisExpansion()
        {
            byte[] input = new byte[18];
            input[0] = 0x82;
            input[1] = 0xA0;
            Array.Fill(input, (byte)0xB1, 2, 16);
            string expected = "\u3042" + new string('\u30A2', 16);

            Assert.AreEqual(51, Encoding.UTF8.GetByteCount(expected));
            Assert.IsTrue(Encoding.UTF8.GetByteCount(expected) > input.Length * 2 + 10);
            Assert.AreEqual(expected, WrapNkf.NkfConvert(input, 0, input.Length));
            byte[] slice = new byte[input.Length + 2];
            Array.Copy(input, 0, slice, 1, input.Length);
            Assert.AreEqual(expected, WrapNkf.NkfConvert(slice, 1, input.Length));

            using (MemoryStream stream = new MemoryStream(input))
            using (NkfTextReader reader = new NkfTextReader(stream))
            {
                Assert.AreEqual(expected, reader.ReadLine());
                Assert.IsNull(reader.ReadLine());
            }
        }

        [TestMethod]
        public void ReaderConversionGrowsForMimeOutput()
        {
            byte[] input = { 0x82, 0xA0, 0x82, 0xA0, 0x82, 0xA0, 0x82, 0xA0 };
            const string expected = "=E3=81=82=E3=81=82=E3=81=82=E3=81=82";
            Assert.IsTrue(expected.Length > input.Length * 2 + 10);
            using (MemoryStream stream = new MemoryStream(input))
            using (NkfTextReader reader = new NkfTextReader(stream))
            {
                reader.SetNkfOption("-MQ");
                Assert.AreEqual(expected, reader.ReadLine());
            }
        }

        [TestMethod]
        public void CharacterSizingHandlesExpandingOptions()
        {
            byte[] input = Encoding.UTF8.GetBytes("\uFF06");
            NkfEncoding encoding = new NkfEncoding("-Z3");
            const string expected = "&amp;";
            Assert.IsTrue(expected.Length > input.Length);
            Assert.AreEqual(expected.Length, encoding.GetCharCount(input, 0, input.Length));
            Assert.AreEqual(expected, encoding.GetString(input));

            char[] exact = new char[expected.Length];
            Assert.AreEqual(exact.Length, encoding.GetChars(input, 0, input.Length, exact, 0));
            Assert.AreEqual(expected, new string(exact));
            Assert.AreEqual("chars", Assert.ThrowsException<ArgumentException>(
                () => encoding.GetChars(input, 0, input.Length, new char[expected.Length - 1], 0)).ParamName);
            Assert.AreEqual("chars", Assert.ThrowsException<ArgumentException>(
                () => encoding.GetChars(input, 0, input.Length, new char[0], 0)).ParamName);

            char[] offset = { '#', '#', '#', '#', '#' };
            Assert.AreEqual("chars", Assert.ThrowsException<ArgumentException>(
                () => encoding.GetChars(input, 0, input.Length, offset, 1)).ParamName);
            Assert.AreEqual('#', offset[0]);
        }

        [DataTestMethod]
        [DataRow("-v")]
        [DataRow("-V")]
        [DataRow("--help")]
        [DataRow("--version")]
        [DataRow("--help=ignored")]
        [DataRow("v")]
        [DataRow("V")]
        [DataRow("wV")]
        [DataRow("-Swv")]
        [DataRow("-wV")]
        [DataRow("-w-help")]
        [DataRow("-w-version")]
        [DataRow("-f60-10v")]
        [DataRow("-w prefix--help")]
        [DataRow("-w -- --help")]
        [DataRow("  --help  ")]
        [DataRow("-w\0 --help")]
        [DataRow("-w\t-V")]
        [DataRow("-w\n--help")]
        [DataRow("-\uFF56")]
        [DataRow("\uFF0D\uFF36")]
        public void UnsafeOptionsAreRejectedAtEveryEntryPoint(string option)
        {
            byte[] input = { 65 };
            byte[] output = new byte[16];
            Assert.ThrowsException<ArgumentException>(() => WrapNkf.SetNkfOption(option));
            Assert.ThrowsException<ArgumentException>(() =>
                WrapNkf.NkfConvertSafeWithOption(output, output.Length, out _, input, input.Length, option));
            Assert.ThrowsException<ArgumentException>(() => new NkfEncoding(option));
            Assert.ThrowsException<ArgumentException>(() => new NkfEncoding().SetNkfOption(option));
            using (MemoryStream stream = new MemoryStream(input))
            using (NkfTextReader reader = new NkfTextReader(stream))
            {
                Assert.ThrowsException<ArgumentException>(() => reader.SetNkfOption(option));
            }
        }

        [DataTestMethod]
        [DataRow("-Sw")]
        [DataRow("-w --ic=UTF-8 --fb-java")]
        [DataRow("-w --cp932inv")]
        [DataRow("-w --prefix=Vv")]
        [DataRow("w")]
        [DataRow(" -w")]
        public void SafeOptionClustersAndLongOptionValuesRemainSupported(string option)
        {
            byte[] input = Encoding.UTF8.GetBytes("ABC");
            byte[] output = new byte[32];
            Assert.IsTrue(WrapNkf.NkfConvertSafeWithOption(output, output.Length, out int count, input, input.Length, option));
            Assert.AreEqual("ABC", Encoding.UTF8.GetString(output, 0, count));
            NkfEncoding encoding = new NkfEncoding(option);
            Assert.AreEqual("ABC", encoding.GetString(input));
            using (MemoryStream stream = new MemoryStream(input))
            using (NkfTextReader reader = new NkfTextReader(stream))
            {
                reader.SetNkfOption(option);
                Assert.AreEqual("ABC", reader.ReadLine());
            }
        }

        [TestMethod]
        public void FoldMarginsRemainSupported()
        {
            byte[] input = Encoding.UTF8.GetBytes("ABC");
            byte[] output = new byte[32];
            Assert.IsTrue(WrapNkf.NkfConvertSafeWithOption(
                output, output.Length, out int count, input, input.Length, "-w -f60-10"));
            Assert.AreEqual("ABC\n", Encoding.UTF8.GetString(output, 0, count));
        }

        [TestMethod]
        public void RejectedOptionsDoNotReplacePreviousSettings()
        {
            byte[] input = Encoding.UTF8.GetBytes("\u3042");
            byte[] output = new byte[16];
            WrapNkf.SetNkfOption("-s");
            try
            {
                Assert.ThrowsException<ArgumentException>(() => WrapNkf.SetNkfOption("--help"));
                Assert.IsTrue(WrapNkf.NkfConvertSafe(output, output.Length, out int count, input, input.Length));
                Assert.AreEqual(2, count);
                Assert.AreEqual((byte)0x82, output[0]);
                Assert.AreEqual((byte)0xA0, output[1]);
            }
            finally
            {
                WrapNkf.SetNkfOption(null!);
            }
        }

        [TestMethod]
        public void ResetOptionsRestoresUtf8AfterOtherNativeCalls()
        {
            byte[] input = Encoding.UTF8.GetBytes("\u3042");
            byte[] output = new byte[16];
            WrapNkf.SetNkfOption("-s");
            WrapNkf.SetNkfOption(null!);
            Assert.AreEqual("\u3042", new NkfEncoding().GetString(input));
            Assert.IsTrue(WrapNkf.NkfConvertSafe(output, output.Length, out int count, input, input.Length));
            Assert.AreEqual(input.Length, count);
            Assert.AreEqual("\u3042", Encoding.UTF8.GetString(output, 0, count));
        }

        [TestMethod]
        public void NativeQueriesAndConversionsRemainConsistentAcrossThreads()
        {
            Task[] workers = new Task[4];
            for (int i = 0; i < workers.Length; i++)
            {
                bool shiftJis = i % 2 == 0;
                workers[i] = Task.Run(() =>
                {
                    const string expected = "\u3042";
                    byte[] input = Encoding.UTF8.GetBytes(expected);
                    byte[] expectedOutput = shiftJis ? new byte[] { 0x82, 0xA0 } : input;
                    byte[] output = new byte[16];
                    NkfEncoding encoding = new NkfEncoding();
                    WrapNkf.SetNkfOption(shiftJis ? "-s" : "-w");
                    try
                    {
                        for (int iteration = 0; iteration < 100; iteration++)
                        {
                            Assert.AreEqual(expected, encoding.GetString(input));
                            Assert.AreEqual(expected, NkfEncoding.NkfConvert(input, 0, input.Length));
                            Assert.AreEqual(expected, WrapNkf.NkfConvert(input, 0, input.Length));
                            Assert.IsTrue(WrapNkf.NkfConvertSafe(output, output.Length, out int count, input, input.Length));
                            Assert.AreEqual(expectedOutput.Length, count);
                            for (int j = 0; j < count; j++)
                            {
                                Assert.AreEqual(expectedOutput[j], output[j]);
                            }
                            Assert.IsTrue(WrapNkf.GetGuess().Contains("UTF-8"));
                            Assert.IsTrue(WrapNkf.Usage().Contains("Usage:"));
                            Assert.IsTrue(WrapNkf.GetNkfVersion().Length > 0);
                            Assert.IsTrue(WrapNkf.GetSupportFunctions().Length > 0);
                        }
                    }
                    finally
                    {
                        WrapNkf.SetNkfOption(null!);
                    }
                });
            }
            Task.WaitAll(workers);
        }
    }
}
