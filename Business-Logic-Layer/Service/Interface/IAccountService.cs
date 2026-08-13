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
        public Task<MessageDTO> Register(RegisterDTO registerDTO);
        public Task<UserDTO> Login(LoginDTO loginDTO);
        public Task ResendConfirmEmail(string email);
        public Task ConfirmEmail(string userId, string token);
        public Task DeleteUser(string userId);
        public Task<List<ReadUserDTO>> GetAllUsers();
        public Task<UserDTO> RefreshToken(string token);

    }
}
