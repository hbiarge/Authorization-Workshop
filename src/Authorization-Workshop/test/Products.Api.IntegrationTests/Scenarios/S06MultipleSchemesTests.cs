using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.TestHost;
using Products.Api.IntegrationTests.Infrastructure;
using Products.Api.Scenarios.S06MultipleSchemes;
using Xunit;

namespace Products.Api.IntegrationTests.Specs
{
    [Collection(Collections.Api)]
    public class S06MultipleSchemesTests
    {
        private readonly TestHostFixture _fixture;

        public S06MultipleSchemesTests(TestHostFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task When_Use_Different_Schemas_We_Are_Authenticated()
        {
            var builder = _fixture.Server.CreateHttpApiRequest<Controller>(
                controller => controller.Values());

            var response = await builder
                .WithIdentity(Identities.BearerUserClaims, "bearer")
                .WithIdentity(Identities.ExtraUserClaims, "extra")
                .GetAsync();

            await response.IsSuccessStatusCodeOrThrow();
        }
    }
}
