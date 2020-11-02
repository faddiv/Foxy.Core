using BenchmarkDotNet.Attributes;
using System.Collections.Generic;

namespace StringNaturalComparerNS
{
    [ArtifactsPath(".\\StringComparerBenchmarks")]
    public class StringComparerBenchmarks : BenchmarksBase
    {
        private readonly IComparer<string> stringNaturalComparer = CommonLibraries.Core.Text.StringNaturalComparer.InvariantCultureIgnoreCase;
        private readonly IComparer<string> naturalSortExtension = NaturalSort.Extension.StringComparerNaturalSortExtension.WithNaturalSort(System.StringComparer.InvariantCultureIgnoreCase);
        private readonly IComparer<string> pInvokeComparer = new PInvokeComparer();
        private readonly IComparer<string> stringComparer = System.StringComparer.InvariantCultureIgnoreCase;
        private readonly string[][] dataSets = new[] {
            new[] {
            "Lorem ipsum dolor sit amet, consectetur adipiscing elit. 2 a",
            "Lorem ipsum dolor sit amet, consectetur adipiscing elit. 10 a"
            } ,new[] {
            "LOREM IPSUM DOLOR SIT AMET, CONSECTETUR ADIPISCING ELIT. 2 A",
            "lorem ipsum dolor sit amet, consectetur adipiscing elit. 10 a"
            }
        };
        private string left;
        private string right;

        public enum Examples
        {
            OrdinalPerf,
            ToUpperPerf
        }

        [Params(Examples.OrdinalPerf, Examples.ToUpperPerf)]
        public Examples Pairs;

        [GlobalSetup]
        public void Setup()
        {
            left = dataSets[(int)Pairs][0];
            right = dataSets[(int)Pairs][1];
        }

        [Benchmark]
        public int StringNaturalComparer()
        {
            return stringNaturalComparer.Compare(left, right);
        }

        [Benchmark]
        public int NaturalSortExtension()
        {
            return naturalSortExtension.Compare(left, right);
        }

        [Benchmark(Baseline = true)]
        public int PInvokeComparer()
        {
            return pInvokeComparer.Compare(left, right);
        }

        [Benchmark()]
        public int StringComparer()
        {
            return stringComparer.Compare(left, right);
        }
    }
}
