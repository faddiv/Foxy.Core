using Foxy.Core.Text;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Examples
{
    public static class TextExamples
    {
        public static void Run_NaturalStringComparer_Example()
        {
            var list = GetFiles();
            list.Sort(NaturalStringComparer.OrdinalIgnoreCase);

            PrintResult(list);
        }

        private static void PrintResult(List<string> list)
        {
            foreach (var item in list)
            {
                Console.WriteLine(item);
            }
        }

        private static List<string> GetFiles()
        {
            var rnd = new Random();
            return Enumerable.Range(0, 20)
                .Select(e => $"File {rnd.Next(1, 100)}.txt")
                .ToList();
        }
    }
}
