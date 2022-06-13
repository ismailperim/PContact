using Core.Models;
using EasyNetQ;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Report.Service
{
    public class ReportQueueService : IHostedService, IDisposable
    {
        public readonly IBus _rabbitMQBus;
        public ReportQueueService(IOptions<ServiceOptions> options)
        {
            _rabbitMQBus = RabbitHutch.CreateBus(options.Value.MessageQueueOptions?.ConnectionString);
            _rabbitMQBus.Advanced.QueueDeclare(Constant.Q_REPORT);

        }
        public Task StartAsync(CancellationToken stoppingToken)
        {
            _rabbitMQBus.SendReceive.Receive<Models.Report>(Constant.Q_REPORT, (x) => {

                Console.WriteLine(x.ID);
            
            
            });

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _rabbitMQBus?.Dispose();
        }
    }
}
