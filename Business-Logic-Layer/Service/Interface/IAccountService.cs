using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business_Logic_Layer.DTO.AccountDTO;

namespace Business_Logic_Layer.Service.Interface
{
    public interface IAccountService
    {
        public Task<UserDTO> Register(RegisterDTO registerDTO);
        //public Task<UserDTO> Login(LoginDTO loginDTO);
    }
}
