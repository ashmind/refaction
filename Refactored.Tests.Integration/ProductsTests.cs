using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using NUnit.Framework;
using Refactored.Models;

namespace Refactored.Tests.Integration
{
    using static TestData;

    public class ProductsTests : IntegrationTestBase
    {
        [Test]
        public async Task GetAll_ReturnsAllProducts()
        {
            var products = await GetAsync<ListModel<Product>>("/products");
            CollectionAssert.AreEqual(
                new[]
                {
                    new
                    {
                        Name = "Samsung Galaxy S7",
                        Description = "Newest mobile product from Samsung.",
                        Price = 1024.99M,
                        DeliveryPrice = 16.99M
                    },
                    new
                    {
                        Name = "Apple iPhone 6S",
                        Description = "Newest mobile product from Apple.",
                        Price = 1299.99M,
                        DeliveryPrice = 15.99M
                    }
                },
                products.Items.Select(p => new { p.Name, p.Description, p.Price, p.DeliveryPrice }).ToArray()
            );
        }

        [Test]
        public async Task GetByName_ReturnsProductWithGivenName()
        {
            var products = await GetAsync<ListModel<Product>>("/products?name=Samsung%20Galaxy%20S7");
            CollectionAssert.AreEqual(
                new[] { "Samsung Galaxy S7" },
                products.Items.Select(p => p.Name).ToArray()
            );
        }

        [Test]
        public async Task GetById_ReturnsProductWithGivenId()
        {
            var product = await GetAsync<Product>($"/products/{GalaxyProductId}");

            Assert.AreEqual(GalaxyProductId, product.Id);
            Assert.AreEqual("Samsung Galaxy S7", product.Name);
            Assert.AreEqual("Newest mobile product from Samsung.", product.Description);
            Assert.AreEqual(1024.99M, product.Price);
            Assert.AreEqual(16.99M, product.DeliveryPrice);
        }

        [Test]
        public async Task GetById_Returns404_IfProductWasNotFound()
        {
            var response = await Server.HttpClient.GetAsync($"/products/{Guid.Empty}");
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Test]
        public async Task Post_CreatesProductThatCanBeRetrieved()
        {
            var product = new Product { Id = Guid.NewGuid(), Name = "Test Name", Description = "Test Description", Price = 1000, DeliveryPrice = 10 };
            await PostAsync("/products", product);
            var retrieved = await GetAsync<Product>($"/products/{product.Id}");

            AssertProductsAreEqual(product, retrieved);
        }

        [Test]
        public async Task Put_UpdatesProduct_IfItExists()
        {
            var updated = new Product { Name = "Updated Name", Description = "Updated Description", Price = 1000, DeliveryPrice = 10 };
            await PutAsync($"/products/{GalaxyProductId}", updated);
            var retrieved = await GetAsync<Product>($"/products/{GalaxyProductId}");

            AssertProductsAreEqual(updated, retrieved, ignoreId: true);
        }

        [Test]
        public async Task Put_CreatesProduct_IfItDidNotExist()
        {
            var product = new Product { Id = Guid.NewGuid(), Name = "Test Name", Description = "Test Description", Price = 1000, DeliveryPrice = 10 };
            await PutAsync($"/products/{product.Id}", product);
            var retrieved = await GetAsync<Product>($"/products/{product.Id}");

            AssertProductsAreEqual(product, retrieved);
        }

        [Test]
        public async Task Delete_DeletesProduct()
        {
            await DeleteAsync($"/products/{GalaxyProductId}");
            var response = await Server.HttpClient.GetAsync($"/products/{GalaxyProductId}");

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        private static void AssertProductsAreEqual(Product expected, Product actual, bool ignoreId = false)
        {
            if (!ignoreId)
                Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.Description, actual.Description);
            Assert.AreEqual(expected.Price, actual.Price);
            Assert.AreEqual(expected.DeliveryPrice, actual.DeliveryPrice);
        }
    }
}
