using System;
using System.Diagnostics;
using Archivers;
using NUnit.Framework;

namespace ArchiversTests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void LZW_Correct()
        {
            var lzw = new LZW();
            var entry = "banana_bandana";
            var compress = lzw.Compress(entry);
            var decompress = lzw.Decompress(compress);
            Debug.WriteLine(entry);
            Debug.WriteLine(compress);
            Debug.WriteLine(decompress);
            Assert.AreEqual(entry, decompress);
        }
    }
}