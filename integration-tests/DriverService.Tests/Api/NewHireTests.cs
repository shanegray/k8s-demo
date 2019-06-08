using DriverService.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace DriverService.Tests.Api
{
    public class NewHireTests : IClassFixture<WebApplicationFactory<Startup>>, IClassFixture<DriverTestFixture>
    {
        private readonly HttpClient client;
        private readonly DriverTestFixture fixture;

        public NewHireTests(WebApplicationFactory<Startup> webApplicationFactory, DriverTestFixture fixture)
        {
            this.client = webApplicationFactory.CreateClient();
            this.fixture = fixture;
        }

        [Fact]
        public async Task Post_hire_200s()
        {
            var driverToAdd = this.fixture.UniqueHireEvent();
            var response = await this.client.PostAsJsonAsync("/api/driver", driverToAdd);
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Post_adds_to_event_store()
        {
            // Arrange
            var driverToAdd = this.fixture.UniqueHireEvent();

            // Act =>
            await this.client.PostAsJsonAsync("/api/driver", driverToAdd);

            // pull fresh from DB
            var driverEvents = await this.fixture.GetDriverEventsFromDb(driverToAdd.DriverId);

            // Assert => one event with matching high level and BSON data
            Assert.Collection(driverEvents, actual =>
            {
                // Assert => expected event (top level)
                var expected = new EventStore
                {
                    DriverId = driverToAdd.DriverId,
                    Event = DriverEventType.Hired
                };

                DeepAssert.Equal(expected, actual, "Id", "Data");
                BsonAssert.Equal(driverToAdd, actual.Data);
            });
        }
    }
}
