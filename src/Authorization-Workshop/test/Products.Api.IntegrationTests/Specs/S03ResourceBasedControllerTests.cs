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
    public class S03ResourceBasedControllerTests
    {
        private readonly TestHostFixture _fixture;

        public S03ResourceBasedControllerTests(TestHostFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task User_Gets_NotFound_For_Non_Existing_Product()
        {
            var builder = _fixture.Server.CreateHttpApiRequest<S03ResourceBasedController>(
                controller => controller.GetProduct("9999"));

            var response = await builder
                .WithIdentity(Identities.HugoBiarge)
                .GetAsync();

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task User_Can_Get_Owned_Products()
        {
            var builder = _fixture.Server.CreateHttpApiRequest<S03ResourceBasedController>(
                controller => controller.GetProduct("1234"));

            var response = await builder
                .WithIdentity(Identities.HugoBiarge)
                .GetAsync();

            await response.IsSuccessStatusCodeOrThrow();
        }

        [Fact]
        public async Task User_Can_Not_Get_Not_Owned_Products()
        {
            var builder = _fixture.Server.CreateHttpApiRequest<S03ResourceBasedController>(
                controller => controller.GetProduct("4321"));

            var response = await builder
                .WithIdentity(Identities.HugoBiarge)
                .GetAsync();

            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }
    }
}