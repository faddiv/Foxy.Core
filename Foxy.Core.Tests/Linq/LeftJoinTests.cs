using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using NorthwindEFCoreSqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace Foxy.Core.Linq
{
    public class LeftJoinTests : IDisposable
    {

        private NorthwindContext db;
        public LeftJoinTests()
        {
            db = CreateDb();
        }

        public void Dispose()
        {
            db.Dispose();
        }

        [Fact]
        public void Enumerable_LeftJoin_works_as_left_join()
        {
            var list1 = GetOrders();
            var list2 = GetOrderDetails();
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
            var list1 = GetOrders();
            var list2 = GetOrderDetails();

            Assert.Throws<ArgumentNullException>("outer", () =>
            {
                Order[] outer = null;
                return outer
                    .LeftJoin(list2, l => l.OrderId, l => l.OrderId, (o, i) => new { o, i })
                    .ToList();
            });

            Assert.Throws<ArgumentNullException>("inner", () =>
            {
                return list1
                    .LeftJoin((OrderDetail[])null, l => l.OrderId, l => l.OrderId, (o, i) => new { o, i })
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
                Func<Order, OrderDetail, object> resultSelector = null;
                return list1
                    .LeftJoin(list2, l => l.OrderId, l => l.OrderId, resultSelector)
                    .ToList();
            });
        }

        [Fact]
        public void Enumerable_LeftJoin_uses_equality_comparer()
        {
            var list1 = GetOrders().Take(2).ToList();
            var list2 = GetOrderDetails().FindAll(e => list1.Any(o => o.OrderId == e.OrderId));
            var comparer = CreateComparer();

            var result = list1
                .LeftJoin(list2, l => l.OrderId, l => l.OrderId, (o, i) => new { o, i }, comparer.Object)
                .ToList();

            comparer.Verify(e => e.Equals(It.IsAny<long>(), It.IsAny<long>()), Times.AtLeastOnce);
        }

        [Fact]
        public void Queriable_LeftJoin_works_on_db_as_left_join()
        {
            using var db = CreateDb();
            var orderDetailsSubset = db.OrderDetails.Where(e => e.OrderId % 2 == 0);
            var result = db.Orders
                .LeftJoin(orderDetailsSubset, l => l.OrderId, l => l.OrderId, (o, i) => new { o, i })
                .ToList();

            result.Should().Contain(e => e.o != null && e.i != null && e.o.OrderId == e.i.OrderId);
            result.Should().Contain(e => e.o != null && e.i == null);
        }

        [Fact]
        public void Queriable_LeftJoin_uses_comparer()
        {
            var list1 = GetOrders().Take(2).ToList();
            var list2 = GetOrderDetails().FindAll(e => list1.Any(o => o.OrderId == e.OrderId));
            var comparer = CreateComparer();

            var result = list1
                .LeftJoin(list2, l => l.OrderId, l => l.OrderId, (o, i) => new { o, i }, comparer.Object)
                .ToList();

            comparer.Verify(e => e.Equals(It.IsAny<long>(), It.IsAny<long>()), Times.AtLeastOnce);
        }

        private static Mock<IEqualityComparer<long>> CreateComparer()
        {
            var comparer = new Mock<IEqualityComparer<long>>();
            comparer.Setup(e => e.Equals(It.IsAny<long>(), It.IsAny<long>()))
                .Returns((long x, long y) => EqualityComparer<long>.Default.Equals(x, y));
            comparer.Setup(e => e.GetHashCode(It.IsAny<long>()))
                .Returns((long x) => EqualityComparer<long>.Default.GetHashCode(x));
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
                DbSet<Order> outer = null;
                return outer
                    .LeftJoin(list2, l => l.OrderId, l => l.OrderId, (o, i) => new { o, i })
                    .ToList();
            });

            Assert.Throws<ArgumentNullException>("inner", () =>
            {
                return list1
                    .LeftJoin((OrderDetail[])null, l => l.OrderId, l => l.OrderId, (o, i) => new { o, i })
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
                Expression<Func<Order, OrderDetail, object>> resultSelector = null;
                return list1
                    .LeftJoin(list2, l => l.OrderId, l => l.OrderId, resultSelector)
                    .ToList();
            });
        }


        private static NorthwindContext CreateDb()
        {
            return new NorthwindContext();
        }

        private List<Order> GetOrders()
        {
            return db.Orders
                .ToList();
        }

        private List<OrderDetail> GetOrderDetails()
        {
            return db.OrderDetails
                .ToList();
        }

    }
}
