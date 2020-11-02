using CommonLibraries.Testing.EntityFrameworkCore.Tests;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using NorthwindDatabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace CommonLibraries.Core.Linq
{
    public class LeftJoinTests
    {
        static NorthWindDatabaseFactory factory = new NorthWindDatabaseFactory(new DatabaseScaffold());

        [Fact]
        public void Enumerable_LeftJoin_works_as_left_join()
        {
            var list1 = GetList();
            var list2 = list1.SelectMany(e => e.OrderDetails).ToList();
            list2.RemoveRange(list2.Count / 2, list2.Count / 2);

            var result = list1
                .LeftJoin(list2, l => l.OrderId, l => l.OrderId, (o, i) => new { o, i })
                .ToList();

            result.Should().Contain(e => e.o != null && e.i != null && e.o.OrderId == e.i.OrderId);
            result.Should().Contain(e => e.o != null && e.i == null);
        }

        [Fact]
        public void Enumerable_LeftJoin_requires_all_arguments()
        {
            var list1 = GetList();
            var list2 = list1.SelectMany(e => e.OrderDetails).ToList();

            Assert.Throws<ArgumentNullException>("outer", () =>
            {
                Orders[] outer = null;
                return outer
                    .LeftJoin(list2, l => l.OrderId, l => l.OrderId, (o, i) => new { o, i })
                    .ToList();
            });

            Assert.Throws<ArgumentNullException>("inner", () =>
            {
                return list1
                    .LeftJoin((OrderDetails[])null, l => l.OrderId, l => l.OrderId, (o, i) => new { o, i })
                    .ToList();
            });

            Assert.Throws<ArgumentNullException>("outerKeySelector", () =>
            {
                return list1
                    .LeftJoin(list2, null, l => l.OrderId, (o, i) => new { o, i })
                    .ToList();
            });

            Assert.Throws<ArgumentNullException>("innerKeySelector", () =>
            {
                return list1
                    .LeftJoin(list2, l => l.OrderId, null, (o, i) => new { o, i })
                    .ToList();
            });
            Assert.Throws<ArgumentNullException>("resultSelector", () =>
            {
                Func<Orders, OrderDetails, object> resultSelector = null;
                return list1
                    .LeftJoin(list2, l => l.OrderId, l => l.OrderId, resultSelector)
                    .ToList();
            });
        }

        [Fact]
        public void Enumerable_LeftJoin_uses_equality_comparer()
        {
            var list1 = GetList();
            var list2 = list1.SelectMany(e => e.OrderDetails).ToList();
            var comparer = CreateComparer();

            var result = list1
                .LeftJoin(list2, l => l.OrderId, l => l.OrderId, (o, i) => new { o, i }, comparer.Object)
                .ToList();

            comparer.Verify(e => e.Equals(It.IsAny<int>(), It.IsAny<int>()), Times.AtLeastOnce);
        }

        [Fact]
        public void Queriable_LeftJoin_works_on_db_as_left_join()
        {
            using (var db = CreateDb())
            {

                var orderDetails = db.Orders.Take(10).SelectMany(e => e.OrderDetails).ToList();
                db.RemoveRange(orderDetails);
                db.SaveChanges();

                var result = db.Orders
                    .LeftJoin(db.OrderDetails, l => l.OrderId, l => l.OrderId, (o, i) => new { o, i })
                    .ToList();

                result.Should().Contain(e => e.o != null && e.i != null && e.o.OrderId == e.i.OrderId);
                result.Should().Contain(e => e.o != null && e.i == null);
            }
        }

        [Fact]
        public void Queriable_LeftJoin_uses_comparer()
        {
            var list1 = GetList();
            var list2 = list1.SelectMany(e => e.OrderDetails).ToList();
            var comparer = CreateComparer();

            var result = list1
                .LeftJoin(list2, l => l.OrderId, l => l.OrderId, (o, i) => new { o, i }, comparer.Object)
                .ToList();

            comparer.Verify(e => e.Equals(It.IsAny<int>(), It.IsAny<int>()), Times.AtLeastOnce);
        }

        private static Mock<IEqualityComparer<int>> CreateComparer()
        {
            var comparer = new Mock<IEqualityComparer<int>>();
            comparer.Setup(e => e.Equals(It.IsAny<int>(), It.IsAny<int>()))
                .Returns((int x, int y) => EqualityComparer<int>.Default.Equals(x, y));
            comparer.Setup(e => e.GetHashCode(It.IsAny<int>()))
                .Returns((int x) => EqualityComparer<int>.Default.GetHashCode(x));
            return comparer;
        }

        [Fact]
        public void Queriable_LeftJoin_requires_all_arguments()
        {
            using var db = CreateDb();
            var list1 = db.Orders;
            var list2 = db.OrderDetails;
            Assert.Throws<ArgumentNullException>("outer", () =>
            {
                DbSet<Orders> outer = null;
                return outer
                    .LeftJoin(list2, l => l.OrderId, l => l.OrderId, (o, i) => new { o, i })
                    .ToList();
            });

            Assert.Throws<ArgumentNullException>("inner", () =>
            {
                return list1
                    .LeftJoin((OrderDetails[])null, l => l.OrderId, l => l.OrderId, (o, i) => new { o, i })
                    .ToList();
            });

            Assert.Throws<ArgumentNullException>("outerKeySelector", () =>
            {
                return list1
                    .LeftJoin(list2, null, l => l.OrderId, (o, i) => new { o, i })
                    .ToList();
            });

            Assert.Throws<ArgumentNullException>("innerKeySelector", () =>
            {
                return list1
                    .LeftJoin(list2, l => l.OrderId, null, (o, i) => new { o, i })
                    .ToList();
            });
            Assert.Throws<ArgumentNullException>("resultSelector", () =>
            {
                Expression<Func<Orders, OrderDetails, object>> resultSelector = null;
                return list1
                    .LeftJoin(list2, l => l.OrderId, l => l.OrderId, resultSelector)
                    .ToList();
            });
        }


        private static TestDbContext CreateDb()
        {
            return factory.CreateDbContext();
        }

        private static List<Orders> GetList()
        {
            var db = CreateDb();
            return db.Orders
                .Include(e => e.OrderDetails)
                .ToList();
        }

        private static List<OrderDetails> GetList2()
        {
            var db = CreateDb();
            return db.OrderDetails
                .Include(e => e.Order)
                .ToList();
        }
    }
}
