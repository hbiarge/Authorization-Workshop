using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.TestHost;
using Products.Api.IntegrationTests.Infrastructure;
using Products.Api.Scenarios.S03MultipleHandlers;
using Xunit;

namespace Products.Api.IntegrationTests.Specs
{
    [Collection(Collections.Api)]
    public class S03MultipleHandlers
    {
        private readonly TestHostFixture _fixture;

        public S03MultipleHandlers(TestHostFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Users_With_BadgeNumber_And_Correct_Department_Can_Enter_The_Office()
        {
            var builder = _fixture.Server.CreateHttpApiRequest<Controller>(
                controller => controller.SeveralOptionsToEnterTheOffice());

            var response = await builder
                .WithIdentity(Identities.Hugo)
                .GetAsync();

            await response.IsSuccessStatusCodeOrThrow();
        }

        [Fact]
        public async Task Users_With_TempBadge_Can_Enter_The_Office()
        {
            var builder = _fixture.Server.CreateHttpApiRequest<Controller>(
                controller => controller.SeveralOptionsToEnterTheOffice());

            var response = await builder
                .WithIdentity(Identities.Ruben)
                .GetAsync();

            await response.IsSuccessStatusCodeOrThrow();
        }
    }
}
