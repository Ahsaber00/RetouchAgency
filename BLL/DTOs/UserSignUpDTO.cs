using DAL.Models;

namespace BLL.DTOs
{
    public class UserSignUpDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; } // Plain password for signup, will be hashed in business layer
        public string? GoogleId { get; set; }
        public string AuthMethod { get; set; } = UserAuthMethod.Local;// 'local' or 'google'
    }
}
