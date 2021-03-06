using FluentAssertions;
using Moq;
using NorthwindEFCoreSqlite;
using System;
using System.Collections.Generic;
using Xunit;

namespace Foxy.Core.Collections
{
    public class KeyComparerTests
    {

        [Fact]
        public void Ctor_keySelector_required()
        {

            Action f = () => new KeyComparer<Customer, string>(null);

            f.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Compare_calls_key_selector_on_both_model()
        {
            var keySelector = new Mock<Func<Customer, string>>();
            var comparer = new KeyComparer<Customer, string>(keySelector.Object);
            var customer1 = new Customer { CustomerId = "2" };
            var customer2 = new Customer { CustomerId = "1" };

            comparer.Compare(customer1, customer2);

            keySelector.Verify(e => e(customer1), Times.Once);
            keySelector.Verify(e => e(customer2), Times.Once);
        }

        [Theory]
        [InlineData("2", "3")]
        [InlineData("2", "1")]
        [InlineData("4", "4")]
        public void Compare_compares_the_keys(string leftKey, string rightKey)
        {
            var comparer = new KeyComparer<Customer, string>(model => model.CustomerId);
            var customer1 = new Customer { CustomerId = leftKey };
            var customer2 = new Customer { CustomerId = rightKey };

            var result = comparer.Compare(customer1, customer2);

            result.Should().Be(Comparer<string>.Default.Compare(leftKey, rightKey));
        }

        [Fact]
        public void SortByKey_sorts_the_list_by_key()
        {
            var customer1 = new Customer { CustomerId = "1" };
            var customer2 = new Customer { CustomerId = "2" };
            var customer3 = new Customer { CustomerId = "3" };
            var list = new List<Customer> { customer2, customer3, customer1 };

            list.SortByKey(e => e.CustomerId);

            list.Should().BeInAscendingOrder(e => e.CustomerId);
        }

        [Fact]
        public void SortByKey_sorts_the_array_by_key()
        {
            var customer1 = new Customer { CustomerId = "1" };
            var customer2 = new Customer { CustomerId = "2" };
            var customer3 = new Customer { CustomerId = "3" };
            var list = new [] { customer2, customer3, customer1 };

            list.SortByKey(e => e.CustomerId);

            list.Should().BeInAscendingOrder(e => e.CustomerId);
        }

    }
}
