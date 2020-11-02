using CommonLibraries.Core.Text.TestHelpers;
using System;
using Xunit;

namespace CommonLibraries.Core.Text
{
    public class OrdinalTests : StringNaturalComparerCommonTests
    {
        public OrdinalTests() : base(
            StringNaturalComparer.Ordinal,
            StringComparer.Ordinal)
        {
        }

        [Theory]
        [InlineData("a", "A", 1)]
        [InlineData("á", "Á", 1)]
        [InlineData("ş", "Ş", 1)]
        [InlineData("ꚉ", "Ꚉ", 1)]
        public void Cases_not_ignored(string left, string right, int expected)
        {
            Comparer.Should().ResultWithSignFor(left, right, expected);
        }

        [Theory]
        [InlineData("[", "A", 1)]
        [InlineData("a", "[", 1)]
        public void Between_the_cases(string left, string right, int expected)
        {
            Comparer.Should().ResultWithSignFor(left, right, expected);
        }

        [Theory]
        [InlineData("F_", "FA", 1)]
        public void Non_equal_cases(string left, string right, int expected)
        {
            Comparer.Should().ResultWithSignFor(left, right, expected);
        }

    }
}
