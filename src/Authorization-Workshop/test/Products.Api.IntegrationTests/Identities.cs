using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Products.Api.IntegrationTests
{
    public static class Identities
    {
        public static readonly IEnumerable<Claim> Hugo = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, "HB04356"),
            new Claim(ClaimTypes.Name, "Hugo Biarge"),
            new Claim(ClaimTypes.Country, "Spain"),
            new Claim(ClaimTypes.DateOfBirth, "1971-12-20", ClaimValueTypes.Date),
            new Claim(ClaimTypes.Email, "hbiarge@gmail.com", ClaimValueTypes.Email),
            new Claim(ClaimTypes.Role, "Developer"),
            new Claim(ClaimTypes.Role, "Buddy"),
            new Claim("Department", "CPM"),
            new Claim("BadgeNumber", "HB04356", ClaimValueTypes.String, "MySuperSecureIssuer"),
        };

        public static readonly IEnumerable<Claim> Ruben = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, "RG95478"),
            new Claim(ClaimTypes.Name, "Ruben Garcia"),
            new Claim("TemporaryBadgeExpiry", DateTime.UtcNow.AddDays(1).ToString("O"), ClaimValueTypes.Date, "MySuperSecureIssuer"),
        };

        public static readonly IEnumerable<Claim> BearerUserClaims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, "1"),
            new Claim("name", "Standard user"),
            new Claim("role", "Operator")
        };

        public static readonly IEnumerable<Claim> ExtraUserClaims = new[]
        {
            new Claim("Supervisor", "The big boss")
        };
    }
}
