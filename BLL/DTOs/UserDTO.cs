namespace BLL.DTOs;
using DAL.Models;

public class UserDTO
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; } // Plain password for creation/update, will be hashed in business layer
    public string GoogleId { get; set; }
    public string AuthMethod { get; set; } = UserAuthMethod.Local;
    public string Role { get; set; } = UserRole.Applicant;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class UserSignUpDTO
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; } // Plain password for signup, will be hashed in business layer
    public string? GoogleId { get; set; }
    public string AuthMethod { get; set; } = UserAuthMethod.Local;// 'local' or 'google'
}

public class UserLoginDTO
{
    public string Email { get; set; }
    public string Password { get; set; }
}
