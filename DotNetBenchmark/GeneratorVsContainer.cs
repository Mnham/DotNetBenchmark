using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Exporters.Csv;

namespace DotNetBenchmark
{
    [MemoryDiagnoser]
    [RPlotExporter, CsvMeasurementsExporter]
    public class GeneratorVsContainer
    {
        private int[] _values;

        [Params(10_000_000)]
        public int Capacity { get; set; }

        [Benchmark(Description = "Generator")]
        public int Benchmark1()
        {
            int total = 0;
            IEnumerable<int> enumerable = Generator();
            foreach (int value in enumerable)
            {
                total += value;
            }

            return total;
        }

        [Benchmark(Description = "GeneratorToArray")]
        public int Benchmark2()
        {
            int total = 0;
            int[] array = Generator().ToArray();
            foreach (int value in array)
            {
                total += value;
            }

            return total;
        }

        [Benchmark(Description = "Container")]
        public int Benchmark3()
        {
            int total = 0;
            List<int> list = Container();
            for (int i = 0; i < list.Count; i++)
            {
                total += list[i];
            }

            return total;
        }

        [GlobalSetup]
        public void Setup() => _values = Utils.GetValues(Capacity);

        private List<int> Container()
        {
            List<int> result = new();
            foreach (int value in _values)
            {
                if (value % 2 == 0)
                {
                    result.Add(value);
                }
            }

            return result;
        }

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