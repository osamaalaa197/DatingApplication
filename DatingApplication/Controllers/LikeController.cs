using DatingApplication.Core.Consts;
using DatingApplication.Core.IRepository;
using DatingApplication.EF.Helper;
using DatingApplication.EF.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DatingApplication.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LikeController : ControllerBase
    {
        private readonly ILikeRepository _likeRepository;

        public LikeController(ILikeRepository likeRepository)
        {
            _likeRepository=likeRepository;
        }
        [Authorize]
        [HttpPost]
        public Task<APIResponse> ToggleLike(string targetId)
        {
            var userSourceId=User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return _likeRepository.ToggleLike(userSourceId, targetId);
        }

        [HttpGet]
        [Authorize]
        public Task<APIResponse> GetLikeUser()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            LikeParams likeParams = new();
            likeParams.UserId = userId;
            return _likeRepository.GetUserLike(likeParams);
        }
    }
}
