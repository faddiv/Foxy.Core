using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace StringNaturalComparerNS
{
    [ArtifactsPath(".\\SortNumbersBenchmarks")]
    public class SortNumbersBenchmarks : BenchmarksBase
    {
        private readonly IComparer<string> stringNaturalComparer = CommonLibraries.Core.Text.StringNaturalComparer.InvariantCultureIgnoreCase;
        private readonly IComparer<string> naturalSortExtension = NaturalSort.Extension.StringComparerNaturalSortExtension.WithNaturalSort(System.StringComparer.InvariantCultureIgnoreCase);
        private readonly IComparer<string> pInvokeComparer = new PInvokeComparer();
        private readonly IComparer<string> stringComparer = System.StringComparer.InvariantCultureIgnoreCase;

        private string[] stringsToSort;

        [GlobalSetup]
        public void Setup()
        {
            var list = new List<ulong>(10000);
            var rnd = new Random(42);
            for (int i = 0; i < 1000; i++)
            {
                ulong val = NextLong(rnd);
                for (ulong j = 0; j < 5; j++)
                {
                    list.Add(val + j);
                    list.Add(val + j);
                }
            }
            stringsToSort = list.Select(e => e.ToString()).ToArray();
            var fileName = "numbers.txt";
            if (File.Exists(fileName))
                return;
            Console.WriteLine("Write file: {0}", Path.GetFullPath(fileName));
            File.WriteAllLines(fileName, stringsToSort);
        }

        private ulong NextLong(Random rnd)
        {
            var b = new byte[sizeof(long)];
            rnd.NextBytes(b);
            var num = BitConverter.ToUInt64(b, 0);
            if (num > ulong.MaxValue - 5)
                return ulong.MaxValue - 5;
            return num;
        }

        [Benchmark]
        public string[] StringNaturalComparer()
        {
            return stringsToSort.OrderBy(e => e, stringNaturalComparer).ToArray();
        }

        [Benchmark]
        public string[] NaturalSortExtension()
        {
            return stringsToSort.OrderBy(e => e, naturalSortExtension).ToArray();
        }

        [Benchmark(Baseline = true)]
        public string[] PInvokeComparer()
        {
            return stringsToSort.OrderBy(e => e, pInvokeComparer).ToArray();
        }

        [Benchmark()]
        public string[] StringComparer()
        {
            return stringsToSort.OrderBy(e => e, stringComparer).ToArray();
        }
    }
}
