using PackageService.Events;
using PackageService.Service;
using ShareMe.Carefully.Rabbit;
using System.Threading.Tasks;

namespace PackageService.Handlers
{
    public class PackageAddedHandler : GenericListener<AddedToNetworkEvent>
    {
        private readonly ReadModelService readModelService;

        public PackageAddedHandler(ReadModelService readModelService)
            : base("package.added.service")
        {
            this.readModelService = readModelService;
        }

        public override async Task<bool> HandleMessage(AddedToNetworkEvent message)
        {
            await this.readModelService.AddPackageToNetwork(message);

            return true;
        }
    }
}
