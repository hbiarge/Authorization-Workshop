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
    public class SchemasControllerTests
    {
        private readonly TestHostFixture _fixture;

        public SchemasControllerTests(TestHostFixture fixture)
        {
            _fixture = fixture;
        }

        //[Fact]
        //public async Task When_Use_Different_Schemas_We_Are_Authenticated()
        //{
        //    var builder = _fixture.Server.CreateHttpApiRequest<SchemasController>(controller => controller.Values());

        //    var response = await builder
        //        .WithIdentity(Identities.BearerUserClaims, "bearer")
        //        .WithIdentity(Identities.ExtraUserClaims, "extra")
        //        .GetAsync();

        //    await response.IsSuccessStatusCodeOrThrow();
        //}
    }
}
