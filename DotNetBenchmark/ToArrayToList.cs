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
            _list = Enumerable.Repeat(0, 10000).Select(i => i).ToList();
            _array = _list.ToArray();
        }
    }
}