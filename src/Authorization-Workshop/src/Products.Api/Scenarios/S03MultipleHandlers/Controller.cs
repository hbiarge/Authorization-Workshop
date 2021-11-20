using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Products.Api.Scenarios.S03MultipleHandlers;

public static class Policies
{
    public const string AllowedInOffice = "AllowedInOffice";

    public static void AddPolicies(AuthorizationOptions options)
    {
        // Code based policy: Several handlers to validate a requirement
        options.AddPolicy(AllowedInOffice, policyBuilder =>
        {
            policyBuilder.AddRequirements(new OfficeEntryRequirement(department: "CPM"));
        });
    }

    public static IServiceCollection AddHandlers(this IServiceCollection services)
    {
        services.AddSingleton<IAuthorizationHandler, HasBadgeHandler>();
        services.AddSingleton<IAuthorizationHandler, HasTemporaryPassHandler>();

        return services;
    }
}

[Route("api/code-based")]
public class Controller : ControllerBase
{
    [Authorize(Policies.AllowedInOffice)]
    [HttpGet("office")]
    public IActionResult SeveralOptionsToEnterTheOffice()
    {
        return Ok(new[] { "Value1", "Value2" });
    }
}

public class OfficeEntryRequirement : IAuthorizationRequirement
{
    public OfficeEntryRequirement(string department)
    {
        Department = department;
    }

    public string Department { get; }
}

public class HasBadgeHandler : AuthorizationHandler<OfficeEntryRequirement>
{
    private const string ExpectedIssuer = "MySuperSecureIssuer";

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OfficeEntryRequirement requirement)
    {
        var badgeClaim = context.User.FindFirst(c => c.Type == "BadgeNumber");
        var departmentClaim = context.User.FindFirst(c => c.Type == "Department");

        // Both claims should exist
        if (badgeClaim == null || departmentClaim == null)
        {
            return Task.CompletedTask;
        }

        // The issuer of the badgeNumber claim should be known
        if (badgeClaim.Issuer != ExpectedIssuer)
        {
            return Task.CompletedTask;
        }

        // The department should match the requirement department
        if (departmentClaim.Value != requirement.Department)
        {
            return Task.CompletedTask;
        }

        context.Succeed(requirement);

        return Task.CompletedTask;
    }
}

public class HasTemporaryPassHandler : AuthorizationHandler<OfficeEntryRequirement>
{
    private const string ExpectedIssuer = "MySuperSecureIssuer";

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OfficeEntryRequirement requirement)
    {
        var temporaryBadgeClaim = context.User.FindFirst(c => c.Type == "TemporaryBadgeExpiry");

        if (temporaryBadgeClaim == null)
        {
            return Task.CompletedTask;
        }

        // The issuer of the TemporaryBadgeClaim claim should be known
        if (temporaryBadgeClaim.Issuer != ExpectedIssuer)
        {
            return Task.CompletedTask;
        }

        var temporaryBadgeExpiry = Convert.ToDateTime(temporaryBadgeClaim.Value).ToUniversalTime();

        if (temporaryBadgeExpiry > DateTime.UtcNow)
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
