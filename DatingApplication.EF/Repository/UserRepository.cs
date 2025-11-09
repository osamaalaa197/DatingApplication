using AutoMapper;
using Azure;
using CloudinaryDotNet.Actions;
using DatingApplication.Core.Consts;
using DatingApplication.Core.DTOs;
using DatingApplication.Core.IRepository;
using DatingApplication.Core.Models;
using DatingApplication.EF.Data;
using DatingApplication.EF.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DatingApplication.EF.Repository
{
    public class UserRepository : BaseRepository<ApplicationUser> , IUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly JWTOptions _jwtOption;
        private readonly ApplicationContext _context;
        private readonly IPhotoService _photoService;

        public UserRepository(ApplicationContext context , UserManager<ApplicationUser> userManager,IMapper mapper,IOptions<JWTOptions> jwtOption,IPhotoService photoService) : base(context)
        {
            _userManager=userManager;
            _mapper=mapper;
            _jwtOption= jwtOption.Value;
            _context=context;
            _photoService=photoService;
        }

        public async Task<APIResponse> LogIn(string email, string password)
        {
            var response = new APIResponse();
            try
            {
                var user = _context.Users.Include(e => e.Photos).FirstOrDefault(e => e.Email.ToLower() == email.ToLower());
                if (user == null)
                {
                    response.Success = false;
                    response.Message = Messages.InValidAccount;
                    return response;
                }
                var isCheckPassword = await _userManager.CheckPasswordAsync(user, password);
                if (!isCheckPassword)
                {
                    response.Success = false;
                    response.Message = Messages.InValidAccount;
                    return response;
                }
                var token = GenerateToken(user);
                response.Success = true;
                response.Data = new LoginResponseDTO { Token = token, UserId = user.Id, UserName = user.UserName, PhotoUrl = user.Photos.FirstOrDefault(e => e.IsMain)?.URL };
                response.Message = Messages.LoginSuccessfully;
                return response;
            }
            catch(Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message.ToString();
                return response;
            }
            
        }

        public async Task<APIResponse> GetUserById(string id)
        {
            var response = new APIResponse();
            try
            {
                var user = await _context.Users.Include(e => e.Photos).Where(e => e.Id == id).FirstOrDefaultAsync();
                if (user is not null)
                {
                    var userDto = _mapper.Map<AccountDTO>(user);
                    userDto.PhotoUrl = user.Photos.FirstOrDefault(e => e.IsMain)?.URL;
                    response.Success = true;
                    response.Data = _mapper.Map<AccountDTO>(user);
                    return response;
                }
                response.Success = false;
                response.Message = "User not found";
                return response;
            }
            catch(Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message.ToString();
                return response;
            }
             
        }

        public async Task<APIResponse> Register(RegisterDTO registerDTO)
        {
            var response = new APIResponse();
            try
            {
                var user = _mapper.Map<ApplicationUser>(registerDTO);
                user.UserName = user.FirstName + user.LastName;
                user.DateOfBirthday = DateOnly.FromDateTime(registerDTO.DateOfBirth);
                var result = await _userManager.CreateAsync(user, registerDTO.Password);
                if (!result.Succeeded)
                {
                    var error = new StringBuilder();
                    foreach (var item in result.Errors)
                    {
                        error.AppendLine(item.Description);
                    }
                    response.Message = error.ToString();
                    response.Success = false;
                    return response;
                }
                var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id),
                new Claim(ClaimTypes.Role,"User")
            };
                await _userManager.AddClaimsAsync(user, claims);
                response.Success = true;
                response.Message = Messages.CreateAccountSuccessfully;
                return response;
            }
            catch(Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message.ToString();
                return response;
            }
            
        }

        private string GenerateToken(ApplicationUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key=Encoding.ASCII.GetBytes(_jwtOption.Secret);
            var claimList= new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id),
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.Role,"User")
            };
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Audience = _jwtOption.Audience,
                Issuer = _jwtOption.Issuer,
                Expires = DateTime.UtcNow.AddDays(60),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Subject=new ClaimsIdentity(claimList)
            };
            var token=tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

       public async Task<APIResponse> GetAllUser(int pageNumber , int pageSize)
        {
            var response = new APIResponse();
            var users = _context.ApplicationUsers.Include(e => e.Photos);
            var totalCount = await users.CountAsync();
            var accountDto = _mapper.Map<List<AccountDTO>>(users);
            var paginatedItems = accountDto
               .Skip((pageNumber - 1) * pageSize)
               .Take(pageSize)
               .ToList();
            var metadata = new PaginationHeader
            {
                TotalCount = totalCount,
                PageSize = pageSize,
                CurrentPage = pageNumber,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
            };
            response.Data = new {data=paginatedItems, PaginationMetadata = metadata};
            response.Success = true;
            return response;

        }

        public async Task<APIResponse> GetUserByName(string name)
        {
            var response = new APIResponse();
            var user = await _context.Users.Where(e => e.UserName == name).FirstOrDefaultAsync();
            if (user is not null)
            {
                response.Success = true;
                response.Data = _mapper.Map<AccountDTO>(user);
                return response;
            }
            response.Success = false;
            response.Message = "User not found";
            return response;
        }

        public async Task<APIResponse> UpdateProfile(string userId,AccountDTO accountDTO)
        {
            var response = new APIResponse();
            try
            {
                var user = await _context.Users.Where(e => e.Id == userId).FirstOrDefaultAsync();
                if (user is not null)
                {
                    var userUpdate = _mapper.Map(accountDTO, user);
                    _context.Users.Update(userUpdate);
                    await _context.SaveChangesAsync();
                    response.Message = "Profile Update Successfully";
                    response.Success = true;
                    return response;

                }

                response.Success = false;
                response.Message = "User not found";
                return response;
            }
            catch(Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message.ToString();
                return response;
            }
           
        }


        public async Task<APIResponse> AddPhoto(string userId,IFormFile file)
        {
            var response = new APIResponse();
            try
            {
                var user = await _context.Users.Where(e => e.Id == userId).FirstOrDefaultAsync();
                if (user is not null)
                {
                    var result = await _photoService.AddPhotoAsync(file);
                    if (result.Error is not null)
                    {
                        response.Success = false;
                        response.Message = result.Error.ToString();
                        return response;
                    }
                    var photo = new Photo()
                    {
                        URL = result.SecureUrl.AbsoluteUri,
                        PublicId = result.PublicId
                    };
                    photo.UserId = userId;
                    if (user.Photos.Count() == 0)
                        photo.IsMain = true;
                    user.Photos.Add(photo);
                    if (await _context.SaveChangesAsync() > 0)
                    {
                        response.Success = true;
                        response.Message = "Photo added successfully";
                        response.Data = _mapper.Map<PhotoDto>(photo);
                        return response;
                    }
                    response.Success = false;
                    response.Message = "Photo not added";
                    return response;
                }
                response.Success = false;
                response.Message = "User not found";
                return response;

            }
            catch(Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message.ToString();
                return response;
            }
            
        }

        public async Task <APIResponse> SetMainPhoto(string userId,int photoId)
        {
            var response=new APIResponse();
            var user = _context.Users.Include(e=>e.Photos).FirstOrDefault(e => e.Id == userId);
            if (user is null)
            {
                response.Success = false;
                response.Message = "User not found";
                return response;
            }
            var photo = user.Photos.FirstOrDefault(e => e.Id == photoId);
            if (photo is null || photo.IsMain)
            {
                response.Success = false;
                response.Message = "Cannot use this as main photo";
                return response;
            }
            var cuurentMainPhoto = user.Photos.FirstOrDefault(e => e.IsMain);
            if (cuurentMainPhoto is not null)
                cuurentMainPhoto.IsMain = false;
            photo.IsMain = true;
            if (await _context.SaveChangesAsync()>0)
            {
                response.Success = true;
                response.Message = "Photo set main successfully";
                return response;
            }
            response.Success = false;
            response.Message = "something went wrong";
            return response;
        }

        public async Task <APIResponse> DeletePhoto(string userId, int photoId)
        {
            var response = new APIResponse();
            var user = _context.Users.Include(e => e.Photos).FirstOrDefault(e => e.Id == userId);
            if (user is null)
            {
                response.Success = false;
                response.Message = "User not found";
                return response;
            }
            var photo = user.Photos.FirstOrDefault(e => e.Id == photoId);
            if (photo is null || photo.IsMain)
            {
                response.Success = false;
                response.Message = "Cannot delete main photo";
                return response;
            }
            if (photo.PublicId is null)
            {

                response.Success = false;
                response.Message = "Cannot delete this photo";
                return response;

            }
            var result = await _photoService.DeletePhotoAsync(photo.PublicId);
            if (result.Error is not null)
            {
                response.Success = false;
                response.Message = result.Error.Message;
                return response;
            }
            user.Photos.Remove(photo);
            if (await _context.SaveChangesAsync()>0)
            {
                response.Success = true;
                response.Message = "Photo deleted succesffully";
                return response;
            }
            response.Success = true;
            response.Message = "something went wrong";
            return response;
        }
    }
}
