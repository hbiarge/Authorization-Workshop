using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.TestHost;
using Products.Api.IntegrationTests.Infrastructure;
using Products.Api.Scenarios.S07CustomPolicyProvider;
using Xunit;

namespace Products.Api.IntegrationTests.Specs
{
    [Collection(Collections.Api)]
    public class S07CustomPolicyProviderTests
    {
        private readonly TestHostFixture _fixture;

        public S07CustomPolicyProviderTests(TestHostFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Should_Be_Forbidden_For_Write()
        {
            var builder = _fixture.Server.CreateHttpApiRequest<Controller>(
                controller => controller.Write());

            var response = await builder
                .WithIdentity(Identities.Hugo)
                .GetAsync();

            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Should_Be_Success_For_Read()
        {
            var builder = _fixture.Server.CreateHttpApiRequest<Controller>(
                controller => controller.Read());

            var response = await builder
                .WithIdentity(Identities.Hugo)
                .GetAsync();

            await response.IsSuccessStatusCodeOrThrow();
        }
    }
}