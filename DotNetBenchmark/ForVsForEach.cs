using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Exporters.Csv;

namespace DotNetBenchmark
{
    [RPlotExporter, CsvMeasurementsExporter]
    public class ForVsForEach
    {
        private int[] _array;
        private List<int> _list;

        [Params(100_000_000)]
        public int Capacity { get; set; }

        [Benchmark]
        public int ForArray()
        {
            int total = 0;
            for (int i = 0; i < _array.Length; i++)
            {
                total += _array[i];
            }

            return total;
        }

        [Benchmark]
        public int ForEachArray()
        {
            int total = 0;
            foreach (int i in _array)
            {
                total += i;
            }

            return total;
        }

        [Benchmark]
        public int ForEachList()
        {
            int total = 0;
            foreach (int i in _list)
            {
                total += i;
            }

            return total;
        }

        [Benchmark]
        public int ForList()
        {
            int total = 0;
            for (int i = 0; i < _list.Count; i++)
            {
                total += _list[i];
            }

            return total;
        }

        [GlobalSetup]
        public void Setup()
        {
            _array = Utils.GetValues(Capacity);
            _list = _array.ToList();
        }
    }
}