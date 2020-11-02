using CommonLibraries.Core.Text.TestHelpers;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace CommonLibraries.Core.Text
{
    public abstract class StringNaturalComparerCommonTests
    {
        protected StringNaturalComparer Comparer { get; }

        protected StringComparer OrigComparer { get; }

        protected static string[] allCh { get; }

        static StringNaturalComparerCommonTests()
        {

            allCh = new string[1 + char.MaxValue - char.MinValue];
            for (int ch1 = char.MinValue; ch1 <= char.MaxValue; ch1++)
            {
                // Add unicode value so there will be exact order in every case.
                allCh[ch1 - char.MinValue] = $"{(char)ch1}";
            }
        }

        protected StringNaturalComparerCommonTests(
            StringNaturalComparer comparer,
            StringComparer origComparer)
        {
            Comparer = comparer;
            OrigComparer = origComparer;
        }

        [Theory]
        [InlineData("", "a", 1)]
        [InlineData("a", "", -1)]
        [InlineData(null, "a", 1)]
        [InlineData("a", null, -1)]
        [InlineData(null, null, 0)]
        [InlineData("", "", 0)]
        public void Null_and_empty(string left, string right, int expected)
        {
            Comparer.Should().ResultWithSignFor(left, right, expected);
        }


        [Theory]
        [InlineData(null, null, 0)]
        [InlineData("", "", 0)]
        [InlineData("a", "a", 0)]
        [InlineData("aa", "aa", 0)]
        [InlineData("aaaa", "aaaa", 0)]
        public void Equal_cases(string left, string right, int expected)
        {
            Comparer.Should().ResultWithSignFor(left, right, expected);
        }

        [Fact]
        public void Equal_when_not_reference_equal()
        {
            Comparer.Should().ResultWithSignFor("asdf", new string("asdf"), 0);
        }

        [Theory]
        [InlineData("aaaa", "aa", 1)]
        [InlineData("a", "b", -1)]
        [InlineData("b", "a", 1)]
        [InlineData("0", "1", -1)]
        [InlineData("1", "0", 1)]
        [InlineData("a2", "a10", -1)]
        [InlineData("10.0401", "10.022", 1)]
        [InlineData("10.01244", "10.01245", -1)]
        [InlineData("40", "052", -1)]
        [InlineData("10.0401", "10.042", 1)]
        [InlineData("MD1366", "MD136_", 1)]
        [InlineData("MD13666", "MD136__", 1)]
        [InlineData("a", "100", 1)]
        [InlineData("1.1.100", "1.1.10", 1)]
        public void General_Non_equal_cases(string left, string right, int expected)
        {
            Comparer.Should().ResultWithSignFor(new string(left), new string(right), expected);
        }

        [Theory]
        [InlineData("9223372036854775801", "9223372036854775802", -1)]
        [InlineData("9223372036854775801", "9223372036854775801", 0)]
        //[InlineData("99223372036854775801", "89223372036854775802", -1)] //Should change implementation?
        public void BigNumber_case(string left, string right, int expected)
        {
            Comparer.Should().ResultWithSignFor(new string(left), new string(right), expected);
        }


        [Theory]
        [InlineData("280a", "2801", -1)]
        [InlineData("2801", "280a", 1)]
        [InlineData("2000a", "20000a", -1)]
        [InlineData("20000a", "2000a", 1)]
        [InlineData("200X ", "20X P", 1)]
        [InlineData("040a", "40a", -1)]
        [InlineData("40a", "040a", 1)]
        [InlineData("40a10", "040a2", 1)]
        [InlineData("10.0401", "10.042", 1)]
        public void Check_numbers_from_the_beggining(string left, string right, int expected)
        {
            Comparer.Should().ResultWithSignFor(new string(left), new string(right), expected);
        }


        [Theory]
        [InlineData("040a", "40a", -1)]
        [InlineData("40b", "040a", 1)]
        [InlineData("01alpha.sgi", "001alpha.sgi", 1)]
        public void Sortable_characters_after_equal_number_with_different_zeroes(string left, string right, int expected)
        {
            Comparer.Should().ResultWithSignFor(new string(left), new string(right), expected);
        }

        [Theory]
        [InlineData("01a.sgi", "001a.sgi", 1)]
        [InlineData("01al.sgi", "001al.sgi", 1)]
        [InlineData("01alp.sgi", "001alp.sgi", 1)]
        [InlineData("01alph.sgi", "001alph.sgi", 1)]
        [InlineData("01alpha.sgi", "001alpha.sgi", 1)]
        [InlineData("01alphaa.sgi", "001alphaa.sgi", 1)]
        [InlineData("01alphaab.sgi", "001alphaab.sgi", 1)]
        [InlineData("01alphaabc.sgi", "001alphaabc.sgi", 1)]
        [InlineData("01alphaabcd.sgi", "001alphaabcd.sgi", 1)]
        [InlineData("a", "b", -1)]
        [InlineData("aa", "ab", -1)]
        [InlineData("aaa", "aab", -1)]
        [InlineData("aaaa", "aaab", -1)]
        [InlineData("aaaaa", "aaaab", -1)]
        [InlineData("aaaaaa", "aaaaab", -1)]
        [InlineData("aaaaaaa", "aaaaaab", -1)]
        [InlineData("aaaaaaaa", "aaaaaaab", -1)]
        [InlineData("aaaaaaaaa", "aaaaaaaab", -1)]
        [InlineData("aaaaaaaaaa", "aaaaaaaaab", -1)]
        [InlineData("aaaaaaaaaaa", "aaaaaaaaaab", -1)]
        [InlineData("aaaaaaaaaaaa", "aaaaaaaaaaab", -1)]
        public void MoveNextDifferenceOrdinal_working_correctly_test(string left, string right, int expected)
        {
            Comparer.Should().ResultWithSignFor(new string(left), new string(right), expected);
        }

        [Theory]
        [InlineData("01", "001", 1)]
        [InlineData("01a", "001a", 1)]
        public void If_only_zeroes_differ_in_number_then_length_decides(string left, string right, int expected)
        {
            Comparer.Should().ResultWithSignFor(new string(left), new string(right), expected);
        }

        [Fact]
        public void Natural_comparer_returns_same_as_original()
        {
            var actual = allCh
                .OrderBy(s => s, Comparer)
                .ThenBy(s => (int)s[0])
                .ToList();
            var expected = allCh
                .OrderBy(s => s, OrigComparer)
                .ThenBy(s => (int)s[0])
                .ToList();
            int i = 0;
            for (; i < allCh.Length; i++)
            {
                if (actual[i] != expected[i])
                    break;
            }
            if (i == allCh.Length)
                return;
            var actNeighbour = Neighbour(actual, i);
            var expNeighbour = Neighbour(expected, i);
            actual[i].Should().Be(expected[i],
                $"unicode actual vs expected at index {i}: " +
                $"0x{(int)actual[i][0]:x4} 0x{(int)expected[i][0]:x4} Neighbour: '{actNeighbour}' vs '{expNeighbour}'");
        }

        private string Neighbour(List<string> actual, int i)
        {
            var size = 5;
            var max = Math.Min(actual.Count - 1, i + size);
            var range = actual.GetRange(i, max - i);
            return string.Join(" ", range);
        }

        [Theory]
        [InlineData("ф", "Ф")]
        [InlineData("ⴆ", "Ⴆ")]
        [InlineData("a", "A")]
        [InlineData("á", "Á")]
        [InlineData("ä", "Ä")]
        public void Works_same_as_normal_string_comparer_on_unicode_characters(string left, string right)
        {
            // This test confirms that GlobalizationMode.Invariant == false
            var expected = OrigComparer.Compare(left, right);
            Comparer.Should().ResultWithSignFor(left, right, expected);
        }

        [Theory]
        [InlineData("ᵣ", "ʀ")]
        [InlineData("ᵣ", "Ʀ")]
        [InlineData("ʀ", "ᵣ")]
        [InlineData("ʀ", "Ʀ")]
        [InlineData("Ʀ", "ʀ")]
        [InlineData("Ʀ", "ᵣ")]
        public void Works_same_as_normal_string_comparer_on_rU002Ds(string left, string right)
        {
            // Tests if the relations of ᵣ, ʀ and Ʀ are the same as OrigComparer.
            var expected = OrigComparer.Compare(left, right);
            var actual = Comparer.Compare(left, right);
            if (expected != actual)
            {
                Comparer.Should().ResultWithSignFor(left, right, expected);
            }

        }


        [Theory]
        [InlineData("ᵣ")]
        [InlineData("ʀ")]
        [InlineData("Ʀ")]
        public void All_charater_has_the_same_elation_to_rU002Ds(string right)
        {
            foreach (var left in allCh)
            {
                var expected = OrigComparer.Compare(left, right);
                var actual = Comparer.Compare(left, right);
                if (expected != actual)
                {
                    Comparer.Should().ResultWithSignFor(left, right, expected);
                }
            }
            // Tests if the relations of ᵣ, ʀ and Ʀ are the same as OrigComparer.

        }
    }
}