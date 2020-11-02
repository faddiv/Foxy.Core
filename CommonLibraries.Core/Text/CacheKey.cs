using System;
using System.Globalization;

namespace CommonLibraries.Core.Text
{
    internal class CacheKey : IEquatable<CacheKey>
    {
        public CacheKey(CultureInfo cultureInfo, bool ignoreCase)
        {
            CultureInfo = cultureInfo ?? throw new ArgumentNullException(nameof(cultureInfo));
            IgnoreCase = ignoreCase;
        }

        public CultureInfo CultureInfo { get; }

        public int CultureCode => CultureInfo.LCID;
        public bool IgnoreCase { get; }

        public bool Equals(CacheKey key)
        {
            return key != null &&
                CultureCode == key.CultureCode &&
                IgnoreCase == key.IgnoreCase;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as CacheKey);

        }

        public override int GetHashCode()
        {
            var hashCode = -635829385;
            hashCode = hashCode * -1521134295 + CultureCode.GetHashCode();
            hashCode = hashCode * -1521134295 + IgnoreCase.GetHashCode();
            return hashCode;
        }
    }
}