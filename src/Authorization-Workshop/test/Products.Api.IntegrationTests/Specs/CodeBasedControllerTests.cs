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
    public class CodeBasedControllerTests
    {
        private readonly TestHostFixture _fixture;

        public CodeBasedControllerTests(TestHostFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Only_Users_Over_The_Allowed_Age()
        {
            var builder = _fixture.Server.CreateHttpApiRequest<CodeBasedController>(controller => controller.OnlyForOldPeople());

            var response = await builder
                .WithIdentity(Identities.HugoBiarge)
                .GetAsync();

            await response.IsSuccessStatusCodeOrThrow();
        }

        [Fact]
        public async Task Users_With_BadgeNumber_And_Correct_Department_Can_Enter_The_Office()
        {
            var builder = _fixture.Server.CreateHttpApiRequest<CodeBasedController>(controller => controller.SeveralOptionsToEnterTheOffice());

            var response = await builder
                .WithIdentity(Identities.HugoBiarge)
                .GetAsync();

            await response.IsSuccessStatusCodeOrThrow();
        }

        [Fact]
        public async Task Users_With_TempBadge_Can_Enter_The_Office()
        {
            var builder = _fixture.Server.CreateHttpApiRequest<CodeBasedController>(controller => controller.SeveralOptionsToEnterTheOffice());

            var response = await builder
                .WithIdentity(Identities.RubenGarcia)
                .GetAsync();

            await response.IsSuccessStatusCodeOrThrow();
        }
    }
}
