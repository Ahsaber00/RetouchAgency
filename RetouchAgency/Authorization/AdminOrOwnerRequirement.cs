using Microsoft.AspNetCore.Authorization;

namespace RetouchAgency.Authorization
{
    /// <summary>
    /// Authorization requirement that allows access if the user is an admin or owns the requested resource
    /// </summary>
    public class AdminOrOwnerRequirement : IAuthorizationRequirement
    {
        public string ResourceIdParameterName { get; }

        public AdminOrOwnerRequirement(string resourceIdParameterName = "id")
        {
            ResourceIdParameterName = resourceIdParameterName;
        }
    }
}