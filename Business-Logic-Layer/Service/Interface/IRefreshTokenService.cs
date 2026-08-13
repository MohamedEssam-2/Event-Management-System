using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business_Logic_Layer.DTO.AccountDTO;

namespace Business_Logic_Layer.Service.Interface
{
    public interface IRefreshTokenService
    {
        Task<string> CreateRefreshToken(string userId);
        Task<RefreshTokenDTO?> GetByRefreshToken(string token);
        Task<MessageDTO> RevokeRefreshToken(string token);
        string GenerateRefreshToken();
    }
}
