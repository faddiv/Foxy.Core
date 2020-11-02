using FluentAssertions;
using System;
using Xunit;

namespace CommonLibraries.Core.Text
{
    public class VectorKeyTests
    {
        [Fact]
        public void ConstructorRuns()
        {
            Func<VectorKey> f = () => new VectorKey(new ushort[] { 1, 2 });
            f.Should().NotThrow();
        }

        [Fact]
        public void GetHashCode_returns_hash_code()
        {
            var v = new VectorKey(new ushort[] { 1, 2 });

            v.GetHashCode().Should().NotBe(0);
        }

        [Fact]
        public void GetHashCode_returns_different_ordered_vector_different_hash_code()
        {
            var v1 = new VectorKey(new ushort[] { 1, 2 });
            var v2 = new VectorKey(new ushort[] { 2, 1 });

            v1.GetHashCode().Should().NotBe(v2.GetHashCode());
        }

        [Fact]
        public void Equals_returns_equals_for_vectors_with_same_elements()
        {
            var v1 = new VectorKey(new ushort[] { 1, 2 });
            var v2 = new VectorKey(new ushort[] { 1, 2 });

            v1.Equals(v2).Should().BeTrue();
        }
    }
}
