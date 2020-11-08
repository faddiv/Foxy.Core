using Examples.Cleanup;
using Examples.Collections;
using Examples.Linq;
using System;

namespace Examples
{
    public static class Program
    {
        public static void Main()
        {
            Console.WriteLine("Run_FooBazExample");
            FooBazExample.Run_FooBazExample();

            Console.WriteLine();
            Console.WriteLine("Run_AddRange_Example");
            CollectionsExamples.Run_AddRange_Example();

            Console.WriteLine();
            Console.WriteLine("Run_AddElements_Example");
            CollectionsExamples.Run_AddElements_Example();

            Console.WriteLine();
            Console.WriteLine("Run_FindAndRemove_Example");
            CollectionsExamples.Run_FindAndRemoveAndRemoveAll_Example();

            Console.WriteLine();
            Console.WriteLine("Run_ToChunk_Example");
            CollectionsExamples.Run_ToChunk_Example();

            Console.WriteLine();
            Console.WriteLine("Run_ToChunk_Example");
            CollectionsExamples.Run_SortByKey_Example();

            Console.WriteLine();
            Console.WriteLine("Run_LeftJoin_Example");
            LinqExamples.Run_LeftJoin_Example();

            Console.WriteLine();
            Console.WriteLine("Run_NaturalStringComparer_Example");
            TextExamples.Run_NaturalStringComparer_Example();

            Console.WriteLine();
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey(false);
        }
    }
}
