using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace UUCodec
{
    public class UUCodec 
    {
        private const string m_filenameFmt = @"{0}\{1}{2}.txt";
        private const string m_headerStringFmt = "begin 644 {0}";
        private const int m_bufferLength = 45;
        private int m_maxLinesPerFile = 5000;

        public UUCodec()
        {

        }

        public UUCodec(int linesPerFile)
        {
            m_maxLinesPerFile = linesPerFile;
        }

        #region ISmartCodec Members

        public void Encode(string filename, string outputFolder)
        {
            #region Test code
            //byte[] test = new byte[3];
            //test[0] = (byte)'C';
            //test[1] = (byte)'a';
            //test[2] = (byte)'t';

            //byte[] ret = new byte[4];
            //int o = EncodeLine(test, 3, ret);

            //byte[] out1 = new byte[3];
            //o = DecodeLine(ret, 4, out1);

            //return;
            #endregion

            FileStream fs = File.OpenRead(filename);
            BinaryReader br = new BinaryReader(fs);
            byte[] buffer = new byte[m_bufferLength];
            byte[] encBytes = new byte[m_bufferLength + (m_bufferLength / 3)];
            int pos = 0;
            int bytesRead = 0;
            int fileNumber = 0;
            int bytesEncd = 0;
            int lineNum = 0;

            FileStream outFile = null;
            TextWriter writer = null;
            while ((bytesRead = br.Read(buffer, pos, m_bufferLength)) > 0)
            {
                lineNum++;
                if (outFile == null || lineNum > m_maxLinesPerFile)
                {
                    if (outFile != null)
                    {
                        writer.Close();
                        outFile.Close();
                    }

                    fileNumber++;
                    outFile = File.Create(string.Format(m_filenameFmt,
                      outputFolder,
                      Path.GetFileNameWithoutExtension(filename),
                      (fileNumber == 1) ? "" : fileNumber.ToString())
                      );

                    writer = new StreamWriter(outFile, Encoding.ASCII);
                    if (fileNumber == 1)
                    {
                        writer.Write(string.Format(m_headerStringFmt, Path.GetFileName(filename)));
                        writer.Write("\n\n");
                    }
                    lineNum = 1;
                }


                bytesEncd = EncodeLine(buffer, bytesRead, encBytes);
                writer.Write(Convert.ToChar(bytesRead + 32));
                writer.Write(System.Text.Encoding.ASCII.GetString(encBytes, 0, bytesEncd));
                writer.Write("\n");


            }

            if (bytesRead < m_bufferLength)
            {
                writer.Write("`\n");
                writer.Write("end\n");
            }

            writer.Close();
            outFile.Close();

            br.Close();
            fs.Close();

        }

        public void Decode(string[] filenames, string outputFolder)
        {
            System.Text.RegularExpressions.Regex beginRegex = new System.Text.RegularExpressions.Regex("^begin [^ ]+ (.*$)");
            FileStream outFileStream = null;
            BinaryWriter writer = null;
            string line = string.Empty;
            int bytesDecd = 0;
            int bytesToRead = 0;
            byte[] buffer = new byte[m_bufferLength + (m_bufferLength / 3)];
            byte[] decBuffer = new byte[m_bufferLength];

            int i = 0;
            foreach (string file in filenames)
            {
                i++;
                FileStream inFileStream = File.OpenRead(file);
                TextReader reader = new StreamReader(inFileStream);

                if (outFileStream == null)
                {
                    line = reader.ReadLine();
                    System.Text.RegularExpressions.Match match = beginRegex.Match(line);
                    if (match.Groups.Count > 0)
                    {
                        outFileStream = File.Create(string.Format(@"{0}\{1}", outputFolder, match.Groups[1]), m_bufferLength);
                        writer = new BinaryWriter(outFileStream);
                    }

                    inFileStream.Seek(0, SeekOrigin.Begin);
                }

                while ((line = reader.ReadLine()) != null && line != "end")
                {
                    if (line.Length > 1)
                    {
                        bytesToRead = line[0] - 32; // The first byte defines how many bytes to process
                        buffer = System.Text.Encoding.ASCII.GetBytes(line.Substring(1));
                        if (buffer.Length >= bytesToRead)
                        {
                            bytesDecd = DecodeLine(buffer, bytesToRead, decBuffer);
                            writer.Write(decBuffer, 0, bytesDecd);
                        }
                    }
                }

                reader.Close();
                inFileStream.Close();

            }

            if (writer != null)
                writer.Close();

            if (outFileStream != null)
                outFileStream.Close();


        }

        public void Decode(string inputFolder, string outputFolder)
        {
            throw new NotImplementedException();
        }

        #endregion

        public List<Byte> EncodeLine(string line)
        {
            int bytesDecd = 0;
            int bytesToRead = 0;
            List<Byte> ans = new List<byte>();

            if (line.Length > 1)
            {
                bytesToRead = line[0] - 32; // The first byte defines how many bytes to process
                byte []buffer = System.Text.Encoding.ASCII.GetBytes(line.Substring(1));
                byte[] decBuffer = new byte[1024];
                if (buffer.Length >= bytesToRead)
                {
                    bytesDecd = DecodeLine(buffer, bytesToRead, decBuffer);
                    for (int i = 0; i < bytesDecd; i++)
                    {
                        ans.Add(decBuffer[i]);
                    }
                }
            }

            return ans;
        }

        public int EncodeLine(byte[] bytes, int numBytes, byte[] outBytes)
        {
            int j = 0;
            for (int i = 0; i < numBytes; i += 3)
            {
                outBytes[j] = (byte)((bytes[i] >> 2) + 32);
                outBytes[j + 1] = (byte)((((bytes[i] & 3) << 4) | ((bytes[i + 1] & 240) >> 4)) + 32); // 3 = 00000011, 240 = 11110000
                outBytes[j + 2] = (byte)((((bytes[i + 1] & 15) << 2) | ((bytes[i + 2] & 192) >> 6)) + 32); // 15 = 00001111, 192 = 11000000
                outBytes[j + 3] = (byte)((bytes[i + 2] & 63) + 32); // 63 = 00111111

                outBytes[j] = (char)outBytes[j] == ' ' ? (byte)'`' : outBytes[j];
                outBytes[j + 1] = (char)outBytes[j + 1] == ' ' ? (byte)'`' : outBytes[j + 1];
                outBytes[j + 2] = (char)outBytes[j + 2] == ' ' ? (byte)'`' : outBytes[j + 2];
                outBytes[j + 3] = (char)outBytes[j + 3] == ' ' ? (byte)'`' : outBytes[j + 3];

                j += 4;
            }

            return j;
        }


        public int DecodeLine(byte[] bytes, int numBytes, byte[] outBytes)
        {
            int j = 0;
            int max = numBytes + (numBytes / 3);
            for (int i = 0; i < max; i += 4)
            {
                bytes[i] = (char)bytes[i] == '`' ? (byte)' ' : bytes[i];
                bytes[i + 1] = (char)bytes[i + 1] == '`' ? (byte)' ' : bytes[i + 1];
                bytes[i + 2] = (char)bytes[i + 2] == '`' ? (byte)' ' : bytes[i + 2];
                bytes[i + 3] = (char)bytes[i + 3] == '`' ? (byte)' ' : bytes[i + 3];

                outBytes[j] = (byte)(((bytes[i] - 32) << 2) | (((bytes[i + 1] - 32) & 48) >> 4)); // 48 = 00110000
                outBytes[j + 1] = (byte)((((bytes[i + 1] - 32) & 15) << 4) | (((bytes[i + 2] - 32) & 60) >> 2)); // 15 = 00001111, 60 = 00111100
                outBytes[j + 2] = (byte)((((bytes[i + 2] - 32) & 3) << 6) | ((bytes[i + 3] - 32) & 63)); // 3 = 00000011, 63 = 00111111

                j += 3;
            }

            return j;
        }

    }
}

