using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace CommonLibraries.Core.Collections
{
    public class CollectionExtensionsTests
    {
        [Fact]
        public void AddRange_adds_multiple_elements()
        {
            // Arrange
            ICollection<int> list = Enumerable.Range(0, 3).ToList();
            var newElements = Enumerable.Range(3, 3);
            var expected = Enumerable.Range(0, 6).ToList();

            // Act
            list.AddRange(newElements);

            // Assert
            list.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void AddRange_requires_the_list()
        {
            // Arrange
            ICollection<int> list = null;
            var newElements = Enumerable.Range(3, 3);
            Action action = () => list.AddRange(newElements);

            // Act
            // Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void AddRange_requires_the_collection()
        {
            // Arrange
            ICollection<int> list = Enumerable.Range(3, 3).ToList();
            IEnumerable<int> newElements = null;
            Action action = () => list.AddRange(newElements);

            // Act
            // Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void AddRange_returns_the_collection()
        {
            // Arrange
            ICollection<int> list = Enumerable.Range(0, 3).ToList();
            var newElements = Enumerable.Range(3, 3);

            // Act
            var result = list.AddRange(newElements);

            // Assert
            result.Should().BeSameAs(list);
        }

        [Fact]
        public void AddElements_adds_multiple_elements()
        {
            // Arrange
            ICollection<int> list = Enumerable.Range(0, 3).ToList();
            var expected = Enumerable.Range(0, 6).ToList();

            // Act
            list.AddElements(3, 4, 5);

            // Assert
            list.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void AddElements_requires_the_list()
        {
            // Arrange
            ICollection<int> list = null;
            var newElements = Enumerable.Range(3, 3).ToArray();
            Action action = () => list.AddElements(newElements);

            // Act
            // Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void AddElements_requires_the_collection()
        {
            // Arrange
            ICollection<int> list = Enumerable.Range(3, 3).ToList();
            int[] newElements = null;
            Action action = () => list.AddElements(newElements);

            // Act
            // Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void AddElements_returns_the_collection()
        {
            // Arrange
            ICollection<int> list = Enumerable.Range(0, 3).ToList();

            // Act
            var result = list.AddElements(1, 2);

            // Assert
            result.Should().BeSameAs(list);
        }

        [Fact]
        public void FindAndRemove_finds_the_element_by_match_function()
        {
            // Arrange
            var elements = new List<int> { 1, 2, 3 };

            // Act
            var result = elements.FindAndRemove(e => e == 2);

            // Assert
            result.Should().Be(2);
        }

        [Fact]
        public void FindAndRemove_returns_default_if_not_found()
        {
            // Arrange
            var elements = new List<int> { 1, 2, 3 };

            // Act
            var result = elements.FindAndRemove(e => e == 4);

            // Assert
            result.Should().Be(0);
        }

        [Fact]
        public void FindAndRemove_removes_the_element_from_the_list()
        {
            // Arrange
            var elements = new List<int> { 1, 2, 3 };

            // Act
            elements.FindAndRemove(e => e == 2);

            // Assert
            elements.Should().NotContain(2);
        }

        [Fact]
        public void FindAndRemove_removes_the_element_from_the_collection()
        {
            // Arrange
            var elements = new Dictionary<int, string> {
                { 1, "a"},
                { 2, "b"},
                { 3, "c"}
            };

            // Act
            elements.FindAndRemove(e => e.Key == 2);

            // Assert
            elements.Should().NotContainKey(2);
        }

        [Fact]
        public void FindAndRemove_requires_the_collection()
        {
            // Arrange
            ICollection<int> elements = null;

            // Act
            Func<int> func = () => elements.FindAndRemove(e => e == 2);

            // Assert
            func.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void FindAndRemove_requires_the_match()
        {
            // Arrange
            var elements = new List<int> { 1 };

            // Act
            Func<int> func = () => elements.FindAndRemove(null);

            // Assert
            func.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void ToChunks_requires_an_enumeration()
        {
            IEnumerable<int> enumeration = null; ;

            Calling(() => enumeration.ToChunks(3).ToList())
                .Should().Throw<ArgumentNullException>()
                .WithMessage("Value cannot be null. (Parameter 'enumerable')");
        }

        [Fact]
        public void ToChunks_requires_an_chunkSize_bigger_than_0()
        {
            var enumeration = Enumerable.Range(1, 10);

            Calling(() => enumeration.ToChunks(0).ToList())
                .Should().Throw<ArgumentException>()
                .WithMessage("ChunkSize must be at least 1. (Parameter 'chunkSize')");
        }

        [Fact]
        public void ToChunks_splits_the_list_into_partitions()
        {
            var enumeration = Enumerable.Range(1, 9);

            var chunks = enumeration.ToChunks(3).ToList();

            chunks.Should().HaveCount(3);
            chunks.Should().OnlyContain(e => e.Count == 3);

            chunks.SelectMany(e => e).Should().BeEquivalentTo(enumeration);
        }

        [Fact]
        public void ToChunks_splits_the_list_into_partitions_with_remainder()
        {
            var enumeration = Enumerable.Range(1, 10);

            var chunks = enumeration.ToChunks(3).ToList();

            chunks.Should().HaveCount(4);
            chunks.Take(3).Should().OnlyContain(e => e.Count == 3);
            chunks.Last().Should().HaveCount(1);

            chunks.SelectMany(e => e).Should().BeEquivalentTo(enumeration);
        }

        public Func<TR> Calling<TR>(Func<TR> func)
        {
            return func;
        }
    }
}
