using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Exporters.Csv;

namespace DotNetBenchmark
{
    [MemoryDiagnoser]
    [RPlotExporter, CsvMeasurementsExporter]
    public class MaterializeEnumerable
    {
        private int[] _values;

        [Params(10_000_000)]
        public int Capacity { get; set; }

        [GlobalSetup]
        public void Setup() => _values = Utils.GetValues(Capacity);

        [Benchmark]
        public int[] ToArray() => Generator().ToArray();

        [Benchmark]
        public List<int> ToList() => Generator().ToList();

        private IEnumerable<int> Generator()
        {
            foreach (int value in _values)
            {
                if (value % 2 == 0)
                {
                    yield return value;
                }
            }
        }
    }
}