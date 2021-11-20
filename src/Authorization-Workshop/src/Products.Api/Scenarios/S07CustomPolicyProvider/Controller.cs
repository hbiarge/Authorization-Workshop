using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Products.Api.Scenarios.S07CustomPolicyProvider;

public static class Policies
{
    public static IServiceCollection AddHandlers(this IServiceCollection services)
    {
        services.AddSingleton<IAuthorizationHandler, PermissionsAuthorizationHandler>();

        // Replace the default authorization policy provider with our own
        // custom provider which can return authorization policies for given
        // policy names (instead of using the default policy provider)
        services.AddSingleton<IAuthorizationPolicyProvider, PermissionsPolicyProvider>();

        return services;
    }
}

[Route("api/custom-provider")]
public class Controller : ControllerBase
{
    [HasPermission(Permission.Read)]
    [HttpGet("Read")]
    public IActionResult Read()
    {
        return Ok(new[] { "Value1", "Value2" });
    }

    [HasPermission(Permission.Write)]
    [HttpGet("Write")]
    public IActionResult Write()
    {
        return Ok(new[] { "Value1", "Value2" });
    }
}

public enum Permission
{
    None = 0,
    Read,
    Write
}

public class PermissionRequirement : IAuthorizationRequirement
{
    public PermissionRequirement(Permission permission)
    {
        Permission = permission;
    }

    public Permission Permission { get; }
}

public class PermissionsAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionRequirement requirement)
    {
        var claims = context.User
            .FindAll(c => c.Type == ClaimTypes.Role)
            .Select(x => x.Value)
            .ToArray();

        if (claims.Any() == false)
        {
            return Task.CompletedTask;
        }

        if (requirement.Permission == Permission.Read && claims.Contains("Developer"))
        {
            context.Succeed(requirement);
        }

        if (requirement.Permission == Permission.Write && claims.Contains("Master"))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false)]
internal class HasPermissionAttribute : AuthorizeAttribute
{
    public HasPermissionAttribute(Permission permission) => Permission = permission;

    // Get or set the Permission property by manipulating the underlying Policy property
    public Permission Permission
    {
        get
        {
            if (Enum.TryParse<Permission>(Policy.Substring(PermissionsPolicyProvider.PolicyPrefix.Length), out var permission))
            {
                return permission;
            }
            return Permission.None;
        }
        set => Policy = $"{PermissionsPolicyProvider.PolicyPrefix}{value}";
    }
}

public class PermissionsPolicyProvider : DefaultAuthorizationPolicyProvider
{
    public const string PolicyPrefix = "Permission";

    private readonly IOptions<AuthorizationOptions> _options;

    public PermissionsPolicyProvider(IOptions<AuthorizationOptions> options)
        : base(options)
    {
        _options = options;
    }

    // Policies are looked up by string name, so expect 'parameters' (like age)
    // to be embedded in the policy names. This is abstracted away from developers
    // by the more strongly-typed attributes derived from AuthorizeAttribute
    // (like [HasPermissionAttribute] in this sample)
    public override async Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
    {
        // First try to get the policy from the existing policies
        var policy = await base.GetPolicyAsync(policyName);

        if (policy != null)
        {
            return policy;
        }

        // Then check if it´s a dynamic generated policy
        if (policyName.StartsWith(PolicyPrefix, StringComparison.OrdinalIgnoreCase)
            &&
            Enum.TryParse<Permission>(policyName[PolicyPrefix.Length..], out var permission))
        {
            // Generate the policy
            var builder = new AuthorizationPolicyBuilder();
            builder.AddRequirements(new PermissionRequirement(permission));
            policy = builder.Build();

            // Add the policy to the known policies (cache)
            _options.Value.AddPolicy(policyName, policy);

            return policy;
        }

        // If the policy name doesn't match the format expected by this policy provider,
        // this would return null instead.
        return null;
    }
}