using System;
using System.IO;
using System.Linq;
using System.Text;
using Archivers.Huffman;

namespace Archivers
{
    class Program
    {
        static void Main(string[] args)
        {
            new LZWArchiever().CompressFile("../../../huge.txt", "../../../hugeC.txt", out var path);
            new LZWArchiever().DecompressFile(path, "../../../hugeD.txt", out var dpath);

            new HuffmanArchiever().CompressFile("../../../huge.txt", "../../../hugeC.txt", out var patha);
            new HuffmanArchiever().DecompressFile(path, "../../../hugeD.txt", out var dpatha);
        }
    }
}