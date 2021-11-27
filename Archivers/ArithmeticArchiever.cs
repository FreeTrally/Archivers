using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archivers
{
    public class Borders
    {
        public decimal Low;
        public decimal High;

        public Borders(decimal low, decimal high)
        {
            Low = low;
            High = high;
        }
    }

    public class ArithmeticArchiever
    {
        private HashSet<char> symbols = new();
        private Dictionary<char, Borders> symbolsFrequency = new();
        private Dictionary<char, decimal> symbolsCount = new();
        private string input;

        private decimal low = 0;
        private decimal high = 1;
        private decimal result;

        public string Compress(string data)
        {
            input = data;

            CalculateFrequency(data);

            CalculateResult();

            return result.ToString();
        }      

        public string Decompress(string data)
        {
            return DecodeResult();
        }

        private string DecodeResult()
        {
            var builder = new StringBuilder();

            for (var i = 0; i < input.Length; i++)
            {
                NewMethod(builder);
            }

            return builder.ToString();
        }

        private void NewMethod(StringBuilder builder)
        {
            foreach (var entry in symbolsFrequency)
            {
                var ch = entry.Key;
                var borders = entry.Value;

                if (result >= borders.Low
                    && result < borders.High)
                {
                    var diff = borders.High - borders.Low;
                    builder.Append(ch);
                    result = (result - borders.Low) / (diff);
                    break;
                }
            }
        }

        //private string DecodeResult()
        //{
        //    low = 0;
        //    high = 1;
        //    var builder = new StringBuilder();

        //    while (low < result)
        //    {
        //        var diff = high - low;

        //        foreach (var entry in symbolsFrequency)
        //        {
        //            var ch = entry.Key;
        //            var borders = entry.Value;
        //            var borderDiff = borders.High - borders.Low;
        //            var newLow = low + diff * borders.Low;
        //            var newHigh = newLow + borderDiff * diff;

        //            if (result >= newLow
        //                && result < newHigh)
        //            {
        //                low = newLow;
        //                high = newHigh;
        //                builder.Append(ch);
        //                break;
        //            }
        //        }
        //    }

        //    return builder.ToString();
        //}

        private void CalculateResult()
        {
            foreach (var ch in input)
            {
                var borders = symbolsFrequency[ch];
                var diff = high - low;
                var borderDiff = borders.High - borders.Low;
                low += diff * borders.Low;
                high = low + borderDiff * diff;
            }

            result = low;
        }

        private void CalculateFrequency(string data)
        {
            foreach (var ch in data)
            {
                symbols.Add(ch);
                symbolsCount[ch] = symbolsCount.ContainsKey(ch) ? symbolsCount[ch] + 1 : 1;
            }

            decimal previous = 0;
            foreach (var ch in symbols)
            {
                var newHigh = symbolsCount[ch] / input.Length + previous;
                symbolsFrequency[ch] = new Borders(previous, newHigh);
                previous = newHigh;
            }
        }
    }
}
