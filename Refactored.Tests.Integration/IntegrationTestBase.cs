using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using System.Transactions;
using System.Web.Http;
using Microsoft.Owin.Testing;
using NUnit.Framework;
using Owin;

namespace Refactored.Tests.Integration
{
    [SingleThreaded] // Integration Tests, share the DB
    public class IntegrationTestBase
    {
        private TransactionScope _transaction;

        [SetUp]
        public void BeforeEachTest()
        {
            // I'm cheating here a bit -- I want to reset the state between tests,
            // and given that I'm using the TestServer which is in the same process
            // and task context, this is a simple solution. It's pretty easy to 
            // run into its limitations though.
            _transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            Server = TestServer.Create(app =>
            {
                var configuration = new HttpConfiguration();
                WebApiConfig.Register(configuration);
                app.UseWebApi(configuration);
            });
        }

        [TearDown]
        public void AfterEachTest()
        {
            Server.Dispose();
            _transaction.Dispose();
        }

        protected TestServer Server { get; private set; }

        protected async Task<T> GetAsync<T>(string url)
        {
            var response = await Server.HttpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<T>();
        }

        protected async Task PostAsync<T>(string url, T body)
        {
            (await Server.HttpClient.PostAsync(url, new ObjectContent<T>(body, new JsonMediaTypeFormatter()))).EnsureSuccessStatusCode();
        }

        protected async Task PutAsync<T>(string url, T body)
        {
            (await Server.HttpClient.PutAsync(url, new ObjectContent<T>(body, new JsonMediaTypeFormatter()))).EnsureSuccessStatusCode();
        }

        protected async Task DeleteAsync(string url)
        {
            (await Server.HttpClient.DeleteAsync(url)).EnsureSuccessStatusCode();
        }
    }
}
