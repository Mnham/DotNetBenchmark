using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Exporters.Csv;
using BenchmarkDotNet.Jobs;

namespace DotNetBenchmark
{
    [MemoryDiagnoser]
    [RPlotExporter, CsvMeasurementsExporter]
    [SimpleJob(RuntimeMoniker.Net48), SimpleJob(RuntimeMoniker.Net60)]
    public class CollectItems
    {
        private int[] _data;

        public IEnumerable<int> EnumerableProvider()
        {
            foreach (int i in _data)
            {
                if (i % 2 == 0)
                {
                    yield return i;
                }
            }
        }

        [Benchmark]
        public int FromEnumerable()
        {
            int total = 0;
            IEnumerable<int> enumerable = EnumerableProvider();
            foreach (int item in enumerable)
            {
                total += item;
            }

            return total;
        }

        [Benchmark]
        public int FromEnumerableAsArray()
        {
            int total = 0;
            int[] array = EnumerableProvider().ToArray();
            foreach (int item in array)
            {
                total += item;
            }

            return total;
        }

        [Benchmark]
        public int FromList()
        {
            int total = 0;
            List<int> list = ListProvider();
            for (int i = 0; i < list.Count; i++)
            {
                total += list[i];
            }

            return total;
        }

        public List<int> ListProvider()
        {
            List<int> result = new();
            foreach (int i in _data)
            {
                if (i % 2 == 0)
                {
                    result.Add(i);
                }
            }

            return result;
        }

        [GlobalSetup]
        public void Setup()
        {
            Random rnd = new();
            _data = Enumerable.Repeat(0, 10000)
                .Select(i => rnd.Next(-10, 10))
                .ToArray();
        }
    }
}