using System;
using System.IO;

namespace Archivers.Huffman
{
    public class BitHandler
    {
        Stream stream;
        bool ownStream = false, isOut = false, isOpen = false;
        byte current = 0;
        int bit = 0;
        byte buffer = 0, bits = 0;

        public BitHandler(Stream stream, bool isOut)
        {
            this.stream = stream;
            isOpen = true;
            this.isOut = isOut;
            if (!isOut)
                bit = stream.ReadByte();
        }

        public void Close()
        {
            isOpen = false;
            if (isOut)
                BitFlush();
            if (ownStream)
                stream.Close();
        }

        public void WriteBit(int bit)
        {
            if (!isOpen)
                throw new InvalidOperationException("Cannot write to the disposing stream");
            if (!isOut)
                throw new NotSupportedException("Cannot write to the read-only bit stream");
            if (bits == 8)
            {
                bits = 0;
                stream.WriteByte(buffer);
                buffer = 0;
            }
            bits++;
            buffer <<= 1;
            if (bit > 0)
                buffer |= 0x1;
        }

        private void BitFlush()
        {
            buffer <<= (8 - bits);
            stream.WriteByte(buffer);
        }

        public int ReadBit()
        {
            if (!isOpen)
                throw new InvalidOperationException("Cannot read from the disposing stream");
            if (isOut)
                throw new NotSupportedException("Cannot read from a write-only bit stream");
            current++;
            int result = (bit & 0x80) > 0 ? 1 : 0;
            bit <<= 1;
            if (current == 8)
            {
                current = 0;
                bit = stream.ReadByte();
                if (bit == -1)
                    return 2;
                else
                    bit &= 0xff;
            }
            return result;
        }
    }
}
