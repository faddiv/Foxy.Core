using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace CommonLibraries.Core.Text
{
    public class PInvokeComparer : IComparer<string>
    {
        /// <summary>
        /// String comparer used for comparing strings.
        /// </summary>
        public PInvokeComparer()
        {
        }

        public int Compare(string str1, string str2)
        {
            if (str1 is null)
            {
                if (str2 is null)
                    return 0;
                else
                    return 1;
            }
            else if (str2 is null)
            {
                return -1;
            }
            return Shlwapi.StrCmpLogicalW(str1, str2);
        }
    }
    internal static class Shlwapi
    {

        [DllImport("shlwapi.dll", CharSet = CharSet.Unicode)]
        public static extern int StrCmpLogicalW(string psz1, string psz2);

    }
}
