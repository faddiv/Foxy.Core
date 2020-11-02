using System;
using System.Linq;

namespace CommonLibraries.Core.Text
{
    internal class VectorKey : IEquatable<VectorKey>
    {
        private readonly ushort[] _vector;
        private readonly int _hashCode;

        public VectorKey(ushort[] table)
        {
            _vector = table ?? throw new ArgumentNullException(nameof(table));
            _hashCode = _vector
                .Aggregate(-1440717171, (sum, element) => sum * -1521134295 + element.GetHashCode());
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as VectorKey);
        }

        public bool Equals(VectorKey other)
        {
            if (other == null) return false;
            if (_hashCode != other._hashCode)
                return false;
            return ArrayComparer.ByteEquals(_vector, other._vector);
        }

        public override int GetHashCode()
        {
            return _hashCode;
        }
    }
}