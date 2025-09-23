using System.Text.RegularExpressions;
using BLL.DTOs;
using BLL.Manager.Interfaces;
using DAL.Interfaces;
using DAL.Models;
using Microsoft.AspNetCore.Identity;


namespace BLL.Manager
{
    public class UserManager(IUnitOfWork unitOfWork) : IUserManager
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<IEnumerable<UserDTO>> GetAllUsersAsync()
        {
            var users = await _unitOfWork.Users.GetAllAsync();
            return users.Select(u => new UserDTO
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
                AuthMethod = u.AuthMethod,
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
                AuthMethod = user.AuthMethod,
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
                GoogleId = userDTO.GoogleId,
                AuthMethod = userDTO.AuthMethod,
                Role = userDTO.Role,
                CreatedAt = DateTime.UtcNow
            };

            var hasher = new PasswordHasher<User>();
            user.PasswordHash = hasher.HashPassword(user, userDTO.Password);

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.SaveAllAsync();

            return user.Id;
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
    }
}
