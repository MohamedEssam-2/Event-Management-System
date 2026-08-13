using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data_Access_Layer.Models;

namespace Data_Access_Layer.Repository.Interface
{
    public interface IRefreshTokenRepo
    {
        public Task<RefreshToken> GetRefreshTokenByToken(string token);
        public Task<RefreshToken> Create (RefreshToken refreshToken);
        public void Update (RefreshToken refreshToken);

    }
}
