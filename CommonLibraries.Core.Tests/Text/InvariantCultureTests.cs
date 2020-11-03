using Foxy.Core.Text.TestHelpers;
using System;
using Xunit;

namespace Foxy.Core.Text
{
    public class InvariantCultureTests : NaturalStringComparerCommonTests
    {
        public InvariantCultureTests()
            : base(NaturalStringComparer.InvariantCulture,
                  StringComparer.InvariantCulture)
        {
        }

        [Theory]
        [InlineData("F_", "FA")]
        [InlineData("ᵣ", "ʀ")]
        public void Non_equal_cases(string left, string right)
        {
            var expected = OrigComparer.Compare(left, right);
            Comparer.Should().ResultWithSignFor(left, right, expected);
        }
    }
}
