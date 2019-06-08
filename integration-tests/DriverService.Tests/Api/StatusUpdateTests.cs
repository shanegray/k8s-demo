using DriverService.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace DriverService.Tests.Api
{
    public class StatusUpdateTests : IClassFixture<WebApplicationFactory<Startup>>, IClassFixture<DriverTestFixture>
    {
        private readonly HttpClient client;
        private readonly DriverTestFixture fixture;

        public StatusUpdateTests(WebApplicationFactory<Startup> webApplicationFactory, DriverTestFixture fixture)
        {
            this.client = webApplicationFactory.CreateClient();
            this.fixture = fixture;
        }

        [Theory]
        [InlineData("load-van", DriverEventType.LoadingVan)]
        [InlineData("run-started", DriverEventType.RunStarted)]
        [InlineData("run-complete", DriverEventType.RunComplete)]
        public async Task LoadVan_updates_event_store(string urlSegment, DriverEventType expectedType)
        {
            var driverId = await this.fixture.CreateDriverHiredEvent();

            // Act =>
            var response = await this.client.PostAsJsonAsync($"/api/driver/{driverId}/{urlSegment}", new { });

            // Assert => response code
            response.EnsureSuccessStatusCode();

            // Get events from DB
            var events = await this.fixture.GetDriverEventsFromDb(driverId);

            // Assert => two events (one for hire)
            Assert.Equal(2, events.Count);

            var actual = events[1];
            var expected = new EventStore
            {
                DriverId = driverId,
                Event = expectedType
            };

            DeepAssert.Equal(expected, actual, "Id", "Data");
        }


        [Fact]        
        public async Task Bad_event_returns_404()
        {
            var driverId = await this.fixture.CreateDriverHiredEvent();

            // Act =>
            var response = await this.client.PostAsJsonAsync($"/api/driver/{driverId}/went-to-jacks", new { });

            // Assert => response code
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
