using BLL.DTOs;

namespace BLL.Manager.Interfaces
{
    public interface IUserManager
    {
        Task<IEnumerable<UserDTO>> GetAllUsersAsync();
        Task<UserDTO?> GetUserByIdAsync(int id);
        Task<int> CreateUserAsync(UserDTO userDTO);
        Task UpdateUserAsync(int id, UserDTO userUpdateDto);
        Task DeleteUserAsync(int id);
    }
}
