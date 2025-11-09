using DatingApplication.Core.Consts;
using DatingApplication.Core.DTOs;
using DatingApplication.Core.IRepository;
using DatingApplication.EF.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DatingApplication.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        public AccountController(IUserRepository userRepository) 
        {
            _userRepository=userRepository;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<APIResponse> Register(RegisterDTO registerDTO) 
        {
           return await _userRepository.Register(registerDTO);
        }

        [HttpPost]
        [Route("LogIn")]
        public async Task<APIResponse> LogIn(LoginDto loginDto)
        {
            return await _userRepository.LogIn(loginDto.Gmail, loginDto.Password);
        }
        [HttpGet]
        [Route("List")]
        public async Task<APIResponse> UserList(int pageNumber = 1, int pageSize = 10)
        {
            return await _userRepository.GetAllUser(pageNumber,pageSize);
        }
        [HttpGet]
        [Route("GetUserById/{id}")]
        [Authorize]
        public Task<APIResponse> GetUserById(string id)
        {
            return _userRepository.GetUserById(id);
        }
        [HttpGet]
        [Route("Profile")]
        [Authorize]
        public Task<APIResponse> Profile()
        {
            var userId = User.Claims.Where(e=>e.Type==ClaimTypes.NameIdentifier).FirstOrDefault().Value.ToString();
            return _userRepository.GetUserById(userId);
        }
        [HttpGet]
        [Route("GetUserByUserName")]
        [Authorize]
        public async Task<APIResponse> GetUserByUserName(string userName)
        {
            return await _userRepository.GetUserByName(userName);

        }

        [HttpPost]
        [Route("UpdateProfile")]
        [Authorize]
        public async Task<APIResponse> UpdateProfile(AccountDTO accountDTO)
        {
            var userId = User.Claims.Where(e => e.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value.ToString();
            return await _userRepository.UpdateProfile(userId,accountDTO);

        }

        [HttpPost]
        [Route("AddProfilePhoto")]
        [Authorize]
        public async Task<APIResponse> AddPhoto(IFormFile file)
        {
            var userId = User.Claims.Where(e => e.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value.ToString();
            return await _userRepository.AddPhoto(userId, file);

        }

        [HttpPut]
        [Route("SetPhotoMain/{photoId}")]
        [Authorize]
        public Task<APIResponse> SetPhotoMain(int photoId)
        {
            var userId = User.Claims.Where(e => e.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value.ToString();
            return _userRepository.SetMainPhoto(userId, photoId);
        }
        [HttpDelete]
        [Route("DeletePhoto/{photoId}")]
        [Authorize]
        public Task<APIResponse> DeletePhoto(int photoId)
        {
            var userId = User.Claims.Where(e => e.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value.ToString();
            return _userRepository.DeletePhoto(userId, photoId);
        }
    }
}
