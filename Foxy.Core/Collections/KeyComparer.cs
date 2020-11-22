using System;
using System.Collections;
using System.Collections.Generic;

namespace Foxy.Core.Collections
{
    /// <summary>
    /// Represents a key comparison operation that uses the keySelector to extract
    /// the key from the <typeparamref name="TValue"/> and uses it to compare the inputs.
    /// </summary>
    /// <typeparam name="TValue">Represents the type of value that compared by a key.</typeparam>
    /// <typeparam name="TKey">Represents the type of key that extracted from the value.</typeparam>
    public class KeyComparer<TValue, TKey> : IComparer<TValue>, IComparer
    {
        private readonly Func<TValue, TKey> _keySelector;
        private readonly IComparer<TKey> _keyComparer;

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyComparer{TValue, TKey}"/> class.
        /// </summary>
        /// <param name="keySelector">A function that extracts the key from the value.</param>
        /// <exception cref="ArgumentNullException">The keySelector is null.</exception>
        public KeyComparer(Func<TValue, TKey> keySelector)
        {
            _keySelector = keySelector ?? throw new ArgumentNullException(nameof(keySelector));
            _keyComparer = Comparer<TKey>.Default;
        }

        /// <summary>
        /// Compares two objects by key and returns a value indicating whether one is less than, equal to, or greater than the other.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>
        /// A signed integer that indicates the relative values of x and y.
        /// <para>If Less than zero then x is less than y.</para>
        /// <para>If Zero	then x equals y.</para>
        /// <para>If Greater than zero then x is greater than y.</para>
        /// </returns>
        public int Compare(TValue x, TValue y)
        {
            var keyLeft = _keySelector(x);
            var keyRight = _keySelector(y);
            return _keyComparer.Compare(keyLeft, keyRight);
        }

        int IComparer.Compare(object x, object y)
        {
            return Compare((TValue)x, (TValue)y);
        }
    }
}
