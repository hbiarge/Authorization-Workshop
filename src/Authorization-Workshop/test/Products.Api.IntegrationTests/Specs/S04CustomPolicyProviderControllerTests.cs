using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.TestHost;
using Products.Api.Controllers;
using Products.Api.IntegrationTests.Infrastructure;
using Xunit;

namespace Products.Api.IntegrationTests.Specs
{
    [Collection(Collections.Api)]
    public class S04CustomPolicyProviderControllerTests
    {
        private readonly TestHostFixture _fixture;

        public S04CustomPolicyProviderControllerTests(TestHostFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Should_Be_Forbidden_For_Minimum_50()
        {
            var builder = _fixture.Server.CreateHttpApiRequest<S04CustomPolicyProviderController>(
                controller => controller.Minimum50());

            var response = await builder
                .WithIdentity(Identities.HugoBiarge)
                .GetAsync();

            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Should_Be_Success_For_Minimum_40()
        {
            var builder = _fixture.Server.CreateHttpApiRequest<S04CustomPolicyProviderController>(
                controller => controller.Minimum40());

            var response = await builder
                .WithIdentity(Identities.HugoBiarge)
                .GetAsync();

            await response.IsSuccessStatusCodeOrThrow();
        }
    }
}