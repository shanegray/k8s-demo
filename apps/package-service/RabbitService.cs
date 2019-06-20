using Microsoft.Extensions.Hosting;
using PackageService.Handlers;
using ShareMe.Carefully.Rabbit;
using System.Threading;
using System.Threading.Tasks;

namespace PackageService
{
    public class RabbitService : IHostedService
    {
        private readonly RabbitHandler rabbitHandler;

        private readonly DipHandler dipHandler;
        private readonly DncHandler dncHandler;
        private readonly HipHandler hipHandler;
        private readonly PackageAddedHandler packageAddedHandler;
        private readonly PodHandler podHandler;
        private readonly VipHandler vipHandler;
        private readonly VopHandler vopHandler;

        public RabbitService(
            RabbitHandler rabbitHandler,
            DipHandler dipHandler,
            DncHandler dncHandler,
            HipHandler hipHandler,
            PackageAddedHandler packageAddedHandler,
            PodHandler podHandler,
            VipHandler vipHandler,
            VopHandler vopHandler)
        {
            this.rabbitHandler = rabbitHandler;
            this.dipHandler = dipHandler;
            this.dncHandler = dncHandler;
            this.hipHandler = hipHandler;
            this.packageAddedHandler = packageAddedHandler;
            this.podHandler = podHandler;
            this.vipHandler = vipHandler;
            this.vopHandler = vopHandler;
        }


        public Task StartAsync(CancellationToken cancellationToken)
        {
            this.rabbitHandler.Startup();
            this.rabbitHandler.AddListeners(
                this.dipHandler,
                this.dncHandler,
                this.hipHandler,
                this.packageAddedHandler,
                this.podHandler,
                this.vipHandler,
                this.vopHandler
            );

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            this.rabbitHandler.ShutDown();
            return Task.CompletedTask;
        }
    }
}
