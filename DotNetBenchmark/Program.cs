using BenchmarkDotNet.Running;

namespace DotNetBenchmark
{
    public static class Program
    {
        public static void Main() => BenchmarkRunner.Run<ReactiveVsChannelProduceConsume>();
    }

    public static class Utils
    {
        public static int[] GetValues(int count)
        {
            Random rnd = new();

            return Enumerable.Repeat(0, count)
                .Select(i => rnd.Next(-100, 100))
                .ToArray();
        }
    }
}