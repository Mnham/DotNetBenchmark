using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Exporters.Csv;
using BenchmarkDotNet.Jobs;

namespace DotNetBenchmark
{
    [RPlotExporter, CsvMeasurementsExporter]
    [SimpleJob(RuntimeMoniker.Net48), SimpleJob(RuntimeMoniker.Net60)]
    public class Iteration
    {
        private int[] _array;
        private List<int> _list;

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
            Random rnd = new();

            _list = Enumerable.Repeat(0, 10000)
                .Select(i => rnd.Next(-10, 10))
                .ToList();

            _array = _list.ToArray();
        }
    }
}