using Business_Logic_Layer.DTO.AccountDTO;
using Business_Logic_Layer.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation_Logic_Layer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
        [HttpPost("RefreshToken")]
        public async Task<ActionResult<UserDTO>> RefreshToken(string token)
        {
            var user = await _accountService.RefreshToken(token);
            return Ok(user);
        }

        [HttpPost("ResendConfirmEmail")]
        public async Task<ActionResult<MessageDTO>> ResendConfirmEmail(string email)
        {
            await _accountService.ResendConfirmEmail(email);
            return Ok(new MessageDTO
            {
                Message = "Confirmation email has been resent successfully"
            });
        }
        [HttpGet("ConfirmEmail")]
        public async Task<ActionResult<MessageDTO>> ConfirmEmail(string userId, string token)
        {
            await _accountService.ConfirmEmail(userId, token);
            return Ok(new MessageDTO
            {
                Message = "Email has been confirmed successfully"
            });
        }

        [HttpDelete("DeleteUser")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<MessageDTO>> DeleteUser(string userId)
        {
            await _accountService.DeleteUser(userId);
            return Ok(new MessageDTO
            {
                Message = "User has been deleted successfully"
            });
        }

        [HttpGet("GetAllUsers")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<UserDTO>>> GetAllUsers()
        {
            var result = await _accountService.GetAllUsers();
            return Ok(result);
          
        }


    }
}
