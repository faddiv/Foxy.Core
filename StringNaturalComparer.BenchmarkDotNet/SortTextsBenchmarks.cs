using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace StringNaturalComparerNS
{
    [ArtifactsPath(".\\SortTextsBenchmarks")]
    public class SortTextsBenchmarks : BenchmarksBase
    {
        private readonly IComparer<string> stringNaturalComparer = CommonLibraries.Core.Text.StringNaturalComparer.InvariantCultureIgnoreCase;
        private readonly IComparer<string> naturalSortExtension = NaturalSort.Extension.StringComparerNaturalSortExtension.WithNaturalSort(System.StringComparer.InvariantCultureIgnoreCase);
        private readonly IComparer<string> pInvokeComparer = new PInvokeComparer();
        private readonly IComparer<string> stringComparer = System.StringComparer.InvariantCultureIgnoreCase;

        private string[] stringsToSort;

        [GlobalSetup]
        public void Setup()
        {
            stringsToSort = File.ReadAllLines("sample-data.txt");
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
