using DriverService.Handlers;
using DriverService.Models;
using MongoDB.Driver;
using System.Threading.Tasks;
using Xunit;

namespace DriverService.Tests.Rabbit
{
    public class DriverHiredHandlerTests : IClassFixture<DriverTestFixture>
    {
        private readonly DriverHiredHandler sut;
        private readonly DriverTestFixture fixture;

        public DriverHiredHandlerTests(DriverTestFixture fixture)
        {
            this.fixture = fixture;
            this.sut = new DriverHiredHandler(this.fixture.ReadModelService);
        }

        [Fact]
        public async Task New_hired_event_generates_read_model()
        {
            var hireEvent = this.fixture.UniqueHireEvent();

            // Act =>
            await this.sut.HandleMessage(hireEvent);

            // Assert => it's there
            var expected = new Driver
            {
                DriverId = hireEvent.DriverId,
                Name = hireEvent.Name,
                Status = DriverStatus.Idle
            };

            // get actual from DB
            var filter = Builders<Driver>.Filter.Eq(t => t.DriverId, hireEvent.DriverId);
            var actual = await this.fixture.DriverCollection.Find(filter).FirstAsync();

            DeepAssert.Equal(expected, actual, "Id");
        }
    }
}
