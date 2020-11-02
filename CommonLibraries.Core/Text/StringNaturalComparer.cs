using System;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace CommonLibraries.Core.Text
{
    /// <summary>
    /// Enhances string comparer with natural sorting functionality,
    /// which allows it to sort numbers inside the strings as numbers, not as letters.
    /// (e.g. "1", "2", "10" instead of "1", "10", "2")
    /// </summary>
    public class StringNaturalComparer : StringComparer
    {
        /// <summary>
        /// Gets a <see cref="StringNaturalComparer"/> object that performs 
        /// a case-sensitive string comparison using the word comparison rules of 
        /// the current culture.
        /// </summary>
        public new static StringNaturalComparer CurrentCulture
                    => new StringNaturalComparer(CompareOptions.None, CultureInfo.CurrentCulture);

        /// <summary>
        /// Gets a <see cref="StringNaturalComparer"/> object that performs 
        /// a case-insensitive string comparison using the word comparison rules of 
        /// the current culture.
        /// </summary>
        public new static StringNaturalComparer CurrentCultureIgnoreCase
                    => new StringNaturalComparer(CompareOptions.IgnoreCase, CultureInfo.CurrentCulture);

#if !NETSTANDARD1_3
        /// <summary>
        /// Gets a <see cref="StringNaturalComparer"/> object that performs 
        /// a case-sensitive string natural comparison using the word comparison rules of 
        /// the invariant culture.
        /// </summary>
        public new static StringNaturalComparer InvariantCulture
                    => new StringNaturalComparer(CompareOptions.None, CultureInfo.InvariantCulture);

        /// <summary>
        /// Gets a <see cref="StringNaturalComparer"/> object that performs 
        /// a case-insensitive string natural comparison using the word comparison rules of 
        /// the invariant culture.
        /// </summary>
        public new static StringNaturalComparer InvariantCultureIgnoreCase
                    => new StringNaturalComparer(CompareOptions.IgnoreCase, CultureInfo.InvariantCulture);
#endif
        /// <summary>
        /// Gets a <see cref="StringNaturalComparer"/> object that performs 
        /// a case-sensitive ordinal string natural comparison.
        /// </summary>
        public new static StringNaturalComparer Ordinal
                    => new StringNaturalComparer(CompareOptions.None);

        /// <summary>
        /// Gets a <see cref="StringNaturalComparer"/> object that performs 
        /// a case-insensitive ordinal string natural comparison.
        /// </summary>
        public new static StringNaturalComparer OrdinalIgnoreCase
                    => new StringNaturalComparer(CompareOptions.IgnoreCase);

        /// <summary>
        ///     Creates a <see cref="StringNaturalComparer"/> object that compares 
        ///     strings according to the rules of a specified culture.
        /// </summary>
        /// <param name="culture">
        ///     A culture whose linguistic rules are used to perform a string 
        ///     natural comparison. If null then ordinal comparison will be used.
        /// </param>
        /// <param name="ignoreCase">
        ///     True to specify that comparison operations be case-insensitive; 
        ///     False to specify that comparison operations be case-sensitive.
        /// </param>
        /// <returns>
        ///     A new <see cref="StringNaturalComparer"/> object that performs 
        ///     string natural comparisons according to the comparison rules used by 
        ///     the culture parameter and the case rule specified by the ignoreCase 
        ///     parameter.
        /// </returns>
        public new static StringNaturalComparer Create(CultureInfo culture, bool ignoreCase)
        {
            return new StringNaturalComparer(
                ignoreCase ? CompareOptions.IgnoreCase : CompareOptions.None,
                culture);
        }

        internal const int intSize = sizeof(int) / sizeof(char);
        internal const int longSize = sizeof(long) / sizeof(char);

        private readonly CultureInfo _culture;
        private readonly CompareOptions _options;
        private readonly bool _ignoreCase;
        private readonly ushort[] _mappings;


        private StringNaturalComparer(CompareOptions options, CultureInfo culture = null)
        {
            _options = options;
            _ignoreCase = _options.HasFlag(CompareOptions.IgnoreCase);
            _culture = culture;
            if (culture != null)
                _mappings = MappingCache.GetMapping(culture, _ignoreCase);
            else
            {
#if NET45
                _mappings = new ushort[0];
#else
                _mappings = Array.Empty<ushort>();
#endif
            }
        }

        public override int Compare(string str1, string str2)
        {
            if (ReferenceEquals(str1, str2))
            {
                return 0;
            }
            if (str1 == null || str1.Length == 0)
            {
                return 1;
            }
            if (str2 == null || str2.Length == 0)
            {
                return -1;
            }

            var strLength1 = str1.Length;
            var strLength2 = str2.Length;
            var length = Math.Min(strLength1, strLength2);

            unsafe
            {
                fixed (char* fp1 = str1)
                fixed (char* fp2 = str2)
                {
                    char* pointer1 = fp1;
                    char* pointer2 = fp2;
                    while (length > 0)
                    {
                        bool hasDiff = MoveNextDifferenceOrdinal(ref pointer1, ref pointer2, ref length);
                        if (!hasDiff)
                        {
                            // Not found difference till the end of one string. Size decide the order.
                            if (strLength1 == strLength2)
                                break;
                            if (pointer1 - fp1 == strLength1
                                && pointer2 - fp2 == strLength2)
                                break;
                        }
                        var diffWithCurrentMode = CompareChar(*pointer1, *pointer2);
                        if (diffWithCurrentMode == 0)
                        {
                            pointer1++;
                            pointer2++;
                            length--;
                            continue;
                        }
                        var noBackstep = pointer1 == fp1 || pointer2 == fp2;
                        bool isDigit1;
                        bool isDigit2;
                        if (noBackstep)
                        {
                            isDigit1 = IsDigit(pointer1);
                            if (!isDigit1)
                                return diffWithCurrentMode;

                            isDigit2 = IsDigit(pointer2);
                            if (!isDigit2)
                                return diffWithCurrentMode;
                        }
                        else
                        {
                            isDigit1 = IsDigit(pointer1);
                            isDigit2 = IsDigit(pointer2);
                            if (!isDigit1 && !isDigit2)
                            {
                                return diffWithCurrentMode;
                            }
                        }

                        // Every other options exhausted. we need to compare digits.
                        // first we go back to the begining of the digits.
                        if (IsDigit(pointer1 - 1))
                        {
                            pointer1--;
                            pointer2--;
                            length++;
                            while (pointer1 - 1 >= fp1
                                && pointer2 - 1 >= fp2
                                && IsDigit(pointer1 - 1))
                            {
                                pointer1--;
                                pointer2--;
                                length++;

                            }
                            diffWithCurrentMode = CompareChar(*pointer1, *pointer2);
                            isDigit1 = IsDigit(pointer1);
                            isDigit2 = IsDigit(pointer2);
                        }
                        // check again. Maybe one wasn't digit at all.
                        if (!isDigit1 || !isDigit2)
                        {
                            if (diffWithCurrentMode != 0 || length == 1)
                            {
                                return diffWithCurrentMode;
                            }
                            else
                            {
                                length--;
                                pointer1++;
                                pointer2++;
                                continue;
                            }

                        }
                        long number1 = ToNumber(strLength1, fp1, ref pointer1);
                        var number2 = ToNumber(strLength2, fp2, ref pointer2);
                        if (number1 != number2)
                        {
                            return number1 > number2 ? 1 : -1;
                        }
                        length = Math.Min(strLength1 - (int)(pointer1 - fp1), strLength2 - (int)(pointer2 - fp2));
                    }

                    if (strLength1 == strLength2)
                        return 0;
                    else
                        return strLength1 < strLength2 ? 1 : -1;
                }
            }
        }

#if !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        private bool IsEqualIgnoreCase(char v1, char v2)
        {
            if (_culture != null)
            {
                // Fix Windows bug related to these characters.
                // The ToUpper and Compare works differently in case of these characters.
                if (ReferenceEquals(_culture, CultureInfo.InvariantCulture)
                    && (v1 == 'ʀ' || v2 == 'ʀ'))
                {
                    if (v1 == 'Ʀ' || v2 == 'Ʀ')
                    {
                        return v1 == v2;
                    }
                }
                return _culture.TextInfo.ToUpper(v1) == _culture.TextInfo.ToUpper(v2);
            }
            else
            {
                return CultureInfo.InvariantCulture.TextInfo.ToUpper(v1) == CultureInfo.InvariantCulture.TextInfo.ToUpper(v2);
            }
        }

#if !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        private int CompareChar(char ch1, char ch2)
        {
            if (_culture != null)
            {

                ushort value1 = _mappings[ch1];
                ushort value2 = _mappings[ch2];
                return value1 == value2
                        ? 0
                        : value1 < value2 ? -1 : 1;
                //return string.Compare(char.ToString(ch1), char.ToString(ch2), _ignoreCase, _culture);
            }
            else
            {
                if (_ignoreCase)
                {
                    return char.ToUpperInvariant(ch1) - char.ToUpperInvariant(ch2);
                }
                else
                {
                    return ch1 - ch2;
                }
            }
        }

        private char ToUpperInvariant(char ch1)
        {
            if (ch1 == 'ᵣ') return 'Ʀ';
            if (ch1 == 'ʀ') return 'ʀ';
            return char.ToUpperInvariant(ch1);
        }

#if !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        internal unsafe static bool IsDigit(char* pointer)
        {
            return '0' <= *pointer && *pointer <= '9';
        }

#if !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        private static unsafe long ToNumber(int strLength, char* fp, ref char* pointer)
        {
            var number = 0L;
            var length = strLength - (pointer - fp);
            while (length > 0 && IsDigit(pointer))
            {
                var currentNumber = *pointer - '0';
                if (number > 0 || currentNumber > 0)
                {
                    number = number * 10 + currentNumber;
                }
                pointer++;
                length--;
            }

            return number;
        }

#if !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        private static unsafe bool MoveNextDifferenceOrdinal(ref char* a, ref char* b, ref int length)
        {
            if (*a != *b) return true;
            if (length == 1) return false;
            if (*(a + 1) != *(b + 1)) goto DiffEnd;
            length -= 2; a += 2; b += 2;

            while (length >= 12)
            {
                if (*(long*)a != *(long*)b) goto DiffOffset0;
                if (*(long*)(a + 4) != *(long*)(b + 4)) goto DiffOffset4;
                if (*(long*)(a + 8) != *(long*)(b + 8)) goto DiffOffset8;
                length -= 12; a += 12; b += 12;
            }

            while (length > 1)
            {
                if (*(int*)a != *(int*)b) goto DiffNextInt;
                length -= 2;
                a += 2;
                b += 2;
            }
            if (length > 0)
            {
                if (*a != *b) return true;
                length--;
                a++;
                b++;
            }
            return false;

        DiffOffset8: a += 4; b += 4;
        DiffOffset4: a += 4; b += 4;

        DiffOffset0:
            if (*(int*)a == *(int*)b)
            {
                a += 2; b += 2;
                length -= 2;
            }
            return false;

        DiffNextInt:
            if (*a != *b) return true;
            DiffEnd:
            length--;
            a++;
            b++;
            return true;
        }

        public override bool Equals(string x, string y)
        {
            if (_culture != null)
            {
                return _culture.CompareInfo.Compare(x, y, _options) == 0;
            }
            return string.Equals(x, y, _ignoreCase
                ? StringComparison.OrdinalIgnoreCase
                : StringComparison.Ordinal);
        }

        public override int GetHashCode(string obj)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            if (_culture != null)
            {
#if NET45
                return obj.GetHashCode();
#else
                return _culture.CompareInfo.GetHashCode(obj, _options);
#endif
            }
            if (_ignoreCase)
            {
                return StringComparer.OrdinalIgnoreCase.GetHashCode(obj);
            }
            return obj.GetHashCode();
        }
    }
}