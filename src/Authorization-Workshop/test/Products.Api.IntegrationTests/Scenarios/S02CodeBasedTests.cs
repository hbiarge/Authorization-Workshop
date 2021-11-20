using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.TestHost;
using Products.Api.IntegrationTests.Infrastructure;
using Products.Api.Scenarios.S02CodeBasedPolicy;
using Xunit;

namespace Products.Api.IntegrationTests.Specs
{
    [Collection(Collections.Api)]
    public class S02CodeBasedTests
    {
        private readonly TestHostFixture _fixture;

        public S02CodeBasedTests(TestHostFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Only_Users_Over_The_Allowed_Age()
        {
            var builder = _fixture.Server.CreateHttpApiRequest<Controller>(
                controller => controller.OnlyForOldPeople());

            var response = await builder
                .WithIdentity(Identities.Hugo)
                .GetAsync();

            await response.IsSuccessStatusCodeOrThrow();
        }
    }
}
