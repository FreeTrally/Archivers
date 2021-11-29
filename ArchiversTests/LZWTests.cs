using System;
using System.IO;
using System.Linq;
using Archivers;
using NUnit.Framework;

namespace ArchiversTests
{
    public class LZWTests
    {
        private IArchiver archiever;

        [SetUp]
        public void Setup()
        {
            archiever = new LZWArchiever();
        }

        [Test]
        public void TinyText()
        {
            var original = "../../../Texts/tiny.txt";
            archiever.CompressFile(original, "../../../Texts/tinyC.txt", out var path);
            archiever.DecompressFile(path, "../../../Texts/tinyD.txt", out var dpath);
            var areEquals = File.ReadLines(original)
                .SequenceEqual(File.ReadLines(dpath));
            Assert.IsTrue(areEquals);
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

        [Test]
        public void HugeText()
        {
            var original = "../../../Texts/huge.txt";
            archiever.CompressFile(original, "../../../Texts/hugeC.txt", out var path);
            archiever.DecompressFile(path, "../../../Texts/hugeD.txt", out var dpath);
            var areEquals = File.ReadLines(original)
                .SequenceEqual(File.ReadLines(dpath));
            Assert.IsTrue(areEquals);
        }
    }
}