using BLL.DTOs;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Manager.Interfaces
{
    public interface IUserRequestsManager
    {
        Task<IEnumerable<UserRequestDto>> GetRequestsAsync();
        Task<UserRequestDto> CreateRequestAsync(int userId, CreateUserRequestDto dto);
        Task<bool> UpdateRequestStatusAsync(int id,UpdateRequestStatusDto dto);
        Task<bool> DeleteRequestAsync(int id);
    }
}
