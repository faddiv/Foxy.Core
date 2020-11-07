using BenchmarkDotNet.Attributes;

namespace NaturalStringComparerNS
{
    [ArtifactsPath(".\\EtcBenchmarks")]
    public class EtcBenchmarks : BenchmarksBase
    {
        public enum TestCase
        {
            InRange,
            OutFromRange
        }
        [Params(TestCase.InRange, TestCase.OutFromRange)]
        public TestCase TestCases;

        char ch;
        [GlobalSetup]
        public void Setup()
        {
            switch (TestCases)
            {
                case TestCase.InRange:
                    ch = '5';
                    break;
                default:
                    ch = 'a';
                    break;
            }
        }

        /// <summary>
        /// In the .net code this method is used to determine if a character is digit
        /// but the other one is faster.
        /// </summary>
        [Benchmark]
        public bool RangeMinus()
        {
            return (uint)(ch - '0') <= '9' - '0';
        }

        [Benchmark]
        public bool Range2Compare()
        {
            return '0' <= ch && ch <= '9';
        }
    }
}
