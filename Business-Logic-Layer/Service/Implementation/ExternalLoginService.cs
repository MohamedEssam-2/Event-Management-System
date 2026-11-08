using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business_Logic_Layer.DTO.AccountDTO.External_LoginDto;
using Business_Logic_Layer.Service.Interface;
using Google.Apis.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;


namespace Business_Logic_Layer.Service.Implementation
{
    internal class ExternalLoginService(IConfiguration _configuration, ILogger<ExternalLoginService> _logger) : IExternalLoginService
    {
        public async Task<ExternalUserInfoDTO?> ValidateGoogleTokenAsync(string idToken)
        {
            try
            {
                var clientId = _configuration["Authentication:Google:ClientId"]
                    ?? throw new InvalidOperationException("Google ClientId is not configured");

                var settings = new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = new[] { clientId }
                };

                var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);

                return new ExternalUserInfoDTO
                {
                    Email = payload.Email,
                    EmailVerified = payload.EmailVerified,
                    FullName = payload.Name ?? payload.Email,
                    ProviderId = payload.Subject
                };
            }
            catch (InvalidJwtException ex)
            {
                _logger.LogWarning(ex, "Invalid Google ID token");
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating Google token");
                return null;
            }
        }
    }
}
