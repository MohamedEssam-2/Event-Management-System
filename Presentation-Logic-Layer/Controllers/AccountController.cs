using Business_Logic_Layer.DTO.AccountDTO;
using Business_Logic_Layer.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Presentation_Logic_Layer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController(IAccountService _accountService) : ControllerBase
    {
        [HttpPost("Login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDto)
        {
            var user = await _accountService.Login(loginDto);
            return Ok(user);
        }
        [HttpPost("Register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDto)
        {
            var user = await _accountService.Register(registerDto);
            return Ok(user);

        }
    }
}
