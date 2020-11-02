using System;

namespace CommonLibraries.Core.Text
{
    internal class ArrayComparer
    {
        public static bool ByteEquals(ushort[] left, ushort[] right)
        {
            if (ReferenceEquals(left, right))
                return true;
            if (left == null)
                return false;
            if (right == null)
                return false;
            if (left.Length != right.Length)
                return false;
            int l = Buffer.ByteLength(left);
            unsafe
            {
                fixed (void* p1 = left, p2 = right)
                {
                    byte* x1 = (byte*)p1, x2 = (byte*)p2;
                    for (int i = 0; i < l / 8; i++, x1 += 8, x2 += 8)
                        if (*(long*)x1 != *(long*)x2) return false;
                    if ((l & 4) != 0) { if (*(int*)x1 != *(int*)x2) return false; x1 += 4; x2 += 4; }
                    if ((l & 2) != 0) { if (*(short*)x1 != *(short*)x2) return false; x1 += 2; x2 += 2; }
                    if ((l & 1) != 0) if (*x1 != *x2) return false;
                    return true;
                }
            }
        }
    }
}
