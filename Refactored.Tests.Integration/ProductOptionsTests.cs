using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using NUnit.Framework;
using Refactored.Models;

namespace Refactored.Tests.Integration
{
    using static TestData;

    public class ProductOptionsTests : IntegrationTestBase
    {
        [Test]
        public async Task GetByProductId_ReturnsAllProductOptionsWithGivenProductId()
        {
            var options = await GetAsync<ListModel<ProductOption>>($"/products/{GalaxyProductId}/options");
            CollectionAssert.AreEqual(
                new[]
                {
                    new { Name = "White", Description = "White Samsung Galaxy S7" },
                    new { Name = "Black", Description = "Black Samsung Galaxy S7" }
                },
                options.Items.Select(o => new { o.Name, o.Description }).ToArray()
            );
        }

        [Test]
        public async Task GetById_ReturnsOptionWithGivenId()
        {
            var option = await GetAsync<ProductOption>($"/products/{GalaxyProductId}/options/{GalaxyWhiteOptionId}");
            Assert.AreEqual(GalaxyWhiteOptionId, option.Id);
            Assert.AreEqual("White", option.Name);
            Assert.AreEqual("White Samsung Galaxy S7", option.Description);
        }

        [Test]
        public async Task GetById_Returns404_IfOptionWasNotFound()
        {
            var response = await Server.HttpClient.GetAsync($"/products/{GalaxyProductId}/options/{Guid.Empty}");
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Test]
        public async Task GetById_Returns404_IfOptionProductIdDoesNotMatchRequestedId()
        {
            var response = await Server.HttpClient.GetAsync($"/products/{IPhoneProductId}/options/{GalaxyWhiteOptionId}");
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Test]
        public async Task Post_CreatesOptionThatCanBeRetrieved()
        {
            var option = new ProductOption { Id = Guid.NewGuid(), Name = "Test Name", Description = "Test Description" };
            await PostAsync($"/products/{GalaxyProductId}/options", option);
            var retrieved = await GetAsync<ProductOption>($"/products/{GalaxyProductId}/options/{option.Id}");

            Assert.AreEqual(option.Id, retrieved.Id);
            Assert.AreEqual(option.Name, retrieved.Name);
            Assert.AreEqual(option.Description, retrieved.Description);
        }

        [Test]
        public async Task Put_UpdatesOption_IfItExists()
        {
            var updated = new ProductOption { Name = "Updated Name", Description = "Updated Description" };
            await PutAsync($"/products/{GalaxyProductId}/options/{GalaxyWhiteOptionId}", updated);
            var retrieved = await GetAsync<ProductOption>($"/products/{GalaxyProductId}/options/{GalaxyWhiteOptionId}");

            Assert.AreEqual(updated.Description, retrieved.Description);
            Assert.AreEqual(updated.Name, retrieved.Name);
        }

        [Test]
        public async Task Put_CreatesOption_IfItDidNotExist()
        {
            var option = new ProductOption { Id = Guid.NewGuid(), Name = "Test Name", Description = "Test Description" };
            await PutAsync($"/products/{GalaxyProductId}/options/{option.Id}", option);
            var retrieved = await GetAsync<ProductOption>($"/products/{GalaxyProductId}/options/{option.Id}");

            Assert.AreEqual(option.Id, retrieved.Id);
            Assert.AreEqual(option.Name, retrieved.Name);
            Assert.AreEqual(option.Description, retrieved.Description);
        }

        [Test]
        public async Task Delete_DeletesOption()
        {
            await DeleteAsync($"/products/{GalaxyProductId}/options/{GalaxyWhiteOptionId}");
            var response = await Server.HttpClient.GetAsync($"/products/{GalaxyProductId}/options/{GalaxyWhiteOptionId}");

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
