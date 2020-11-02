using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace CommonLibraries.Core.Text
{
    internal class MappingCache
    {
        private static readonly ConcurrentDictionary<CacheKey, ushort[]> mappings
            = new ConcurrentDictionary<CacheKey, ushort[]>();

        private static readonly ConcurrentDictionary<VectorKey, ushort[]> orderings
            = new ConcurrentDictionary<VectorKey, ushort[]>();

        public static ushort[] GetMapping(CultureInfo cultureInfo, bool ignoreCase)
        {
            return mappings.GetOrAdd(new CacheKey(cultureInfo, ignoreCase), CreateMappingTable);
        }

        private static ushort[] CreateMappingTable(CacheKey cacheKey)
        {
            return GetOrdering(CreateMappingTable(StringComparer.Create(
                cacheKey.CultureInfo, cacheKey.IgnoreCase)));
        }

        private static ushort[] GetOrdering(ushort[] createdMapping)
        {
            return orderings.GetOrAdd(new VectorKey(createdMapping), createdMapping);
        }

        internal static ushort[] CreateMappingTable(StringComparer comparer)
        {
            var sort = EnumerateAllCharacter()
                .OrderBy(c => c, comparer);
            int index = -1;
            string lastItem = null;
            var mapping = new ushort[1 + char.MaxValue - char.MinValue];
            foreach (var item in sort)
            {
                if (!comparer.Equals(lastItem, item))
                {
                    index++;
                    lastItem = item;
                }
                mapping[item[0]] = (ushort)index;
            }
            return mapping;
        }

        private static IEnumerable<string> EnumerateAllCharacter()
        {
            for (int ch1 = char.MinValue; ch1 <= char.MaxValue; ch1++)
            {
                // Add unicode value so there will be exact order in every case.
                yield return char.ToString((char)ch1);
            }
        }

    }
}