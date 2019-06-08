using DriverService.Events;
using DriverService.Handlers;
using DriverService.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DriverService.Tests.Rabbit
{
    public class StatusHandlerTests : IClassFixture<DriverTestFixture>
    {
        private readonly DriverTestFixture fixture;

        public StatusHandlerTests(DriverTestFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public async Task LoadVan_updates_status()
        {
            // Arrange => system under test & create driver
            var sut = new LoadingVanHandler(this.fixture.ReadModelService);
            // Arrange => Create driver for updates
            var driver = await this.fixture.CreateDriverModel();

            // Act =>
            await sut.HandleMessage(new TimedEvent { DriverId = driver.DriverId });

            // Assert
            await AssertExcepted(driver, DriverStatus.LoadingVan);
        }

        [Fact]
        public async Task RunStarted_updates_status()
        {
            // Arrange => system under test & create driver
            var sut = new RunStartedHandler(this.fixture.ReadModelService);
            // Arrange => Create driver for updates
            var driver = await this.fixture.CreateDriverModel();

            // Act =>
            await sut.HandleMessage(new TimedEvent { DriverId = driver.DriverId });

            // Assert
            await AssertExcepted(driver, DriverStatus.OnRun);
        }

        private async Task AssertExcepted(Driver driver, DriverStatus expectedStatus)
        {
            var expected = new Driver
            {
                DriverId = driver.DriverId,
                Name = driver.Name,
                Status = expectedStatus
            };
            var actual = await this.fixture.GetDriverFromDb(driver.DriverId);

            DeepAssert.Equal(expected, actual, "Id");
        }
    }
}