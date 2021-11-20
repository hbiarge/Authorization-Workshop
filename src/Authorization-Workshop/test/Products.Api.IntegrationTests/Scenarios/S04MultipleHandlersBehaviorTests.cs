using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.TestHost;
using Products.Api.IntegrationTests.Infrastructure;
using Products.Api.Scenarios.S04MultipleHandlersBehavior;
using Xunit;

namespace Products.Api.IntegrationTests.Specs
{
    [Collection(Collections.Api)]
    public class S04MultipleHandlersBehaviorTests
    {
        private readonly TestHostFixture _fixture;

        public S04MultipleHandlersBehaviorTests(TestHostFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Values_Is_Authorized_When_Always_Succeed()
        {
            var builder = _fixture.Server.CreateHttpApiRequest<Controller>(
                controller => controller.HandlersBehavior());

            var response = await builder
                .WithIdentity(Identities.Hugo)
                .GetAsync();

            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }
    }
}