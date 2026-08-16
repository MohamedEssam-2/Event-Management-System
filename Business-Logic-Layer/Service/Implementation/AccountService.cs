using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Business_Logic_Layer.DTO.AccountDTO;
using Business_Logic_Layer.Exceptions;
using Business_Logic_Layer.Exceptions.UserExceptions;
using Business_Logic_Layer.Service.Interface;
using CloudinaryDotNet;
using Data_Access_Layer.Models;
using Data_Access_Layer.Repository.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;


namespace Business_Logic_Layer.Service.Implementation
{
    public class AccountService(UserManager<ApplicationUser> _userManager, IOptions<JwtOptions> _jwtoptions, IConfiguration _configuration, IEmailService _emailService, IRefreshTokenService _refreshTokenService) : IAccountService
    {
        public async Task<MessageDTO> Register(RegisterDTO registerDTO)
        {
            var user = new ApplicationUser
            {
                FullName = registerDTO.FullName,
                UserName = registerDTO.Email,
                Email = registerDTO.Email,
                Age = registerDTO.Age,
                //PhoneNumber = registerDTO.Phone_Number

            };
            var result = await _userManager.CreateAsync(user, registerDTO.Password);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                throw new BadRequestException(errors);
            }
            var roleResult = await _userManager.AddToRoleAsync(user, "Attendee");
            if (!roleResult.Succeeded)
            {
                // Rollback user creation
                await _userManager.DeleteAsync(user);
                var errors = roleResult.Errors.Select(e => e.Description).ToList();
                throw new BadRequestException(errors);
            }
            await SendConfirmationEmail(user);
            return new MessageDTO
            {
                Message = "User registered successfully. Please check your email to confirm your account."
            };
        }
        public async Task<string> CreateTokenAsync(ApplicationUser user)
        {
            var jwt = _jwtoptions.Value;
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id!),
                new Claim(ClaimTypes.Email,user.Email!),
                new Claim(ClaimTypes.Name,user.FullName!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };
            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwt.Issuer,
                audience: jwt.Audience,
                claims: claims,
                expires: DateTime.Now.AddHours(jwt.DurationInHourse),
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        

        public async Task<UserDTO> Login(LoginDTO loginDTO)
        {
            var user = await _userManager.FindByEmailAsync(loginDTO.Email);
            if (user == null)
            {
                throw new UnauthorizedException("Invalid email or password ");
            }
            var result = await _userManager.CheckPasswordAsync(user, loginDTO.Password);
            if (!result)
            {
                throw new UnauthorizedException("Invalid email or password");
            }
            if (!user.EmailConfirmed)
            {
                throw new UnauthorizedException("Please confirm your email before logging in.");
            }
            var accessToken = await CreateTokenAsync(user);

            var refreshToken = await _refreshTokenService.CreateRefreshToken(user.Id);
            return new UserDTO()
            {
                Email = user.Email!,
                DispalyName = user.FullName,
                Token = accessToken,
                RefreshToken = refreshToken
            };
        }

        public async Task ResendConfirmEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new NotFoundException("Email is required");
            }
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                throw new NotFoundException("User not found");
            }
            if (user.EmailConfirmed)
            {
                throw new Exception("Email is already confirmed");
            }
            await SendConfirmationEmail(user);
        }

        public async Task ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
                throw new Exception("UserId and token are required");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new UserNotFoundException(userId);
            }
            if (user.EmailConfirmed)
            {
                throw new Exception("Email is already confirmed");
            }
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                throw new BadRequestException(errors);
            }

        }
        private async Task SendConfirmationEmail(ApplicationUser user)
        {
            var token =
                await _userManager.GenerateEmailConfirmationTokenAsync(user);

            var baseUrl = _configuration["ServerSettings:BaseUrl"];

            var confirmationLink =
                $"{baseUrl.TrimEnd('/')}/api/account/ConfirmEmail" +
                $"?userId={user.Id}" +
                $"&token={Uri.EscapeDataString(token)}";

            var htmlMessage =
                $"<h1>Confirm your email</h1>" +
                $"<p>Please confirm your email by clicking the link below:</p>" +
                $"<a href='{HtmlEncoder.Default.Encode(confirmationLink)}'>" +
                $"Confirm Email</a>";

            await _emailService.SendEmailAsync(
                user.Email!,
                "Confirm your email",
                htmlMessage);
        }

        public async Task DeleteUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new UserNotFoundException(userId);
            }
            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                throw new BadRequestException(errors);
            }
        }

        public async Task<List<ReadUserDTO>> GetAllUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            var result = new List<ReadUserDTO>();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                result.Add(new ReadUserDTO
                {
                    UserId = user.Id!,
                    FullName = user.FullName,
                    Email = user.Email!,
                    EmailConfirmed = user.EmailConfirmed,
                    Roles = roles.ToList()
                });
            }
            if (!result.Any())
            {
                throw new Exception("No users found");
            }
            return result;
        }
    
    public async Task<UserDTO> RefreshToken(string token)
        {

            var storedToken = await _refreshTokenService.GetByRefreshToken(token);
            
            if (storedToken.ExpiresAt <= DateTime.UtcNow)
            {
                throw new UnauthorizedException(
                    "Refresh token has expired");
            }
            var user = await _userManager.FindByIdAsync(storedToken.UserId);
            if (user == null)
            {
                throw new UserNotFoundException(
                    storedToken.UserId);
            }
            if (!user.EmailConfirmed)
            {
                throw new UnauthorizedException(
                    "Please confirm your email before continuing.");
            }

            // 4. Revoke the old refresh token
            await _refreshTokenService.RevokeRefreshToken(token);

            // 5. Generate a new access token
            var accessToken = await CreateTokenAsync(user);

            // 6. Generate and save a new refresh token
            var newRefreshToken = await _refreshTokenService.CreateRefreshToken(user.Id);

            // 7. Return both tokens
            return new UserDTO
            {
                Email = user.Email!,
                DispalyName = user.FullName,
                Token = accessToken,
                RefreshToken = newRefreshToken
            };
        }

        public async Task<MessageDTO> ForgotPassword(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new NotFoundException("Email is required");
            }
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                throw new NotFoundException("User not found");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var BaseUrl = _configuration["ServerSettings:BaseUrl"];
            var resetLink =
                            $"{BaseUrl.TrimEnd('/')}/api/account/ResetPassword" +
                            $"?userId={user.Id}" +
                            $"&token={Uri.EscapeDataString(token)}";

            var htmlMessage =
                            $"<h1>Reset your password</h1>" +
                            $"<p>Click the link below to reset your password:</p>" +
                            $"<a href='{HtmlEncoder.Default.Encode(resetLink)}'>Reset Password</a>";
            await _emailService.SendEmailAsync(user.Email!, "Reset your password", htmlMessage);
            return new MessageDTO
            {
                Message = "Reset password link has been sent to your email."
            };

        }

        public async Task<MessageDTO> ResetPassword(ResetPasswordDTO dto)
        {
            var user = await _userManager.FindByIdAsync(dto.UserId);
            if (user == null)
            {
                throw new UserNotFoundException(dto.UserId);
            }
            var result = await _userManager.ResetPasswordAsync(user, dto.Token, dto.NewPassword);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                throw new BadRequestException(errors);
            }
            return new MessageDTO
            {
                Message = "Password has been reset successfully."
            };
        }
    }
}

