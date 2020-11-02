using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;
using System;
using System.Collections.Generic;

namespace CommonLibraries.Core.Text.TestHelpers
{
    public static class ComparerAssertionExtensions
    {
        public static ComparerAssertions<T> Should<T>(this IComparer<T> instance)
        {
            return new ComparerAssertions<T>(instance);
        }
        public class ComparerAssertions<T>
            : ReferenceTypeAssertions<IComparer<T>, ComparerAssertions<T>>
        {
            public ComparerAssertions(
                IComparer<T> comparer)
            {
                Subject = comparer;
            }

            protected override string Identifier => Subject?.ToString() ?? "comparer";

            public AndConstraint<ComparerAssertions<T>> ResultWithSignFor(
                T left,
                T right,
                int expected,
                string because = "",
                params object[] becauseArgs)
            {
                var actual = Math.Sign(Subject.Compare(left, right));
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .ForCondition(actual == Math.Sign(expected))
                    .FailWith($"Expected {{context:comparer}} with (\"{left}\", \"{right}\") to return {expected}{{reason}}, but found {actual}.");
                return new AndConstraint<ComparerAssertions<T>>(this);
            }

            public AndConstraint<ComparerAssertions<T>> ResultSameAsFor(
                IComparer<T> comparer,
                T left,
                T right,
                string because = "",
                params object[] becauseArgs)
            {
                var actual = Math.Sign(Subject.Compare(left, right));
                var expected = Math.Sign(comparer.Compare(left, right));
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .ForCondition(actual == expected)
                    .FailWith($"Expected {{context:comparer}} with {comparer}(\"{left}\", \"{right}\") to return {expected}{{reason}}, but found {actual}.");
                return new AndConstraint<ComparerAssertions<T>>(this);
            }
        }
    }
}
