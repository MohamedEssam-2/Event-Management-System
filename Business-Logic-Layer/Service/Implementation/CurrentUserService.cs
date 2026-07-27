using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business_Logic_Layer.Service.Interface;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Business_Logic_Layer.Service.Implementation
{
    public class CurrentUserService(IHttpContextAccessor _httpContextAccessor) : ICurrentUserService
    {
        public string? UserId => _httpContextAccessor.HttpContext?.User
                .FindFirst(ClaimTypes.NameIdentifier)?.Value;

        public string? FullName =>_httpContextAccessor.HttpContext?.User
                .FindFirst(ClaimTypes.Name)?.Value;

        public string? Email =>_httpContextAccessor.HttpContext?.User
                .FindFirst(ClaimTypes.Email)?.Value;
    }
}
