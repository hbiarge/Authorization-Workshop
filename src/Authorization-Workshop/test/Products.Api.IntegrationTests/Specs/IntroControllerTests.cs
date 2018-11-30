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
    public class IntroControllerTests
    {
        private readonly TestHostFixture _fixture;

        public IntroControllerTests(TestHostFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Values_Is_Authorized_For_Valid_User()
        {
            var builder = _fixture.Server.CreateHttpApiRequest<IntroController>(controller => controller.Values());
            
            var response = await builder
                .WithIdentity(Identities.HugoBiarge)
                .GetAsync();

            await response.IsSuccessStatusCodeOrThrow();
        }

        [Fact]
        public async Task Values_Is_Not_Authorized_For_Without_User_Information()
        {
            var builder = _fixture.Server.CreateHttpApiRequest<IntroController>(controller => controller.Values());

            var response = await builder
                .GetAsync();

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task PublicValues_Is_Authorized_Without_User_Information()
        {
            var builder =_fixture.Server.CreateHttpApiRequest<IntroController>(controller => controller.PublicValues());

            var response = await builder
                .GetAsync();

            await response.IsSuccessStatusCodeOrThrow();
        }
    }
}
