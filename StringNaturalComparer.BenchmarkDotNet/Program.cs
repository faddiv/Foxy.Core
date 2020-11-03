using BenchmarkDotNet.Running;
using System;
using System.Diagnostics;
using System.IO;

namespace NaturalStringComparerNS
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            Type benchmark;
            if (args.Length == 1)
            {
                var assembly = typeof(Program).Assembly;
                benchmark = assembly.GetType(args[0]);
            }
            else
            {
                benchmark = typeof(StringComparerBenchmarks);
            }
            BenchmarkRunner.Run(benchmark);
            var processStartInfo = new ProcessStartInfo(
                "c:\\Program Files\\R\\R-3.6.2\\bin\\Rscript.exe",
                "BuildPlots.R")
            {
                WorkingDirectory = Path.GetFullPath($".\\{benchmark.Name}\\results")
            };
            var process = Process.Start(processStartInfo);
            process.WaitForExit();
        }
    }
}
