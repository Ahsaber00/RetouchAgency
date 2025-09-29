using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using DAL.Models;

namespace RetouchAgency.Authorization
{
    /// <summary>
    /// Authorization handler that allows access if the user is an admin or owns the requested resource
    /// </summary>
    public class AdminOrOwnerHandler : AuthorizationHandler<AdminOrOwnerRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context, 
            AdminOrOwnerRequirement requirement)
        {
            // Get the user's role from claims
            var userRole = context.User.FindFirst(ClaimTypes.Role)?.Value;
            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userRole) || string.IsNullOrEmpty(userId))
            {
                context.Fail();
                return Task.CompletedTask;
            }

            // If user is admin, allow access
            if (userRole == UserRole.Admin)
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            // Get the resource ID from the HTTP context
            var httpContext = context.Resource as HttpContext;
            if (httpContext?.Request.RouteValues.TryGetValue(requirement.ResourceIdParameterName, out var resourceIdObj) == true)
            {
                if (int.TryParse(resourceIdObj?.ToString(), out var resourceId))
                {
                    // If the user ID matches the resource ID, allow access
                    if (int.TryParse(userId, out var currentUserId) && currentUserId == resourceId)
                    {
                        context.Succeed(requirement);
                        return Task.CompletedTask;
                    }
                }
            }

            // If neither admin nor owner, deny access
            context.Fail();
            return Task.CompletedTask;
        }
    }
}