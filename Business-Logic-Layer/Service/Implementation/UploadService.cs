using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business_Logic_Layer.DTO.CloudinaryDTO;
using Business_Logic_Layer.Service.Interface;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Business_Logic_Layer.Service.Implementation
{
    public class UploadService : IUploadService
    {
        private readonly Cloudinary _cloudinary;
        public UploadService(IOptions<CloudinarySettingsDTO> config)
        {
            var account = new Account(
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret
            );
            _cloudinary = new Cloudinary(account);
        }
        public async Task<PhotoUploadResultDTO> UploadImageAsync(IFormFile file, string folder)
        {
            if (file == null || file.Length == 0)
                throw new Exception("Image is required.");


            if (!file.ContentType.StartsWith("image/"))
                throw new Exception("Invalid file type.");

            await using var stream = file.OpenReadStream();

            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Folder = folder
            };
            var result = await _cloudinary.UploadAsync(uploadParams);

            if (result.Error != null)
                throw new Exception(result.Error.Message);

            return new PhotoUploadResultDTO
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };
        }
    }
}
