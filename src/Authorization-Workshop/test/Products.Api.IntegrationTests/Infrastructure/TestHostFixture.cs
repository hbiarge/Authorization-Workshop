using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;

namespace Products.Api.IntegrationTests.Infrastructure
{
    public class TestHostFixture : IDisposable
    {
        public TestHostFixture()
        {
            var host = new WebHostBuilder()
                .UseDefaultServiceProvider(x =>
                {
                    x.ValidateScopes = true;
                })
                .UseStartup<TestStartup>();

            Server = new TestServer(host);
        }

        public TestServer Server { get; }

        public void Dispose()
        {
            Server.Dispose();
        }
    }
}