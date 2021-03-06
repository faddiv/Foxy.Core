using BenchmarkDotNet.Attributes;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NaturalStringComparerNS
{
    [ArtifactsPath(".\\SortTextsBenchmarks")]
    public class SortTextsBenchmarks : BenchmarksBase
    {
        private readonly IComparer<string> naturalStringComparer = Foxy.Core.Text.NaturalStringComparer.InvariantCultureIgnoreCase;
        private readonly IComparer<string> naturalSortExtension = NaturalSort.Extension.NaturalSortExtension.WithNaturalSort(System.StringComparison.InvariantCultureIgnoreCase);
        private readonly IComparer<string> pInvokeComparer = new PInvokeComparer();
        private readonly IComparer<string> stringComparer = System.StringComparer.InvariantCultureIgnoreCase;

        private string[] stringsToSort;

        [GlobalSetup]
        public void Setup()
        {
            stringsToSort = File.ReadAllLines("sample-data.txt");
        }

        [Benchmark]
        public string[] NaturalStringComparer()
        {
            return stringsToSort.OrderBy(e => e, naturalStringComparer).ToArray();
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
