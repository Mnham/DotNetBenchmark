using System.Reactive.Linq;
using System.Threading.Channels;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Exporters.Csv;

namespace DotNetBenchmark
{
    [MemoryDiagnoser]
    [RPlotExporter, CsvMeasurementsExporter]
    public class ReactiveVsChannelProduceConsume
    {
        private Channel<int> _channel;
        private bool _observerIsEnabled;
        private int[] _values;

        [Params(1_000_000)]
        public int Iterations { get; set; }

        private event Action<int> ActionEvent;

        [Benchmark(Description = "Channel")]
        public async Task<int> Benchmark1()
        {
            IAsyncEnumerable<int> values = StartChannelPolling();
            _ = Task.Run(StartChannelProducer);

            int total = 0;
            await foreach (int value in values)
            {
                total += value;
            }

            return total;
        }

        [Benchmark(Description = "Reactive")]
        public async Task<long> Benchmark2()
        {
            IAsyncEnumerable<int> values = StartReactivePolling();
            _ = Task.Run(StartReactiveProducer);

            long total = 0;
            await foreach (int value in values)
            {
                total += value;
            }

            return total;
        }

        [GlobalSetup]
        public void Setup() => _values = Utils.GetValues(Iterations);

        private IAsyncEnumerable<int> StartChannelPolling()
        {
            _channel = Channel.CreateUnbounded<int>(new UnboundedChannelOptions
            {
                SingleWriter = true,
                SingleReader = true,
                AllowSynchronousContinuations = false,
            });

            return _channel.Reader.ReadAllAsync();
        }

        private async Task StartChannelProducer()
        {
            await Task.Delay(1);

            foreach (int value in _values)
            {
                await _channel.Writer.WriteAsync(value);
            }

            _channel.Writer.Complete();
        }

        private IAsyncEnumerable<int> StartReactivePolling()
        {
            _observerIsEnabled = true;

            return Observable
                .FromEvent<int>(
                    act => ActionEvent += act,
                    act => ActionEvent -= act)
                .TakeWhile(_ => _observerIsEnabled)
                .ToAsyncEnumerable();
        }

        private async Task StartReactiveProducer()
        {
            await Task.Delay(1);

            foreach (int value in _values)
            {
                ActionEvent?.Invoke(value);
            }

            _observerIsEnabled = false;
            ActionEvent?.Invoke(0);
        }
    }
}