using BenchmarkDotNet.Attributes;
using System.Collections.Generic;

namespace StringNaturalComparerNS
{
    [ArtifactsPath(".\\BigNumberBenchmarks")]
    public class BigNumberBenchmarks : BenchmarksBase
    {
        private readonly IComparer<string> stringNaturalComparer = CommonLibraries.Core.Text.StringNaturalComparer.Ordinal;
        private readonly IComparer<string> naturalSortExtension = NaturalSort.Extension.StringComparerNaturalSortExtension.WithNaturalSort(System.StringComparer.CurrentCulture);
        private readonly IComparer<string> pInvokeComparer = new PInvokeComparer();

        private string[][] dataSets = new[] {
            new[] {
            "9223372036854775801",
            "9223372036854775802",
            } ,new[] {
            "9223372036854775801",
            new string("9223372036854775801".ToCharArray())
            }
        };
        private string left;
        private string right;

        public enum Examples
        {
            LongNumberNE,
            LongNumberEQ
        }

        [Params(Examples.LongNumberNE, Examples.LongNumberEQ)]
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
    }
}
