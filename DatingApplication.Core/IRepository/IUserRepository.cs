using DatingApplication.Core.Consts;
using DatingApplication.Core.DTOs;
using DatingApplication.Core.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatingApplication.Core.IRepository
{
    public interface IUserRepository :IBaseRepository<ApplicationUser>
    {
        Task<APIResponse> LogIn(string userName, string password);
        Task<APIResponse> Register(RegisterDTO registerDTO);
        Task<APIResponse> GetUserById(string id);
        Task<APIResponse> GetAllUser(int pageNumber, int pageSize);
        Task<APIResponse> GetUserByName(string name);

        Task<APIResponse> UpdateProfile(string userId,AccountDTO accountDTO);
        Task<APIResponse> AddPhoto(string userId, IFormFile file);
        Task<APIResponse> SetMainPhoto(string userId, int photoId);

        Task<APIResponse> DeletePhoto(string userId, int photoId);


    }
}
