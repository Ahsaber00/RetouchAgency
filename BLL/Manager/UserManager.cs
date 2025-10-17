using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using BLL.Configuration;
using BLL.DTOs;
using BLL.Manager.Interfaces;
using DAL.Interfaces;
using DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Net.Mail;


namespace BLL.Manager
{
    public class UserManager(IUnitOfWork unitOfWork, JwtOptions jwtOptions, EmailSettings emailSettings) : IUserManager
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly JwtOptions _jwtOptions = jwtOptions;
        private readonly EmailSettings _emailSettings = emailSettings;

        public async Task<IEnumerable<UserDTO>> GetAllUsersAsync()
        {
            var users = await _unitOfWork.Users.GetAllAsync();
            return users.Select(u => new UserDTO
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
               
                Role = u.Role,
                CreatedAt = u.CreatedAt
            });
        }

        public async Task<UserDTO?> GetUserByIdAsync(int id)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id);
            if (user == null) return null;
            return new UserDTO
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                
                Role = user.Role,
                CreatedAt = user.CreatedAt
            };
        }

        public async Task<int> CreateUserAsync(UserDTO userDTO)
        {
            // Check if user already exists by email
            var existingUsers = await _unitOfWork.Users.GetByEmailAsync(userDTO.Email);
            if (existingUsers != null)
                throw new InvalidOperationException("A user with this email already exists.");

            var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            if (!emailRegex.IsMatch(userDTO.Email))
                throw new ArgumentException("Invalid email format.");

            var passwordRegex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*()_+\-={}:;'<>,.?]).{8,}$");
            if (!passwordRegex.IsMatch(userDTO.Password))
                throw new ArgumentException("Password must be at least 8 characters and include uppercase, lowercase, number, and special character.");

            var user = new User
            {
                FirstName = userDTO.FirstName,
                LastName = userDTO.LastName,
                Email = userDTO.Email,

                Role = userDTO.Role,
                CreatedAt = DateTime.UtcNow
            };

            var hasher = new PasswordHasher<User>();
            user.PasswordHash = hasher.HashPassword(user, userDTO.Password);

            await SendVerificationEmailAsync(user);

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.SaveAllAsync();

            return user.Id;
        }

        private async Task SendVerificationEmailAsync(User user)
        {
            var fromAddress = new MailAddress(_emailSettings.SenderEmail, _emailSettings.SenderName);
            var toAddress = new MailAddress(user.Email, $"{user.FirstName} {user.LastName}");

            string VerificationCode = new Random().Next(100000, 999999).ToString();
            user.EmailVerificationToken = VerificationCode;
            user.EmailVerificationTokenExpiry = DateTime.UtcNow.AddHours(1);

            string subject = "Verify your email";
            string body = $"Your verification code is: {VerificationCode}";

            var smtp = new SmtpClient
            {
                Host = _emailSettings.SmtpServer,
                Port = _emailSettings.SmtpPort,
                EnableSsl = _emailSettings.EnableSsl,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(_emailSettings.SenderEmail, _emailSettings.SenderPassword),
                Timeout = _emailSettings.TimeoutSeconds * 1000
            };

            using var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            };
            await smtp.SendMailAsync(message);
        }

        public async Task ResendVerificationEmailAsync(string email)
        {
            var user = await _unitOfWork.Users.GetByEmailAsync(email)
                ?? throw new KeyNotFoundException("User not found.");
                
            if (user.IsEmailVerified)
                throw new InvalidOperationException("Email is already verified.");

            await SendVerificationEmailAsync(user);
            _unitOfWork.Users.Update(user);
            await _unitOfWork.SaveAllAsync();
        }

        public async Task UpdateUserAsync(int id, UserDTO userUpdateDto)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id)
                ?? throw new KeyNotFoundException("User not found.");

            user.FirstName = userUpdateDto.FirstName;
            user.LastName = userUpdateDto.LastName;

            if (!string.IsNullOrEmpty(userUpdateDto.Password))
            {
                var hasher = new PasswordHasher<User>();
                user.PasswordHash = hasher.HashPassword(user, userUpdateDto.Password);
            }

            _unitOfWork.Users.Update(user);
            await _unitOfWork.SaveAllAsync();
            return;
        }

        public async Task DeleteUserAsync(int id)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id)
                ?? throw new KeyNotFoundException("User not found.");
            await _unitOfWork.Users.DeleteAsync(id);
            await _unitOfWork.SaveAllAsync();
            return;
        }

        public async Task<string?> LoginUserAsync(UserLoginDTO loginDto)
        {
            var user = await _unitOfWork.Users.GetByEmailAsync(loginDto.Email);
            if (user == null)
                return null;

            var hasher = new PasswordHasher<User>();
            var result = hasher.VerifyHashedPassword(user, user.PasswordHash, loginDto.Password);
            if (result == PasswordVerificationResult.Failed)
                return null;

            if (!user.IsEmailVerified)
                return "EmailNotVerified";

            var tokenHandler = new JwtSecurityTokenHandler();

            var claimsIdentity = new ClaimsIdentity(
                [
                    new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new (ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new (ClaimTypes.Email, user.Email),
                    new (ClaimTypes.GivenName, user.FirstName),
                    new (ClaimTypes.Role, user.Role),
                ]
            );

            var securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Expires = DateTime.Now.AddMinutes(_jwtOptions.Lifetime),
                Issuer = _jwtOptions.Issuer,
                Audience = _jwtOptions.Audience,
                SigningCredentials = new SigningCredentials(
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SigningKey)),
                        SecurityAlgorithms.HmacSha256
                    ),
                Subject = claimsIdentity
            };
            var securityToken = tokenHandler.CreateToken(securityTokenDescriptor);
            return tokenHandler.WriteToken(securityToken);
        }

        public async Task<bool> VerifyEmailAsync(EmailVerificationDTO verificationDto)
        {
            var user = await _unitOfWork.Users
                .GetByEmailAsync(verificationDto.Email);

            if (user == null || user.EmailVerificationToken != verificationDto.VerificationToken || user.EmailVerificationTokenExpiry < DateTime.UtcNow)
                return false;

            user.IsEmailVerified = true;
            user.EmailVerificationToken = null;
            user.EmailVerificationTokenExpiry = null;
            _unitOfWork.Users.Update(user);
            await _unitOfWork.SaveAllAsync();
            return true;
        }
    }
}