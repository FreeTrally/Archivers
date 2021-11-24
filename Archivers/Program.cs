using System;
using System.IO;
using System.Linq;

namespace Archivers
{
    class Program
    {
        static void Main(string[] args)
        {
            var ar = new HuffmanArchiver();
            //Console.WriteLine(ar.Compress("your_struggles_are_futile"));
            var res = ar.Compress("Welcome to the Archive service   Menu:   1. Use LZW archiver;    2. Use all archivers;");
            Console.WriteLine(res);
            Console.WriteLine(ar.Decompress(res));
            //Handle();
        }

        private static void Handle()
        {
            Console.WriteLine("Welcome to the Archive service");
            Console.WriteLine("Menu: ");
            Console.WriteLine("1. Use LZW archiver;");
            Console.WriteLine("2. Use all archivers;");

            Console.WriteLine("Please, choose an input method:");
            Console.WriteLine("1. Just put in to the console input;");
            Console.WriteLine("2. Input from the file.");
            var inputMethod = Console.ReadLine();
            var entry = string.Empty;

            if (inputMethod == "1")
                entry = Console.ReadLine();

            if (inputMethod == "2")
            {
                Console.WriteLine("Please, enter file path.");

                var path = Console.ReadLine();
                using var fstream = File.OpenRead(path);
                // преобразуем строку в байты
                var array = new byte[fstream.Length];
                // считываем данные
                fstream.Read(array, 0, array.Length);
                // декодируем байты в строку
                entry = System.Text.Encoding.Default.GetString(array);
                Console.WriteLine($"Text from the file: {entry}");
            }

            var lzw = new LZW();

            var compressed = lzw.Compress(entry);
            var decompressed = lzw.Decompress(compressed);
            if (inputMethod == "1")
            {
                Console.WriteLine("Compressed: " + compressed);
                Console.WriteLine("Decompressed: " + decompressed);
            }

            if (inputMethod == "2")
            {
                using (var fstream =
                    new FileStream($@"C:\Users\kvash\Desktop\archiveComressed.txt", FileMode.OpenOrCreate))
                {
                    fstream.Write(compressed.Select(el => (byte)el).ToArray(), 0, compressed.Count);
                    Console.WriteLine("Text compressed and saved to the file archiveComressed.txt");
                }

                using (var fstream = new FileStream($@"C:\Users\kvash\Desktop\archiveDecomressed.txt",
                    FileMode.OpenOrCreate))
                {
                    var array = System.Text.Encoding.Default.GetBytes(decompressed);
                    fstream.Write(array, 0, array.Length);
                    Console.WriteLine("Text decompressed and saved to the file archiveDecomressed.txt");
                }
            }
        }
    }
}