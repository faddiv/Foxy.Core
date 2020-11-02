using FluentAssertions;
using System;
using System.Linq;
using Xunit;

namespace CommonLibraries.Core.Text
{
    public class ArrayComparerTests
    {
        [Fact]
        public void ByteEquals_returns_true_for_nulls()
        {
            ArrayComparer.ByteEquals(null, null)
                .Should().BeTrue();
        }

        [Fact]
        public void ByteEquals_returns_true_for_ref_equals()
        {
            var arr = new ushort[] { 1, 2 };
            ArrayComparer.ByteEquals(arr, arr)
                .Should().BeTrue();
        }

        [Fact]
        public void ByteEquals_returns_false_if_left_is_null()
        {
            var arr = new ushort[] { 1, 2 };
            ArrayComparer.ByteEquals(null, arr)
                .Should().BeFalse();
        }

        [Fact]
        public void ByteEquals_returns_false_if_right_is_null()
        {
            var arr = new ushort[] { 1, 2 };
            ArrayComparer.ByteEquals(arr, null)
                .Should().BeFalse();
        }

        [Fact]
        public void ByteEquals_returns_false_for_different_size_array()
        {
            var arr = new ushort[] { 1, 2 };
            var arr2 = new ushort[] { 1, 2, 3 };
            ArrayComparer.ByteEquals(arr, arr2)
                .Should().BeFalse();
        }

        [Fact]
        public void ByteEquals_returns_true_if_elements_match()
        {
            for (int i = 1; i < 20; i++)
            {
                var arr = Enumerable.Range(1, i).Select(e => (ushort)e).ToArray();
                var arr2 = new ushort[arr.Length];
                Array.Copy(arr, arr2, arr.Length);
                ArrayComparer.ByteEquals(arr, arr2)
                    .Should().BeTrue();
            }
        }

        [Fact]
        public void ByteEquals_returns_false_if_elements_doesnt_match()
        {
            for (int i = 1; i < 20; i++)
            {
                var arr = Enumerable.Range(1, i).Select(e => (ushort)e).ToArray();
                var arr2 = new ushort[arr.Length];
                Array.Copy(arr, arr2, arr.Length);
                arr2[arr2.Length - 1] = 999;
                ArrayComparer.ByteEquals(arr, arr2)
                    .Should().BeFalse();
            }
        }
    }
}
