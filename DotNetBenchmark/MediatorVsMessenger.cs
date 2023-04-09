using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Exporters.Csv;

using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;

using MediatR;

using Microsoft.Extensions.DependencyInjection;

namespace DotNetBenchmark
{
    [MemoryDiagnoser]
    [RPlotExporter, CsvMeasurementsExporter]
    public class MediatorVsMessenger
    {
        private IMediator _mediator;
        private IMessenger _messenger;
        private MessageRecipient _recipient;

        [Params(100_000)]
        public int Iterations { get; set; }

        [Benchmark]
        public async Task<int> MediatR()
        {
            int total = 0;
            for (int i = 0; i < Iterations; i++)
            {
                total += await _mediator.Send(new Request());
            }

            return total;
        }

        [Benchmark]
        public async Task<int> Messenger()
        {
            int total = 0;
            for (int i = 0; i < Iterations; i++)
            {
                total += await _messenger.Send<Request>();
            }

            return total;
        }

        [GlobalSetup]
        public void Setup()
        {
            ServiceProvider serviceProvider = new ServiceCollection()
                .AddMediatR(config => config.RegisterServicesFromAssemblyContaining<MediatorVsMessenger>())
                .AddSingleton<IMessenger, WeakReferenceMessenger>()
                .AddSingleton<MessageRecipient>()
                .BuildServiceProvider();

            _mediator = serviceProvider.GetService<IMediator>();
            _messenger = serviceProvider.GetService<IMessenger>();
            _recipient = serviceProvider.GetService<MessageRecipient>();
        }
    }

    public sealed class MessageRecipient : IRecipient<Request>
    {
        private readonly IServiceProvider _services;

        public MessageRecipient(IMessenger messenger, IServiceProvider services)
        {
            messenger.Register(this);
            _services = services;
        }

        public void Receive(Request message)
        {
            IRequestHandler<Request, int> handler = _services.GetService<IRequestHandler<Request, int>>();
            message.Reply(handler.Handle(message, default));
        }
    }

    public sealed class Request : AsyncRequestMessage<int>, IRequest<int>
    {
    }

    public sealed class RequestHandler : IRequestHandler<Request, int>
    {
        private static readonly Random _rnd = new();

        Task<int> IRequestHandler<Request, int>.Handle(Request request, CancellationToken cancellationToken) =>
            Task.FromResult(_rnd.Next(-100, 100));
    }
}