using System;
using System.Collections;
using System.Collections.Generic;

namespace CommonLibraries.Core.Collections
{
    /// <summary>
    /// Represents a key comparison operation that uses the keySelector to extract
    /// the key from the <see cref="TValue"/>.
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
        public int Compare(TValue left, TValue right)
        {
            var keyLeft = _keySelector(left);
            var keyRight = _keySelector(right);
            return _keyComparer.Compare(keyLeft, keyRight);
        }

        int IComparer.Compare(object left, object right)
        {
            return Compare((TValue)left, (TValue)right);
        }
    }
}
