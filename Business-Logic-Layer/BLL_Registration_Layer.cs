using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business_Logic_Layer.DTO.AccountDTO;
using Business_Logic_Layer.Service.Implementation;
using Business_Logic_Layer.Service.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Business_Logic_Layer
{
    public static class BLL_Registration_Layer
    {
        public static IServiceCollection BLL_Registration(this IServiceCollection Services, IConfiguration _configuration)
        {
            Services.AddAutoMapper(x => { }, typeof(ServiceLayerAssemblyReference).Assembly);
            //Services.Configure<JwtOptions>(_configuration.GetSection("JwtOptions"));
            Services.AddScoped<IEventService, EventService>();
            Services.AddScoped<IAccountService, AccountService>();
            Services.AddScoped<ICategoryService, CategoryService>();
            Services.AddScoped<IRegistrationService, RegistrationService>();
            Services.AddHttpContextAccessor();
            Services.AddScoped<ICurrentUserService, CurrentUserService>();
            return Services;
        }
    }
}
