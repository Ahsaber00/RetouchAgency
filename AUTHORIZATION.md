# Custom Authorization: AdminOrOwner

This custom authorization attribute allows access to endpoints if the user is either:
1. An **Admin** user (based on their role claim)
2. The **Owner** of the resource (their user ID matches the requested resource ID)

## How to Use

### Basic Usage
```csharp
[HttpGet("{id}")]
[AdminOrOwner] // Allows admin or user with matching ID to access
public async Task<IActionResult> GetUserById(int id)
{
    // Your code here
}
```

### Custom Parameter Name
If your route parameter is not named "id", you can specify the parameter name:
```csharp
[HttpGet("{userId}")]
[AdminOrOwner("userId")] // Allows admin or user with matching userId
public async Task<IActionResult> GetUserProfile(int userId)
{
    // Your code here
}
```

## How It Works

1. **Admin Check**: If the user's role claim is "admin", access is granted
2. **Owner Check**: If the user's ID (from NameIdentifier claim) matches the route parameter value, access is granted
3. **Deny Access**: If neither condition is met, access is denied (403 Forbidden)

## JWT Token Requirements

Your JWT token must include these standard claims:
- `ClaimTypes.Role`: The user's role (e.g., "admin", "applicant")
- `ClaimTypes.NameIdentifier`: The user's ID

## Examples

### User Controller
```csharp
[HttpGet("{id}")]
[AdminOrOwner] // Admin can see any user, users can only see themselves
public async Task<IActionResult> GetUserById(int id) { ... }

[HttpPut("{id}")]
[AdminOrOwner] // Admin can update any user, users can only update themselves
public async Task<IActionResult> UpdateUser(int id, [FromBody] UserDTO user) { ... }

[HttpDelete("{id}")]
[AdminOrOwner] // Admin can delete any user, users can only delete themselves
public async Task<IActionResult> DeleteUser(int id) { ... }
```

### Profile Controller (if you have different parameter names)
```csharp
[HttpGet("profile/{userId}")]
[AdminOrOwner("userId")] // Note: parameter name specified
public async Task<IActionResult> GetProfile(int userId) { ... }
```

## Benefits

1. **Clean Code**: No need to manually check user roles and IDs in every controller method
2. **Reusable**: Can be applied to any controller method with an ID parameter
3. **Flexible**: Supports different parameter names
4. **Secure**: Follows ASP.NET Core authorization patterns
5. **Maintainable**: All authorization logic is centralized

## Alternative Approaches

### Admin Only
```csharp
[Authorize(Roles = UserRole.Admin)] // Only admin can access
```

### Any Authenticated User
```csharp
[Authorize] // Any authenticated user can access
```

### Custom Logic in Controller
If you need more complex authorization logic, you can still implement it manually in the controller method.