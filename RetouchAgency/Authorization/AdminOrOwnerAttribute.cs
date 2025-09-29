using Microsoft.AspNetCore.Authorization;

namespace RetouchAgency.Authorization
{
    /// <summary>
    /// Authorization attribute that allows access if the user is an admin or owns the requested resource
    /// Usage: [AdminOrOwner] or [AdminOrOwner("userId")] if the parameter name is different from "id"
    /// </summary>
    public class AdminOrOwnerAttribute : AuthorizeAttribute
    {
        public AdminOrOwnerAttribute(string resourceIdParameterName = "id")
        {
            Policy = $"AdminOrOwner_{resourceIdParameterName}";
        }
    }
}