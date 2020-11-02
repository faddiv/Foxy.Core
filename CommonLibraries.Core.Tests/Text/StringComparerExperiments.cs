using FluentAssertions;
using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Xunit;

namespace CommonLibraries.Core.Text
{
    public partial class StringComparerExperiments
    {
        protected static string[] allCh { get; }

        static StringComparerExperiments()
        {

            allCh = new string[char.MaxValue - char.MinValue];
            for (char ch1 = char.MinValue; ch1 < char.MaxValue; ch1++)
            {
                allCh[ch1 - char.MinValue] = $"{ch1}";
            }
        }
        [Fact]
        public void OrdinalIgnoreCase_converts_lowercase_to_uppercase()
        {
            var comparer = StringComparer.OrdinalIgnoreCase;
            comparer.Compare("[", "A").Should().Be(26);
            comparer.Compare("[", "a").Should().Be(26);
            ('[' - 'A').Should().Be(26);
        }

        [Fact]
        public void Ordinal_case_insensitive_handles_english_characters()
        {
            var comparer = StringComparer.OrdinalIgnoreCase;
            comparer.Compare("a", "A").Should().Be(0);
        }

        [Fact]
        public void Ordinal_case_insensitive_converts_lowercase_to_uppercase_in_national_characters()
        {
            var comparer = StringComparer.OrdinalIgnoreCase;
            comparer.Compare("×", "Á").Should().Be(1); // \x00D7
            comparer.Compare("×", "á").Should().Be(1);
            ('×' - 'Á').Should().Be(22);
            ('×' - 'á').Should().Be(-10);
        }

        [Fact]
        public void Ordinal_case_insensitive_correct_in_latin1()
        {
            var comparer = StringComparer.OrdinalIgnoreCase;
            comparer.Compare("á", "Á").Should().Be(0);
        }

        [Fact]
        public void Ordinal_case_insensitive_differentiates_accent_characters()
        {
            var comparer = StringComparer.OrdinalIgnoreCase;
            comparer.Compare("á", "a").Should().Be(1);
        }

        [Fact]
        public void Ordinal_doesnt_consider_accent_culture_spec_order()
        {
            var comparer = StringComparer.Ordinal;
            var abc = new[] { "a", "á", "b", "c" };
            var sorted = abc.OrderBy(e => e, comparer).ToArray();
            sorted.Should().ContainInOrder("a", "b", "c", "á");
        }

        [Fact]
        public void InvariantCulture_consider_accent_culture_spec_order()
        {
            var comparer = StringComparer.InvariantCulture;
            var abc = new[] { "a", "á", "b", "c" };
            var sorted = abc.OrderBy(e => e, comparer).ToArray();
            sorted.Should().ContainInOrder("a", "á", "b", "c");
        }

        [Fact]
        public void InvariantCulture_Turkish_culture_i()
        {
            var iList = new[] { "İ", "I", "ı", "i" };
            "ı".ToUpperInvariant().Should().Be("ı");
            "i".ToUpperInvariant().Should().Be("I");

            "ı".ToUpper(CultureInfo.GetCultureInfo("tr-TR")).Should().Be("I");
            "i".ToUpper(CultureInfo.GetCultureInfo("tr-TR")).Should().Be("İ");
        }

        [Fact]
        public void InvariantCultureIgnoreCase_doesnt_handle_Turkish_i()
        {
            var comparer = StringComparer.InvariantCultureIgnoreCase;
            comparer.Compare("ı", "I").Should().NotBe(0);
            comparer.Compare("i", "I").Should().Be(0);
            comparer.Compare("i", "İ").Should().NotBe(0);
        }

        [Fact]
        public void TurkishCultureIgnoreCase_handle_Turkish_i()
        {
            var comparer = StringComparer.Create(CultureInfo.GetCultureInfo("tr-TR"), true);
            comparer.Compare("ı", "I").Should().Be(0);
            comparer.Compare("i", "İ").Should().Be(0);
            comparer.Compare("i", "I").Should().NotBe(0);
            comparer.Compare("ı", "İ").Should().NotBe(0);
        }

        [Fact]
        public void OrdinalIgnoreCase_case_sort()
        {
            var comparer = StringComparer.Ordinal;
            var abc = new[] { "A", "a", "b", "B" };
            var sorted = abc.OrderBy(e => e, comparer).ToArray();
            sorted.Should().ContainInOrder("A", "B", "a", "b");
        }

        [Fact]
        public void InvariantCultureIgnoreCase_case_sort()
        {
            var comparer = StringComparer.InvariantCulture;
            var abc = new[] { "A", "a", "b", "B" };
            var sorted = abc.OrderBy(e => e, comparer).ToArray();
            sorted.Should().ContainInOrder("a", "A", "b", "B");
        }

        [Fact]
        public void Turk_vs_invariant()
        {
            var invariant = StringComparer.InvariantCulture;
            var turkish = StringComparer.Create(
                CultureInfo.GetCultureInfo("tr-TR"), true);

            invariant.Compare("ş", "ṥ").Should().Be(0);
            turkish.Compare("ş", "ṥ").Should().NotBe(0);

            invariant.Compare("ş", "ꞅ").Should().NotBe(0);
            turkish.Compare("ş", "ꞅ").Should().Be(0);
        }

        [Fact(Skip = "Windows error. Check sometimes if fixed.")]
        public void Uvular_trill_does_not_equal_to_subscript_r_if_case_ignored()
        {
            var invariantIgnoreCase = StringComparer.InvariantCultureIgnoreCase;
         
            invariantIgnoreCase.Compare("ᵣ", "ʀ").Should().NotBe(0);

            invariantIgnoreCase.Compare("ᵣ", "Ʀ").Should().NotBe(0);

            invariantIgnoreCase.Compare("Ʀ", "ʀ").Should().Be(0);
        }

        [Fact(Skip = "Windows error. Check sometimes if fixed.")]
        public void ToUpperInvariant_works_consistently_with_InvariantCultureIgnoreCase()
        {
            var invariantIgnoreCase = StringComparer.InvariantCultureIgnoreCase;

            var allCharacter = new string[char.MaxValue];
            for (char ch1 = char.MinValue; ch1 < char.MaxValue; ch1++)
            {
                allCharacter[ch1] = $"{ch1}";
            }
            var actual = allCharacter
                .Select(character => new { character, code = (int)character[0] })
                .OrderBy(e => e.character, invariantIgnoreCase)
                .ThenBy(e => e.code)
                .ToList();
            var expected = allCharacter
                .Select(character => new { character = character.ToUpperInvariant(), code = (int)character[0] })
                .OrderBy(e => e.character, invariantIgnoreCase)
                .ThenBy(e => e.code)
                .ToList();
            int i = 0;
            for (; i < allCharacter.Length; i++)
            {
                if (actual[i].code != expected[i].code)
                    break;
            }
            if (i == allCharacter.Length)
                return;
            actual[i].Should().Be(expected[i],
                $"At index {i} the same character should be");
        }

        [Fact]
        public void Simplified_chinese_vs_invariant()
        {
            var invariant = StringComparer.InvariantCulture;
            var cultureSpec = StringComparer.Create(
                CultureInfo.GetCultureInfo("zh-Hans"), true);

            invariant.Compare("⺟", "母").Should().Be(0);
            cultureSpec.Compare("⺟", "母").Should().NotBe(0);
        }

        [Fact]
        public void Equal_groups()
        {
            var invariantCulture = allCh
                .GroupBy(c => c, StringComparer.InvariantCulture)
                .Select(e => e.ToArray())
                .Where(e => e.Length > 1)
                //.Where(e => e.Contains("⽻") && e.Contains("羽"))
                .ToList();
            var culture = allCh.GroupBy(c => c, 
                StringComparer.Create(CultureInfo.GetCultureInfo("pl"), false))
                .Select(e => e.ToArray())
                .Where(e => e.Length > 1)
                //.Where(e => e.Contains("⽻") && e.Contains("羽"))
                .ToList();
            var cultureSpecific = culture.Except(invariantCulture, new StringArrayComparer<string>())
                .ToList();
            cultureSpecific.Should().HaveCountGreaterThan(-1);

            var cultureExcept = invariantCulture.Except(culture, new StringArrayComparer<string>())
                .ToList();
            cultureExcept.Should().HaveCountGreaterThan(-1);
        }

        [Fact]
        public void Digits()
        {
            var digits = allCh
                .Where(e => char.IsDigit(e, 0))
                .ToList();
            var digitsCategory = digits
                .Select(e => new
                {
                    ch = e,
                    cat = char.GetUnicodeCategory(e, 0)
                })
                .ToList();
            digits.Should().NotBeEmpty();
            digitsCategory.Should().NotBeEmpty();
        }
        // Tamil : 23 -> ௨௰௩ 2 10 3;  234 -> 2 100 3 10 4; 10 -> 10 (weird)
        // Hindi : 23 -> 2 3; 234 -> 2 3 4 (normal)
        [Fact]
        public void Ordinal_ignore_case_compare_national_chars()
        {
            var ordinalIgnoreCase = StringComparer.OrdinalIgnoreCase;
            ordinalIgnoreCase.Compare("á", "Á").Should().Be(0);
            ordinalIgnoreCase.Compare("ş", "Ş").Should().Be(0);
            ordinalIgnoreCase.Compare("ꚉ", "Ꚉ").Should().Be(0);
        }

        [Fact]
        public void GlobalizationMode_Invariant_is_false()
        {

            var gm = Type.GetType("System.Globalization.GlobalizationMode");
            var invariantProperty = gm.GetProperty("Invariant", BindingFlags.Static | BindingFlags.NonPublic);
            var result = (bool)invariantProperty.GetValue(null);
            result.Should().BeFalse();
        }

        [Fact]
        public void CharComparisons()
        {
            var culture = CultureInfo.InvariantCulture;
            var expected = culture.CompareInfo.Compare("Ʀ", "ʀ");
            var actual = Math.Sign('ʀ' - 'Ʀ');
            actual.Should().Be(expected);
        }


        [Fact]
        public void Sort_Is_Success()
        {
            var comparer = StringComparer.InvariantCulture;
            var comparer2 = StringComparer.InvariantCulture;
            var sort2 = allCh
                .OrderBy(c => c, comparer2)
                .ThenBy(e => (ushort)e[0])
                .ToList();
            var sort = allCh
                .OrderBy(c => c, comparer);
            int index = -1;
            string lastItem = null;
            var mapping = new ushort[1 + char.MaxValue - char.MinValue];
            foreach (var item in sort)
            {
                if (!comparer.Equals(lastItem, item))
                {
                    index++;
                    lastItem = item;
                }
                mapping[item[0]] = (ushort)index;
            }
            for (int i = 1; i < sort2.Count; i++)
            {
                if (mapping[sort2[i - 1][0]] > mapping[sort2[i][0]])
                {
                    mapping[sort2[i - 1][0]].Should().BeLessOrEqualTo(mapping[sort2[i][0]]);
                }
                comparer.Compare(sort2[i - 1], sort2[i]).Should().BeLessOrEqualTo(0);
            }
        }

        [Fact]
        public void Group_works()
        {
            // Currently cant group because GetHashCode yields different results
            // and GroupBy demands same hashcode.
            var comparer = StringComparer.InvariantCulture;
            var strs = new string[] { "\uFC5E", "\u0614", "\uFC5F", "\u1B6F" };
            comparer.Equals(strs[0], strs[1]).Should().BeTrue();
            comparer.Equals(strs[1], strs[2]).Should().BeFalse();
            comparer.Equals(strs[2], strs[3]).Should().BeTrue();
            comparer.Equals(strs[0], strs[3]).Should().BeFalse();
            comparer.Equals(strs[1], strs[3]).Should().BeFalse();
            var result = strs.GroupBy(e => e, comparer)
                .Select(e => new { e.Key, List = e.ToList()})
                .ToList();
            result.Should().HaveCount(4);
        }

        [Fact]
        public void ToUpperInvariant_workings()
        {
            // Unicode of the wrong chars.
            "ᵣ".Should().Be("\u1D63");
            "ʀ".Should().Be("\u0280");
            "Ʀ".Should().Be("\u01A6");
            //Correct to upper transformation.
            char.ToUpperInvariant('ᵣ').Should().NotBe('Ʀ');
            char.ToUpperInvariant('ʀ').Should().Be('Ʀ');
        }

        [Fact]
        public void ToUpperInvariant_workings_on_title_case()
        {
            var dz = "\u01F3"[0];
            var upperDZ = char.ToUpperInvariant(dz);
            upperDZ.Should().Be("\u01F1"[0]);
        }

        [Fact]
        public void ToUpperInvariant_workings_on_compound_letter()
        {
            var squarecm = "\u339D";
            var result = squarecm.ToUpperInvariant();
            result.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void Reversed_relations_between_normal_and_ignore_case()
        {
            var ch1 = "\uFE63";
            var ch2 = "\uFF0D";
            var inv = StringComparer.InvariantCulture;
            var invIC = StringComparer.InvariantCultureIgnoreCase;
            inv.Compare(ch1, ch2).Should().Be(1);
            invIC.Compare(ch1, ch2).Should().Be(-1);
            ch1.ToUpperInvariant().Should().Be(ch1);
            ch2.ToUpperInvariant().Should().Be(ch2);
            var ci = CultureInfo.InvariantCulture.CompareInfo;
            ci.Compare(ch1, ch2, CompareOptions.None).Should().Be(1);
            ci.Compare(ch1, ch2, CompareOptions.IgnoreCase).Should().Be(-1);
        }
    }
}
