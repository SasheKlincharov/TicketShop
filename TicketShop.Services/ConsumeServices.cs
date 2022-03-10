using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using TicketShop.Services.Interface;

namespace TicketShop.Services
{
    public class ConsumeServices : IHostedService
    {
        private readonly IServiceProvider _service;
        public ConsumeServices(IServiceProvider service)
        {
            _service = service;

        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await DoWork();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private async Task DoWork()
        {
            using (var scope = _service.CreateScope())
            {
                var scopedProcessingService = scope.ServiceProvider.GetRequiredService<IBackgroundEmailSender>();
                await scopedProcessingService.DoWork();
            }
        }
    }
}
