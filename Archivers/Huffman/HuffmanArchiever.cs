using System;

namespace Archivers.Huffman
{
    public class HuffmanArchiever : IArchiver
    {
        public void CompressFile (string filename, string fileOutName, out string filePath)
        {
            HuffmanCompressor.Compress(filename, fileOutName, out var file);
            filePath = file;
        }

        public void DecompressFile (string filename, string fileOutName, out string filePath) {
            HuffmanCompressor.Decompress(filename, fileOutName, out var file);
            filePath = file;
        }
    }
}