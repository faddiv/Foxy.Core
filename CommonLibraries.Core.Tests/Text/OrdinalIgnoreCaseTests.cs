using CommonLibraries.Core.Text.TestHelpers;
using FluentAssertions;
using System;
using Xunit;

namespace CommonLibraries.Core.Text
{
    public class OrdinalIgnoreCaseTests : StringNaturalComparerCommonTests
    {
        public OrdinalIgnoreCaseTests() : base(
            StringNaturalComparer.OrdinalIgnoreCase,
            StringComparer.OrdinalIgnoreCase)
        {
        }

        [Theory]
        [InlineData("F_", "FA", 1)]
        public void Non_equal_cases(string left, string right, int expected)
        {
            Comparer.Should().ResultWithSignFor(left, right, expected);
        }

        [Theory]
        [InlineData("a", "A", 0)]
        [InlineData("á", "Á", 0)]
        [InlineData("ş", "Ş", 0)]
        [InlineData("ꚉ", "Ꚉ", 0)]
        public void Cases_ignored(string left, string right, int expected)
        {
            Comparer.Should().ResultWithSignFor(left, right, expected);
        }

        [Theory]
        [InlineData("[", "A", 1)]
        [InlineData("[", "a", 1)]
        public void Between_the_cases(string left, string right, int expected)
        {
            Comparer.Should().ResultWithSignFor(left, right, expected);
        }

        [Theory]
        [InlineData("a2", "A10", -1)]
        public void Test_after_different_casing_natural(string left, string right, int expected)
        {
            Comparer.Should().ResultWithSignFor(left, right, expected);
        }
    }
}
