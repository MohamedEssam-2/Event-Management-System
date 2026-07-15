using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Business_Logic_Layer.DTO.AccountDTO;
using Business_Logic_Layer.Exceptions;
using Business_Logic_Layer.Service.Interface;
using Data_Access_Layer.Models;
using Data_Access_Layer.Repository.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Business_Logic_Layer.Service.Implementation
{
    public class AccountService(UserManager<ApplicationUser> _userManager , IOptions<JwtOptions> _jwtoptions) : IAccountService
    {
        public async Task<UserDTO> Register(RegisterDTO registerDTO)
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
            if (result.Succeeded)
            {
                return new UserDTO()
                {
                    Email = user.Email!,
                    DispalyName = user.FullName,
                    Token = await CreateTokenAsync(user),
                };
            }
            else
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                throw new BadRequestException(errors);
            }
        }
        public async Task<string>CreateTokenAsync(ApplicationUser user)
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
            var result =await _userManager.CheckPasswordAsync(user, loginDTO.Password);
            if (!result)
            {
                throw new UnauthorizedException("Invalid email or password");
            }
            return new UserDTO()
            {
                Email = user.Email!,
                DispalyName = user.FullName,
                Token = await CreateTokenAsync(user),
            };
        }
    }

}
