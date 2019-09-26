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
        public async Task Should_Be_Forbidden_For_Write()
        {
            var builder = _fixture.Server.CreateHttpApiRequest<S04CustomPolicyProviderController>(
                controller => controller.Write());

            var response = await builder
                .WithIdentity(Identities.HugoBiarge)
                .GetAsync();

            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Should_Be_Success_For_Read()
        {
            var builder = _fixture.Server.CreateHttpApiRequest<S04CustomPolicyProviderController>(
                controller => controller.Read());

            var response = await builder
                .WithIdentity(Identities.HugoBiarge)
                .GetAsync();

            await response.IsSuccessStatusCodeOrThrow();
        }
    }
}