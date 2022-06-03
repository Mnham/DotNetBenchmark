using BenchmarkDotNet.Running;

namespace DotNetBenchmark
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BenchmarkRunner.Run<Iteration>();
            BenchmarkRunner.Run<ToArrayToList>();
        }
    }
}