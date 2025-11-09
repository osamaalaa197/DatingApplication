using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DatingApplication.Core.Consts;
using DatingApplication.Core.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatingApplication.EF.Repository
{
    public class PhotoService : IPhotoService
    {
        public readonly Cloudinary _cloudinary;
        public PhotoService(IOptions<CloudinarySettings> options)
        {
            var acc=new Account(options.Value.CloudName,options.Value.ApiKey,options.Value.ApiSercret);
            _cloudinary = new Cloudinary(acc);

        }
        public async Task<ImageUploadResult> AddPhotoAsync(IFormFile file)
        {
            var uploadResult = new ImageUploadResult();
            if(file.Length>0)
            {
                using(var stream=file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams
                    {
                        File = new FileDescription(file.FileName, stream),
                        Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face")
                    };
                    uploadResult = _cloudinary.Upload(uploadParams);
                }
            }
            return uploadResult;
        }

        public async Task<DeletionResult> DeletePhotoAsync(string publicId)
        {
            var deleteParem=new DeletionParams(publicId);
            return await _cloudinary.DestroyAsync(deleteParem);
        }
    }
}
