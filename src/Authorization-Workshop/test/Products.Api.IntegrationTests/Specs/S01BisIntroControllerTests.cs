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
    public class S01BisIntroControllerTests
    {
        private readonly TestHostFixture _fixture;

        public S01BisIntroControllerTests(TestHostFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Values_Is_Authorized_When_Always_Succeed()
        {
            var builder = _fixture.Server.CreateHttpApiRequest<S01BisIntroController>(
                controller => controller.HandlersBehavior());

            var response = await builder
                .WithIdentity(Identities.HugoBiarge)
                .GetAsync();

            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }
    }
}