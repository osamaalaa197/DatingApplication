using AutoMapper;
using DatingApplication.Core;
using DatingApplication.Core.Consts;
using DatingApplication.Core.DTOs;
using DatingApplication.Core.IRepository;
using DatingApplication.Core.Models;
using DatingApplication.EF.Data;
using DatingApplication.EF.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DatingApplication.EF.Repository
{
    public class LikeRepository : ILikeRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly APIResponse _response;
        private readonly IMapper _mapper;

        public LikeRepository(IUnitOfWork unitOfWork,IMapper mapper) 
        {
            _unitOfWork= unitOfWork;
            _mapper=mapper;
            _response= new APIResponse();
        }
        public async Task<APIResponse> ToggleLike(string sourceUserId,string targetId)
        {
            //var response=new APIResponse();
            if (string.IsNullOrWhiteSpace(targetId) || string.IsNullOrWhiteSpace(sourceUserId))
            {
                _response.Success = false;
                _response.Message = "User not Valid";
                return _response;
            }
            if (sourceUserId==targetId)
            {
                _response.Success = false;
                _response.Message = "You cannot like yourself";
                return _response;
            }
            var existingLike = _unitOfWork.UserLike.FindAll(e => e.SourceUserId == sourceUserId && e.TargetUserId==targetId).FirstOrDefault();
            if (existingLike == null)
            {
                var userLike = new UserLike
                {
                    SourceUserId = sourceUserId,
                    TargetUserId = targetId
                };
                _unitOfWork.UserLike.Add(userLike);
                _unitOfWork.Complete();
                _response.Success = true; ;
                _response.Message = "User Added Successfully";
                return _response;
            }
            else
            {
                _unitOfWork.UserLike.Remove(existingLike);
                _unitOfWork.Complete();
                _response.Success = true; ;
                _response.Message = "User Removed Successfully";
                return _response;

            }
        }

        public async Task<APIResponse> GetUserLike(LikeParams likeParams)
        {
            if (string.IsNullOrWhiteSpace(likeParams.UserId))
            {
                _response.Success = false;
                _response.Message = "User not Valid";
                return _response;
            }
            var userLike = _unitOfWork.UserLike.FindAllWithInclude(e => e.SourceUserId == likeParams.UserId, e => e.TargetUser).Select(e=>e.TargetUser);
            if (userLike.Any())
            {
                var data = _mapper.Map<IQueryable<AccountDTO>>(userLike);
                _response.Success = true;
                _response.Data = PagedList<AccountDTO>.CreateAsync(data,likeParams.PageNumber,likeParams.PageSize);
                return _response;
            }
            _response.Success = false;
            _response.Message = "No data found";
            return _response;
        }
    }
}
