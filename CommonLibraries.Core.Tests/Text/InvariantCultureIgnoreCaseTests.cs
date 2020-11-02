using CommonLibraries.Core.Text.TestHelpers;
using FluentAssertions;
using System;
using Xunit;

namespace CommonLibraries.Core.Text
{
    public class InvariantCultureIgnoreCaseTests : StringNaturalComparerCommonTests
    {
        public InvariantCultureIgnoreCaseTests()
            : base(StringNaturalComparer.InvariantCultureIgnoreCase,
                  StringComparer.InvariantCultureIgnoreCase)
        {
        }

        [Theory]
        [InlineData("F_", "FA", -1)]
        [InlineData("F_", "Fa", -1)]
        public void Non_equal_cases(string left, string right, int expected)
        {
            Comparer.Should().ResultWithSignFor(left, right, expected);
        }


        [Theory]
        [InlineData("a", "A", 0)]
        [InlineData("á", "Á", 0)]
        [InlineData("ş", "Ş", 0)]
        [InlineData("ꚉ", "Ꚉ", 0)]
        [InlineData("abcd", "ABCD", 0)]
        public void Cases_ignored(string left, string right, int expected)
        {
            Comparer.Should().ResultWithSignFor(left, right, expected);
        }

        [Theory]
        [InlineData("A", "[")]
        [InlineData("a", "[")]
        public void Between_the_cases(string left, string right)
        {
            var expected = OrigComparer.Compare(left, right);
            Comparer.Should().ResultWithSignFor(left, right, expected);
        }


        [Theory]
        [InlineData("AbCd", "aBcD", 0)]
        [InlineData("aB10C", "Ab2c", 1)]
        [InlineData("Ab10Cd", "aB010cD", 1)]
        public void Multiple_case_difference(string left, string right, int expected)
        {
            Comparer.Should().ResultWithSignFor(left, right, expected);
        }

        [Theory]
        [InlineData("a2", "A10", -1)]
        public void Test_after_different_casing_natural(string left, string right, int expected)
        {
            Comparer.Should().ResultWithSignFor(left, right, expected);
        }

        [Fact]
        public void Comparers_should_work_the_same_when_comparing_with_InvariantCultureIgnoreCase()
        {
            // This is to ensure the benchmarks will work consistently in the given scenario.
            // The natural short part is not tested because the basic comparer doesnt support it.
            var stringComparer = StringComparer.InvariantCultureIgnoreCase;
            var naturalSortExtensionComparer =
                NaturalSort.Extension.StringComparerNaturalSortExtension
                .WithNaturalSort(StringComparer.InvariantCultureIgnoreCase);
            var stringNaturalComparer = StringNaturalComparer.InvariantCultureIgnoreCase;
            var pinvokeComparer = new PInvokeComparer();
            var text1 = "LOREM IPSUM DOLOR SIT AMET, CONSECTETUR ADIPISCING ELIT. 2 A";
            var text2 = "lorem ipsum dolor sit amet, consectetur adipiscing elit. 2 a";

            var scResult = stringComparer.Compare(text1, text2);
            var nsecResult = naturalSortExtensionComparer.Compare(text1, text2);
            var sncResult = stringNaturalComparer.Compare(text1, text2);
            var picResult = pinvokeComparer.Compare(text1, text2);

            nsecResult.Should().Be(scResult);
            sncResult.Should().Be(scResult);
            picResult.Should().Be(scResult);
        }
    }
}
