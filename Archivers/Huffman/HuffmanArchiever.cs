using System;

namespace Archivers.Huffman
{
    public class HuffmanArchiever : IArchiver
    {
        public void CompressFile (string filename, string fileOutName, out string filePath)
        {
            try {
                HuffmanCompressor.Compress(filename, fileOutName, out var file);
                filePath = file;
            }
            catch (Exception e) 
            {
                filePath = null;
                Console.WriteLine("Unable to compress the file due to the error: " + e.Message);
            }
        }

        public void DecompressFile (string filename, string fileOutName, out string filePath) {
            try {
                HuffmanCompressor.Decompress(filename, fileOutName, out var file);
                filePath = file;
            }
            catch (Exception e) 
            {
                filePath = null;
                Console.WriteLine("Unable to decompress the file due to the error: " + e.Message);
            }
        }
    }
}