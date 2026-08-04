using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic_Layer.DTO.CloudinaryDTO
{
    public class PhotoUploadResultDTO
    {
        public string Url { get; set; } = null!;
        public string PublicId { get; set; } = null!;
    }
}
