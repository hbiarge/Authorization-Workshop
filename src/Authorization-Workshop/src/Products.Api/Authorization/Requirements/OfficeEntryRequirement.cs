using Microsoft.AspNetCore.Authorization;

namespace Products.Api.Authorization.Requirements
{
    public class OfficeEntryRequirement : IAuthorizationRequirement
    {
        public OfficeEntryRequirement(string department)
        {
            Department = department;
        }

        public string Department { get; }
    }
}