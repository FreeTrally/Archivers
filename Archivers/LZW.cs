﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Archivers
{
    public class LZW
    {
        public string Compress(string uncompressed)
        {
            if (uncompressed.Length == 0)
                return string.Empty;
            var dictionary = GetInitialDictionaryBySymbol();

            var current = string.Empty;
            var compressed = new List<int>();

            foreach (var ch in uncompressed)
            {
                var workingString = current + ch;
                if (dictionary.ContainsKey(workingString))
                    current = workingString;
                else
                {
                    compressed.Add(dictionary[current]);
                    dictionary.Add(workingString, dictionary.Count);
                    current = ch.ToString();
                }
            }

            // write remaining output if necessary
            if (!string.IsNullOrEmpty(current))
                compressed.Add(dictionary[current]);

            return string.Join(',', compressed);
        }

        public string Decompress(string compressed)
        {
            if (compressed.Length == 0)
                return string.Empty;

            var dict = GetInitialDictionaryByIndex();

            var indexes = compressed.Split(',').Select(ch => int.Parse(ch)).ToList();

            var workingString = dict[indexes[0]];
            indexes.RemoveAt(0);

            var decompressed = new StringBuilder(workingString);

            foreach (var index in indexes)
            {
                string entry = null;
                if (dict.ContainsKey(index))
                    entry = dict[index];
                else if (index == dict.Count)
                    entry = workingString + workingString[0];

                decompressed.Append(entry);

                // new sequence; add it to the dictionary
                dict.Add(dict.Count, workingString + entry?.FirstOrDefault());

                workingString = entry;
            }

            return decompressed.ToString();
        }

        private static Dictionary<string, int> GetInitialDictionaryBySymbol()
        {
            var dict = new Dictionary<string, int>();
            for (var i = 0; i < 256; i++)
                dict.Add(((char)i).ToString(), i);
            return dict;
        }

        private static Dictionary<int, string> GetInitialDictionaryByIndex()
        {
            var dict = new Dictionary<int, string>();
            for (var i = 0; i < 256; i++)
                dict.Add(i, ((char)i).ToString());
            return dict;
        }
    }
}