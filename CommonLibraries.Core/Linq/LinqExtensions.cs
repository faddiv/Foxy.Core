using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace CommonLibraries.Core.Linq
{
    public static class LinqExtensions
    {
        /// <summary>
        /// It returns all element from the outer enumerable and default (null) 
        /// from inner enumerable if no matching key found.
        /// The default equality comparer is used to compare keys.
        /// </summary>
        /// <typeparam name="TOuter">The type of the elements of the first sequence.</typeparam>
        /// <typeparam name="TInner">The type of the elements of the second sequence.</typeparam>
        /// <typeparam name="TKey">The type of the keys returned by the key selector functions.</typeparam>
        /// <typeparam name="TResult">The type of the result elements.</typeparam>
        /// <param name="outer">The first sequence to left join.</param>
        /// <param name="inner">The sequence to left join to the first sequence.</param>
        /// <param name="outerKeySelector">A function to extract the left join key from each element of the first sequence.</param>
        /// <param name="innerKeySelector">A function to extract the left join key from each element of the second sequence.</param>
        /// <param name="resultSelector">A function to create a result element from two matching elements. The second element can be null or default.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> that has elements of type TResult 
        /// that are obtained by performing a left join on two sequences.</returns>
        /// <exception cref="ArgumentNullException">outer or inner or outerKeySelector or innerKeySelector or resultSelector is null.</exception>
        public static IEnumerable<TResult> LeftJoin<TOuter, TInner, TKey, TResult>(
            this IEnumerable<TOuter> outer,
            IEnumerable<TInner> inner,
            Func<TOuter, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<TOuter, TInner, TResult> resultSelector)
        {
            return outer.LeftJoin(inner, outerKeySelector, innerKeySelector, resultSelector, null);
        }


        /// <summary>
        /// It returns all element from the outer enumerable and default (null) 
        /// from inner enumerable if no matching key found.
        /// </summary>
        /// <typeparam name="TOuter">The type of the elements of the first sequence.</typeparam>
        /// <typeparam name="TInner">The type of the elements of the second sequence.</typeparam>
        /// <typeparam name="TKey">The type of the keys returned by the key selector functions.</typeparam>
        /// <typeparam name="TResult">The type of the result elements.</typeparam>
        /// <param name="outer">The first sequence to left join.</param>
        /// <param name="inner">The sequence to left join to the first sequence.</param>
        /// <param name="outerKeySelector">A function to extract the left join key from each element of the first sequence.</param>
        /// <param name="innerKeySelector">A function to extract the left join key from each element of the second sequence.</param>
        /// <param name="resultSelector">A function to create a result element from two matching elements. The second element can be null or default.</param>
        /// <param name="comparer">An <see cref="IEqualityComparer{T}"/> to hash and compare keys.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> that has elements of type TResult 
        /// that are obtained by performing a left join on two sequences.</returns>
        /// <exception cref="ArgumentNullException">outer or inner or outerKeySelector or innerKeySelector or resultSelector is null.</exception>
        public static IEnumerable<TResult> LeftJoin<TOuter, TInner, TKey, TResult>(
            this IEnumerable<TOuter> outer,
            IEnumerable<TInner> inner,
            Func<TOuter, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<TOuter, TInner, TResult> resultSelector,
            IEqualityComparer<TKey> comparer)
        {
            if (outer is null) throw new ArgumentNullException(nameof(outer));
            if (inner is null) throw new ArgumentNullException(nameof(inner));
            if (outerKeySelector is null) throw new ArgumentNullException(nameof(outerKeySelector));
            if (innerKeySelector is null) throw new ArgumentNullException(nameof(innerKeySelector));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

            return outer.GroupJoin(inner, outerKeySelector, innerKeySelector
                , (i, o) => new { i, o },
                comparer)
                .SelectMany(e => e.o.DefaultIfEmpty(), (t, e) => resultSelector(t.i, e));
        }

#if NETSTANDARD2_0 || NET45
        /// <summary>
        /// It returns all element from the outer enumerable and default (null) 
        /// from inner enumerable if no matching key found.
        /// The default equality comparer is used to compare keys.
        /// </summary>
        /// <typeparam name="TOuter">The type of the elements of the first sequence.</typeparam>
        /// <typeparam name="TInner">The type of the elements of the second sequence.</typeparam>
        /// <typeparam name="TKey">The type of the keys returned by the key selector functions.</typeparam>
        /// <typeparam name="TResult">The type of the result elements.</typeparam>
        /// <param name="outer">The first sequence to left join.</param>
        /// <param name="inner">The sequence to left join to the first sequence.</param>
        /// <param name="outerKeySelector">A function to extract the left join key from each element of the first sequence.</param>
        /// <param name="innerKeySelector">A function to extract the left join key from each element of the second sequence.</param>
        /// <param name="resultSelector">A function to create a result element from two matching elements. The second element can be null or default.</param>
        /// <returns>An <see cref="IQueryable{T}"/> that contains elements of type TResult
        /// obtained by performing a left join on two sequences.</returns>
        public static IQueryable<TResult> LeftJoin<TOuter, TInner, TKey, TResult>(
            this IQueryable<TOuter> outer,
            IQueryable<TInner> inner,
            Expression<Func<TOuter, TKey>> outerKeySelector,
            Expression<Func<TInner, TKey>> innerKeySelector,
            Expression<Func<TOuter, TInner, TResult>> resultSelector)
        {
            return outer.LeftJoin(inner, outerKeySelector, innerKeySelector, resultSelector, null);
        }

        /// <summary>
        /// It returns all element from the outer enumerable and default (null) 
        /// from inner enumerable if no matching key found.
        /// </summary>
        /// <typeparam name="TOuter">The type of the elements of the first sequence.</typeparam>
        /// <typeparam name="TInner">The type of the elements of the second sequence.</typeparam>
        /// <typeparam name="TKey">The type of the keys returned by the key selector functions.</typeparam>
        /// <typeparam name="TResult">The type of the result elements.</typeparam>
        /// <param name="outer">The first sequence to left join.</param>
        /// <param name="inner">The sequence to left join to the first sequence.</param>
        /// <param name="outerKeySelector">A function to extract the left join key from each element of the first sequence.</param>
        /// <param name="innerKeySelector">A function to extract the left join key from each element of the second sequence.</param>
        /// <param name="resultSelector">A function to create a result element from two matching elements. The second element can be null or default.</param>
        /// <param name="comparer">An <see cref="IEqualityComparer{T}"/> to hash and compare keys.</param>
        /// <returns>An <see cref="IQueryable{T}"/> that contains elements of type TResult
        /// obtained by performing a left join on two sequences.</returns>
        public static IQueryable<TResult> LeftJoin<TOuter, TInner, TKey, TResult>(
            this IQueryable<TOuter> outer,
            IQueryable<TInner> inner,
            Expression<Func<TOuter, TKey>> outerKeySelector,
            Expression<Func<TInner, TKey>> innerKeySelector,
            Expression<Func<TOuter, TInner, TResult>> resultSelector,
            IEqualityComparer<TKey> comparer)
        {
            if (outer is null) throw new ArgumentNullException(nameof(outer));
            if (inner is null) throw new ArgumentNullException(nameof(inner));
            if (outerKeySelector is null) throw new ArgumentNullException(nameof(outerKeySelector));
            if (innerKeySelector is null) throw new ArgumentNullException(nameof(innerKeySelector));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

            var grp = comparer != null
                ? outer.GroupJoin(inner,
                outerKeySelector, innerKeySelector,
                (o, i) => new
                {
                    Outer = o,
                    Inner = i,
                }, comparer)
                : outer.GroupJoin(inner,
                outerKeySelector, innerKeySelector,
                (o, i) => new
                {
                    Outer = o,
                    Inner = i,
                });
            var selectManyCall = CreateSelectManyCall(grp, resultSelector);
            var query = grp.Provider.CreateQuery<TResult>(selectManyCall);
            return query;
        }

        private static MethodCallExpression CreateSelectManyCall(
            IQueryable grp,
            LambdaExpression resultSelector)
        {
            var innerType = resultSelector.Parameters[1].Type;
            var resultType = resultSelector.ReturnType;
            var paramType = grp.ElementType;
            var param = Expression.Parameter(paramType);
            var collectionSelector = CreateDefaultIfEmptySelector(
                param, innerType);
            var newResult = CreateResultSelector(resultSelector, param);
            return Expression.Call(typeof(Queryable),
                nameof(Queryable.SelectMany),
                new Type[] { grp.ElementType, innerType, resultType },
                grp.Expression,
                collectionSelector,
                newResult);
        }

        private static LambdaExpression CreateDefaultIfEmptySelector(
            ParameterExpression param,
            Type innerType)
        {
            var getter = Expression.Property(param, "Inner");
            var defaultIfEmpty = Expression.Call(
                typeof(Enumerable),
                nameof(Enumerable.DefaultIfEmpty),
                new Type[] { innerType },
                getter);
            var collectionSelector = Expression.Lambda(
                    defaultIfEmpty,
                    param);
            return collectionSelector;
        }

        private static LambdaExpression CreateResultSelector(
            LambdaExpression resultSelector,
            ParameterExpression param)
        {
            var getter = Expression.Property(param, "Outer");
            var visitor = new ReplaceParameterExpressionVisitor(resultSelector.Parameters[0], getter);
            var newResultSelector = visitor.Visit(resultSelector.Body);
            var newResult = Expression.Lambda(newResultSelector, param, resultSelector.Parameters[1]);
            return newResult;
        }
#endif
    }

#if NETSTANDARD2_0 || NET45
    class ReplaceParameterExpressionVisitor : ExpressionVisitor
    {
        public ReplaceParameterExpressionVisitor(ParameterExpression oldExpression, Expression newExpression)
        {
            OldExpression = oldExpression ?? throw new ArgumentNullException(nameof(oldExpression));
            NewExpression = newExpression ?? throw new ArgumentNullException(nameof(newExpression));
        }

        public ParameterExpression OldExpression { get; }
        public Expression NewExpression { get; }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            if (ReferenceEquals(node, OldExpression))
            {
                return NewExpression;
            }
            return base.VisitParameter(node);
        }
    }
#endif
}
