using CommonLibraries.Core.Text.TestHelpers;
using System.Globalization;
using Xunit;

namespace CommonLibraries.Core.Text
{
    public class TurkishCultureTests
    {
        private static readonly CultureInfo turkish =
            CultureInfo.GetCultureInfo("tr-TR");
        private StringNaturalComparer Comparer { get; }

        public TurkishCultureTests()
        {
            Comparer = StringNaturalComparer.Create(turkish, true);
        }

        [Theory]
        [InlineData("ı", "I", 0)]
        [InlineData("i", "İ", 0)]
        public void Turkish_i(string left, string right, int expected)
        {
            Comparer.Should().ResultWithSignFor(left, right, expected);
        }
    }
}
