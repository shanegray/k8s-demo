using DriverService.Handlers;
using Microsoft.Extensions.Hosting;
using ShareMe.Carefully.Rabbit;
using System.Threading;
using System.Threading.Tasks;

namespace DriverService
{
    // This is a simple hosted service. Can be improved with a background service and cancellation tokens
    public class RabbitBackgroundService : IHostedService
    {
        private readonly RabbitHandler rabbitHandler;
        private readonly DriverHiredHandler driverHiredHandler;
        private readonly LoadingVanHandler loadingVanHandler;
        private readonly RunCompletedHandler runCompletedHandler;
        private readonly RunStartedHandler runStartedHandler;

        // Could pull in services through a service provider but meh...
        public RabbitBackgroundService(
            RabbitHandler rabbitHandler, 
            DriverHiredHandler driverHiredHandler,
            LoadingVanHandler loadingVanHandler,
            RunCompletedHandler runCompletedHandler,
            RunStartedHandler runStartedHandler)
        {
            this.rabbitHandler = rabbitHandler;
            this.driverHiredHandler = driverHiredHandler;
            this.loadingVanHandler = loadingVanHandler;
            this.runCompletedHandler = runCompletedHandler;
            this.runStartedHandler = runStartedHandler;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            this.rabbitHandler.Startup();
            this.rabbitHandler.AddListeners(
                this.driverHiredHandler, 
                this.loadingVanHandler,
                this.runCompletedHandler,
                this.runStartedHandler
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
