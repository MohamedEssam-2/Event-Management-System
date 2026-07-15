using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Business_Logic_Layer.DTO.ErrorDTO
{
    public class ErrorToReturn
    {
        public int StatusCode { get; set; }

        public string ErrorMessage { get; set; } = string.Empty;

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<string>? Errors { get; set; }
    }
}
