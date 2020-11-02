using BenchmarkDotNet.Attributes;
using CommonLibraries.Core.Text;
using System.Collections.Generic;

namespace StringNaturalComparerNS
{
    [ArtifactsPath(".\\StringNaturalComparerModes")]
    public class StringNaturalComparerModes : BenchmarksBase
    {
        private readonly IComparer<string> currentCulture = StringNaturalComparer.CurrentCulture;
        private readonly IComparer<string> currentCultureIgnoreCase = StringNaturalComparer.CurrentCultureIgnoreCase;
        private readonly IComparer<string> ordinal = StringNaturalComparer.Ordinal;
        private readonly IComparer<string> ordinalIgnoreCase = StringNaturalComparer.OrdinalIgnoreCase;
        private readonly string[][] dataSets = new[] {
            new[] {
            "Lorem ipsum dolor sit amet, consectetur adipiscing elit. 2 a",
            "Lorem ipsum dolor sit amet, consectetur adipiscing elit. 10 a"
            } ,new[] {
            "Lorem ipsum dolor sit amet, consectetur adipiscing elit. 10 a",
            new string("Lorem ipsum dolor sit amet, consectetur adipiscing elit. 10 a".ToCharArray())
            }
        };
        private string left;
        private string right;

        public enum Examples
        {
            LongTextNumberNE,
            LongTextNumberEQ
        }

        [Params(Examples.LongTextNumberNE, Examples.LongTextNumberEQ)]
        public Examples Pairs;

        [GlobalSetup]
        public void Setup()
        {
            left = dataSets[(int)Pairs][0];
            right = dataSets[(int)Pairs][1];
        }

        [Benchmark]
        public int CurrentCulture()
        {
            return currentCulture.Compare(left, right);
        }

        [Benchmark]
        public int CurrentCultureIgnoreCase()
        {
            return currentCultureIgnoreCase.Compare(left, right);
        }

        [Benchmark]
        public int Ordinal()
        {
            return ordinal.Compare(left, right);
        }

        [Benchmark(Baseline = true)]
        public int OrdinalIgnoreCase()
        {
            return ordinalIgnoreCase.Compare(left, right);
        }
    }
}
