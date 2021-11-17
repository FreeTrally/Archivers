using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Archivers
{
    public class LZW
    {
        public List<int> Compress(string uncompressed)
        {
            // build the dictionary
            var dictionary = new Dictionary<string, int>();
            for (var i = 0; i < 256; i++)
                dictionary.Add(((char)i).ToString(), i);

            var w = string.Empty;
            var compressed = new List<int>();

            foreach (var c in uncompressed)
            {
                var wc = w + c;
                if (dictionary.ContainsKey(wc))
                    w = wc;
                else
                {
                    // write w to output
                    compressed.Add(dictionary[w]);
                    // wc is a new sequence; add it to the dictionary
                    dictionary.Add(wc, dictionary.Count);
                    w = c.ToString();
                }
            }

            // write remaining output if necessary
            if (!string.IsNullOrEmpty(w))
                compressed.Add(dictionary[w]);

            return compressed;
        }

        public string Decompress(List<int> compressed)
        {
            // build the dictionary
            var dictionary = new Dictionary<int, string>();
            for (var i = 0; i < 256; i++)
                dictionary.Add(i, ((char)i).ToString());

            var w = dictionary[compressed[0]];
            compressed.RemoveAt(0);
            var decompressed = new StringBuilder(w);

            foreach (var k in compressed)
            {
                string entry = null;
                if (dictionary.ContainsKey(k))
                    entry = dictionary[k];
                else if (k == dictionary.Count)
                    entry = w + w[0];

                decompressed.Append(entry);

                // new sequence; add it to the dictionary
                dictionary.Add(dictionary.Count, w + entry?.FirstOrDefault());

                w = entry;
            }

            return decompressed.ToString();
        }
    }
}
