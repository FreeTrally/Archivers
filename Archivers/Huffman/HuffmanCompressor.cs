using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace Archivers.Huffman
{
    public static class HuffmanCompressor
    {
        public delegate void UpdateCrc(int value);

        private static byte[] sign = { 0x5a, 0x52, 0x41, 0x48 };

        public static void Compress(string inputFile, string outputFile, out string filePath)
        {
            var input = new FileStream(inputFile, FileMode.Open, FileAccess.Read);
            var output = new FileStream(outputFile, FileMode.Create, FileAccess.ReadWrite);
            var crc = new CyclicRedundancyCheck();

            output.Write(sign, 0, sign.Length);
            for (var i = 0; i < 12; i++)
                output.WriteByte(0x0);

            var compressor = new Thread(o => Huff(input, output, val => crc.UpdateByte((byte)val)));
            compressor.Start();

            while (compressor.IsAlive)
                Thread.Sleep(100);

            output.Seek(4, SeekOrigin.Begin);
            var buffer = BitConverter.GetBytes(output.Length);
            output.Write(buffer, 0, buffer.Length);

            var originalL = input.Length;
            var compressedL = output.Length;
            input.Close();
            output.Close();

            var coef = (double)originalL / compressedL;
            Debug.WriteLine("");
            Debug.WriteLine($"Длина изначальная = {originalL}");
            Debug.WriteLine($"Длина сжатая = {compressedL}");
            Debug.WriteLine($"Коэффициент сжатия = {coef}");
            Debug.WriteLine("");
            filePath = Path.GetFullPath(outputFile);
        }

        private static void Huff(Stream input, Stream output, UpdateCrc callback)
        {
            var tree = new HuffmanTree();
            int rd, i, tmp;
            Stack<int> ret;
            var bw = new BitHandler(output, true);
            while ((rd = input.ReadByte()) != -1)
            {
                tmp = rd;
                callback((byte)tmp);
                if (!tree.Contains(rd))
                {
                    ret = tree.GetCode(257);
                    while (ret.Count > 0)
                        bw.WriteBit(ret.Pop());
                    for (i = 0; i < 8; i++)
                    {
                        bw.WriteBit((int)(rd & 0x80));
                        rd <<= 1;
                    }
                    tree.Update(tmp);
                }
                else
                {
                    ret = tree.GetCode(tmp);
                    while (ret.Count > 0)
                        bw.WriteBit(ret.Pop());
                    tree.Update(tmp);
                }
            }

            ret = tree.GetCode(256);
            while (ret.Count > 0)
            {
                bw.WriteBit(ret.Pop());
            }
        }

        public static void Decompress(string inputFile, string outputFile, out string filePath)
        {
            var input = new FileStream(inputFile, FileMode.Open, FileAccess.Read);
            var output = new FileStream(outputFile, FileMode.Create, FileAccess.ReadWrite);
            var crc = new CyclicRedundancyCheck();
            uint crc_old, crc_new;

            for (int i = 0; i < sign.Length; i++)
                if (input.ReadByte() != sign[i])
                {
                    input.Close();
                    output.Close();
                    throw new IOException("The supplied file is not a valid huff archive");
                }

            byte[] buffer = new byte[8];
            input.Read(buffer, 0, 8);
            long size = BitConverter.ToInt64(buffer, 0);
            if (size != input.Length)
            {
                input.Close();
                output.Close();
                throw new IOException("Invalid file length");
            }

            input.Read(buffer, 0, 4);
            crc_old = BitConverter.ToUInt32(buffer, 0);

            var myDecompressor = new Thread(o =>
           {
               UnHuff(input, output, x => crc.UpdateByte((byte)x));
           });
            myDecompressor.Start();
            while (myDecompressor.IsAlive)
                Thread.Sleep(100);

            crc_new = crc.Get();
            output.Close();
            input.Close();
            filePath = Path.GetFullPath(outputFile);
        }

        public static void UnHuff(Stream InStream, Stream OutStream, UpdateCrc callback)
        {
            int i = 0, count = 0, sym;
            HuffmanTree t = new HuffmanTree();
            BitHandler bitIO = new BitHandler(InStream, false);
            while ((i = bitIO.ReadBit()) != 2)
            {
                if ((sym = t.DecodeBinary(i)) != HuffmanTree.CharIsEof)
                {
                    OutStream.WriteByte((byte)sym);
                    callback(sym);
                    count++;
                }
            }
        }
    }
}
