using System.Collections.Generic;
using System.Linq;

namespace CommonLibraries.Core.Text
{
    public partial class StringComparerExperiments
    {
        class StringArrayComparer<T> : IEqualityComparer<IEnumerable<T>>
        {
            public bool Equals(IEnumerable<T> x, IEnumerable<T> y)
            {
                if (ReferenceEquals(x, y))
                    return true;
                if (x == null || y == null)
                    return false;
                return x.Zip(y, (left, right) => Equals(left, right)).All(e => e);
            }

            public int GetHashCode(IEnumerable<T> obj)
            {
                return obj.Aggregate(0, (a, b) => a.GetHashCode() * 314159 + b.GetHashCode());
            }
        }
    }
}
