using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.TestHost;
using Products.Api.IntegrationTests.Infrastructure;
using Products.Api.Scenarios.S01ClaimsBasedPolicy;
using Xunit;

namespace Products.Api.IntegrationTests.Specs
{
    [Collection(Collections.Api)]
    public class S01ClaimsBasedPolicyTests
    {
        private readonly TestHostFixture _fixture;

        public S01ClaimsBasedPolicyTests(TestHostFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Values_Is_Authorized_For_Valid_User()
        {
            var builder = _fixture.Server.CreateHttpApiRequest<Controller>(
                controller => controller.Values());

            var response = await builder
                .WithIdentity(Identities.Hugo)
                .GetAsync();

            await response.IsSuccessStatusCodeOrThrow();
        }

        [Fact]
        public async Task Values_Is_Not_Authorized_Without_User_Information()
        {
            var builder = _fixture.Server.CreateHttpApiRequest<Controller>(
                controller => controller.Values());

            var response = await builder
                .GetAsync();

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task PublicValues_Is_Authorized_Without_User_Information()
        {
            var builder = _fixture.Server.CreateHttpApiRequest<Controller>(
                controller => controller.PublicValues());

            var response = await builder
                .GetAsync();

            await response.IsSuccessStatusCodeOrThrow();
        }
    }
}
