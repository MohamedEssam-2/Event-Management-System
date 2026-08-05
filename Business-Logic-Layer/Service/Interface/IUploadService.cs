using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business_Logic_Layer.DTO.CloudinaryDTO;
using Microsoft.AspNetCore.Http;

namespace Business_Logic_Layer.Service.Interface
{
    public interface IUploadService
    {
        Task<PhotoUploadResultDTO> UploadImageAsync(IFormFile file, string folder);
        Task DeleteImageAsync(string publicId);
    }
}
