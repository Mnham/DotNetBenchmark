using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Exporters.Csv;
using BenchmarkDotNet.Jobs;

namespace DotNetBenchmark
{
    [MemoryDiagnoser]
    [RPlotExporter, CsvMeasurementsExporter]
    [SimpleJob(RuntimeMoniker.Net48), SimpleJob(RuntimeMoniker.Net60)]
    public class MaterializeEnumerable
    {
        private IEnumerable<int> _enumerable;

        [Benchmark]
        public int[] EnumerableToArray() => _enumerable.ToArray();

        [Benchmark]
        public List<int> EnumerableToList() => _enumerable.ToList();

        [GlobalSetup]
        public void Setup() => _enumerable = Enumerable.Repeat(0, 10000).Select(i => i);
    }
}