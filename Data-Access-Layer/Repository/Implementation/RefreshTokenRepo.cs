using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data_Access_Layer.Database;
using Data_Access_Layer.Repository.Interface;
using Data_Access_Layer.Models;
using Microsoft.EntityFrameworkCore;

namespace Data_Access_Layer.Repository.Implementation
{
    public class RefreshTokenRepo(EventContext eventContext) : IRefreshTokenRepo
    {
        public async Task<RefreshToken> Create(RefreshToken refreshToken)
        {
            await eventContext.RefreshTokens.AddAsync(refreshToken);
            return refreshToken;
        }

        public async Task<RefreshToken?> GetRefreshTokenByToken(string token)
        {
            return await eventContext.RefreshTokens.Include(u=>u.User).FirstOrDefaultAsync(x => x.Token == token);
        }

        public void Update(RefreshToken refreshToken)
        {
             eventContext.RefreshTokens.Update(refreshToken);  
        }
    }
}
