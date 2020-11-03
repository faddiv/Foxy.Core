using BenchmarkDotNet.Attributes;
using Foxy.Core.Text;
using System.Collections.Generic;

namespace NaturalStringComparerNS
{
    [ArtifactsPath(".\\CompareToBuildinComparerBenchmarks")]
    public class CompareToBuildinComparerBenchmarks : BenchmarksBase
    {
        private readonly IComparer<string> NaturalStringComparerOrdinal =
            NaturalStringComparer.Ordinal;
        private readonly IComparer<string> stringComparer = 
            System.StringComparer.Ordinal;

        private readonly IComparer<string> NaturalStringComparerOrdinalIgnoreCase =
            NaturalStringComparer.OrdinalIgnoreCase;
        private readonly IComparer<string> stringComparerOrdinalIgnoreCase =
            System.StringComparer.OrdinalIgnoreCase;

        private readonly IComparer<string> NaturalStringComparerInvariant =
            NaturalStringComparer.InvariantCulture;
        private readonly IComparer<string> stringComparerInvariant =
            System.StringComparer.InvariantCulture;

        private readonly IComparer<string> NaturalStringComparerInvariantIgnoreCase =
            NaturalStringComparer.InvariantCultureIgnoreCase;
        private readonly IComparer<string> stringComparerInvariantIgnoreCase =
            System.StringComparer.InvariantCultureIgnoreCase;

        private readonly string[][] dataSets = new[] {
            new[] {
            "Lorem ipsum dolor sit amet, consectetur adipiscing elit.a",
            "Lorem ipsum dolor sit amet, consectetur adipiscing elit.b"
            } ,new[] {
            "Lorem ipsum dolor sit amet, consectetur adipiscing elit.a",
            new string("Lorem ipsum dolor sit amet, consectetur adipiscing elit.a".ToCharArray())
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
        public int NaturalOrdinal()
        {
            return NaturalStringComparerOrdinal.Compare(left, right);
        }

        [Benchmark]
        public int BasicOrdinal()
        {
            return stringComparer.Compare(left, right);
        }

        [Benchmark]
        public int NaturalOrdinalIgnoreCase()
        {
            return NaturalStringComparerOrdinalIgnoreCase.Compare(left, right);
        }

        [Benchmark]
        public int BasicOrdinalIgnoreCase()
        {
            return stringComparerOrdinalIgnoreCase.Compare(left, right);
        }

        [Benchmark]
        public int NaturalInvariant()
        {
            return NaturalStringComparerInvariant.Compare(left, right);
        }

        [Benchmark]
        public int BasicInvariant()
        {
            return stringComparerInvariant.Compare(left, right);
        }

        [Benchmark]
        public int NaturalInvariantIgnoreCase()
        {
            return NaturalStringComparerInvariantIgnoreCase.Compare(left, right);
        }

        [Benchmark]
        public int BasicInvariantIgnoreCase()
        {
            return stringComparerInvariantIgnoreCase.Compare(left, right);
        }
    }
}
