using FluentAssertions;
using System.Globalization;
using Xunit;

namespace CommonLibraries.Core.Text
{
    public class MappingCacheTests
    {
        [Fact]
        public void CreateMappingTable_returns_the_same_array_as_prev_if_has_the_same_order()
        {
            var invariant = MappingCache.GetMapping(CultureInfo.InvariantCulture, false);
            var sameAsInvariant = MappingCache.GetMapping(CultureInfo.GetCultureInfo("en-US"), false);
            var sameAsInvariant2 = MappingCache.GetMapping(CultureInfo.GetCultureInfo("en-GB"), false);
            invariant.Should().BeSameAs(sameAsInvariant);
            invariant.Should().BeSameAs(sameAsInvariant2);
        }
    }
}
