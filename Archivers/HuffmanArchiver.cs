using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Archivers
{
    public class HuffmanArchiver : IArchiver
    {
        private Dictionary<char, string> symbolCodes = new();
        private Dictionary<string, char> codeSymbols = new();
        private Dictionary<char, int> symbolsCount = new();
        private Dictionary<string, int> availableLeaves = new();
        private string input;

        public string Compress(string data)
        {
            input = data;

            CalculateFrequency(data);

            return CalculateResult();
        }

        public string Decompress(string data)
        {
            return DecodeResult(data);
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

        private string CalculateResult()
        {
            EncodeSymbols();
            var builder = new StringBuilder();
            foreach (var ch in input)
            {
                builder.Append(symbolCodes[ch]);
            }
            return builder.ToString();
        }

        private void EncodeSymbols()
        {
            while (availableLeaves.Count > 1)
            {
                NewMethod();
            }

            foreach (var encoding in symbolCodes)
            {
                var encode = encoding.Value;
                if (encode.Length > 1)
                    encode = new string(encode.Reverse().ToArray());
                symbolCodes[encoding.Key] = encode;
                codeSymbols.Add(encode, encoding.Key);
            }               
        }

        private void NewMethod()
        {
            var toMerge = availableLeaves.OrderBy(leaf => leaf.Value).Take(2);
            var newLeafKey = "";
            var newLeafValue = 0;

            var index = 0;
            foreach (var leaf in toMerge)
            {
                foreach (var ch in leaf.Key)
                    symbolCodes[ch] += index.ToString();
                index++;

                newLeafKey += leaf.Key;
                newLeafValue += leaf.Value;
                availableLeaves.Remove(leaf.Key);
            }
            availableLeaves.Add(newLeafKey, newLeafValue);
        }

        private void CalculateFrequency(string data)
        {
            foreach (var ch in data)
            {
                symbolCodes.TryAdd(ch, "");
                symbolsCount[ch] = symbolsCount.ContainsKey(ch) ? symbolsCount[ch] + 1 : 1;
            }

            foreach (var counts in symbolsCount)
            {
                availableLeaves.Add(counts.Key.ToString(), counts.Value);
            }
        }
    }
}
