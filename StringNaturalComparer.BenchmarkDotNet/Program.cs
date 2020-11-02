using BenchmarkDotNet.Running;
using System.Diagnostics;
using System.IO;

namespace StringNaturalComparerNS
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var benchmark  = typeof(StringComparerBenchmarks);
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
