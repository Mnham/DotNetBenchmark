using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Exporters.Csv;
using BenchmarkDotNet.Jobs;

namespace DotNetBenchmark
{
    [MemoryDiagnoser]
    [RPlotExporter, CsvMeasurementsExporter]
    [SimpleJob(RuntimeMoniker.Net48), SimpleJob(RuntimeMoniker.Net60)]
    public class ToArrayToList
    {
        private int[] _array;
        private List<int> _list;

        [Benchmark]
        public int[] ArrayToArray() => _array.ToArray();

        [Benchmark]
        public List<int> ArrayToList() => _array.ToList();

        [Benchmark]
        public int[] ListToArray() => _list.ToArray();

        [Benchmark]
        public List<int> ListToList() => _list.ToList();

        [GlobalSetup]
        public void Setup()
        {
            Random rnd = new();
            _list = Enumerable.Repeat(0, 1000)
                .Select(i => rnd.Next(-10, 10))
                .ToList();
            _array = _list.ToArray();
        }
    }
}