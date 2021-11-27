using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Archivers;
using Archivers.Huffman;
using NUnit.Framework;

namespace ArchiversTests
{
    public class HuffmanTests
    {
        private IArchiver archiever;

        [SetUp]
        public void Setup()
        {
            archiever = new HuffmanArchiever();
        }

        [Test]
        public void SmallText()
        {
            var original = "../../../Texts/small.txt";
            archiever.CompressFile(original, "../../../Texts/smallC.txt", out var path);
            archiever.DecompressFile(path, "../../../Texts/smallD.txt", out var dpath);
            var areEquals = File.ReadLines(original)
                .SequenceEqual(File.ReadLines(dpath));
            Assert.IsTrue(areEquals);
        }

        [Test]
        public void MediumText()
        {
            var original = "../../../Texts/medium.txt";
            archiever.CompressFile(original, "../../../Texts/mediumC.txt", out var path);
            archiever.DecompressFile(path, "../../../Texts/mediumD.txt", out var dpath);
            var areEquals = File.ReadLines(original)
                .SequenceEqual(File.ReadLines(dpath));
            Assert.IsTrue(areEquals);
        }

        [Test]
        public void BigText()
        {
            var original = "../../../Texts/big.txt";
            archiever.CompressFile(original, "../../../Texts/bigC.txt", out var path);
            archiever.DecompressFile(path, "../../../Texts/bigD.txt", out var dpath);
            var areEquals = File.ReadLines(original)
                .SequenceEqual(File.ReadLines(dpath));
            Assert.IsTrue(areEquals);
        }
    }
}