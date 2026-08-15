using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Business_Logic_Layer.DTO.AccountDTO;
using Business_Logic_Layer.Exceptions;
using Business_Logic_Layer.Exceptions.UserExceptions;
using Business_Logic_Layer.Service.Interface;
using Data_Access_Layer.Models;
using Data_Access_Layer.Repository.Interface;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json.Linq;

namespace Business_Logic_Layer.Service.Implementation
{
    public class RefreshTokenService(IRefreshTokenRepo _refreshToken, IUnitOfWork _unitOfWork  , IMapper _mapper) : IRefreshTokenService
    {
        public async Task<string> CreateRefreshToken(string userId)
        {
           
            var token = GenerateRefreshToken();
            var refreshToken = new RefreshToken
            {
                Token = token,
                UserId = userId,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                IsRevoked = false
            };
            await _refreshToken.Create(refreshToken);
            await _unitOfWork.SaveChangesAsync();
                return token;
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng= RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public async Task<RefreshTokenDTO?> GetByRefreshToken(string token)
        {
            var refreshToken = await _refreshToken.GetRefreshTokenByToken(token);
            if(refreshToken == null)
            {
                throw new NotFoundException("Refresh token not found");
            }
            if(refreshToken.ExpiresAt <= DateTime.UtcNow)
            {
                throw new UnauthorizedException("Refresh token has expired");
            }
            if (refreshToken.IsRevoked)
            {
                throw new UnauthorizedException("Refresh token has been revoked");
            }
            return _mapper.Map<RefreshTokenDTO>(refreshToken);
        }

        public async Task<MessageDTO> RevokeRefreshToken(string token)
        {
            var refreshToken = await _refreshToken.GetRefreshTokenByToken(token);
            if (refreshToken == null)
            {
                throw new NotFoundException("Refresh token not found");
            }
            if (refreshToken.ExpiresAt <= DateTime.UtcNow)
            {
                throw new UnauthorizedException("Refresh token has expired");
            }
            if (refreshToken.IsRevoked)
            {
                throw new UnauthorizedException("Refresh token has been revoked");
            }
            refreshToken.IsRevoked = true;
            _refreshToken.Update(refreshToken);
            await _unitOfWork.SaveChangesAsync();
            return new MessageDTO
            {
                Message = "Refresh token revoked successfully"
            };
        }


    }
}
