using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business_Logic_Layer.DTO.RegistrationPdfDto;

namespace Business_Logic_Layer.Service.Interface
{
    public interface IRegistrationPdfService
    {
        Task<byte[]> GenerateRegistrationPdfAsync(RegistrationPdfDto dto);
    }
}
