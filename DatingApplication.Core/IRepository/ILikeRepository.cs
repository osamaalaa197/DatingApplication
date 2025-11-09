using DatingApplication.Core.Consts;
using DatingApplication.EF.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatingApplication.Core.IRepository
{
    public interface ILikeRepository
    {
        Task<APIResponse> ToggleLike (string sourceUserId, string targetId);
        Task<APIResponse> GetUserLike(LikeParams likesParam);
    }
}
