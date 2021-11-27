using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Archivers.Huffman
{
    public class HuffmanOld
    {
        private readonly Dictionary<char, string> symbolCodes = new();
        private readonly Dictionary<string, char> codeSymbols = new();
        private readonly Dictionary<char, int> symbolsCount = new();
        private readonly Dictionary<string, int> availableLeaves = new();

        public string Compress(string data)
        {
            CalculateFrequency(data);
            var result = CalculateResult(data);

            return result;
        }

        public string Decompress(string data)
        {
            return DecodeResult(data);
        }

        private void CalculateFrequency(string data)
        {
            foreach (var ch in data)
            {
                symbolCodes.TryAdd(ch, "");
                symbolsCount[ch] = symbolsCount.ContainsKey(ch) ? symbolsCount[ch] + 1 : 1;
            }

            foreach (var (ch, count) in symbolsCount)
            {
                availableLeaves.Add(ch.ToString(), count);
            }
        }

        private string DecodeResult(string data)
        {
            var builder = new StringBuilder();
            var keyBuilder = new StringBuilder();

            foreach (var ch in data)
            {
                keyBuilder.Append(ch);
                var key = keyBuilder.ToString();
                if (codeSymbols.ContainsKey(key))
                {
                    builder.Append(codeSymbols[key]);
                    keyBuilder.Clear();
                }
            }

            return builder.ToString();
        }

        private string CalculateResult(string data)
        {
            EncodeSymbols();
            var builder = new StringBuilder();
            foreach (var ch in data)
                builder.Append(symbolCodes[ch]);

            return builder.ToString();
        }

        private void EncodeSymbols()
        {
            while (availableLeaves.Count > 1)
                CreateCodeForSymbols();

            foreach (var encoding in symbolCodes)
            {
                var encode = encoding.Value;
                if (encode.Length > 1)
                    encode = new string(encode.Reverse().ToArray());
                symbolCodes[encoding.Key] = encode;
                codeSymbols.Add(encode, encoding.Key);
            }
        }

        private void CreateCodeForSymbols()
        {
            var toMerge = availableLeaves.OrderBy(leaf => leaf.Value).Take(2);
            var newLeafKey = "";
            var newLeafValue = 0;

            var index = 0;
            foreach (var (key, value) in toMerge)
            {
                foreach (var ch in key)
                    symbolCodes[ch] += index.ToString();
                index++;

                newLeafKey += key;
                newLeafValue += value;
                availableLeaves.Remove(key);
            }

            availableLeaves.Add(newLeafKey, newLeafValue);
        }
    }
}
