
using System.Security.Claims;
using Business_Logic_Layer.DTO.AccountDTO;
using Business_Logic_Layer.DTO.AccountDTO.External_LoginDto;
using Business_Logic_Layer.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation_Logic_Layer.Controllers
{
    /// <summary>
    /// Handles user account operations, including authentication,
    /// registration, email confirmation, and password management.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController(IAccountService _accountService,IRefreshTokenService _refreshToken) : ControllerBase
    {
        /// <summary>
        /// Authenticates a user using their email and password.
        /// </summary>
        /// <param name="loginDto">The user's login credentials.</param>
        /// <returns>The authenticated user's information and authentication tokens.</returns>
        [HttpPost("Login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDto)
        {
            var user = await _accountService.Login(loginDto);
            return Ok(user);
        }

        /// <summary>
        /// Registers a new user account.
        /// </summary>
        /// <param name="registerDto">The registration details of the new user.</param>
        /// <returns>The newly registered user's information.</returns>
        [HttpPost("Register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDto)
        {
            var user = await _accountService.Register(registerDto);
            return Ok(user);
        }

        /// <summary>
        /// Authenticates a user using their Google account.
        /// </summary>
        /// <param name="dto">The Google authentication credentials.</param>
        /// <returns>The authenticated user's information and authentication tokens.</returns>

        [HttpPost("google")]
        [AllowAnonymous]
        public async Task<ActionResult<UserDTO>> GoogleLogin(GoogleLoginDTO dto)
        {
            var user = await _accountService.GoogleLogin(dto);
            return Ok(user);
        }

        /// <summary>
        /// Generates a new authentication session using a refresh token.
        /// </summary>
        /// <param name="token">The refresh token used to generate new authentication tokens.</param>
        /// <returns>The user's updated authentication information.</returns>
        [HttpPost("RefreshToken")]
        public async Task<ActionResult<UserDTO>> RefreshToken(string token)
        {
            var user = await _accountService.RefreshToken(token);
            return Ok(user);
        }

        /// <summary>
        /// Logs out the user by revoking their refresh token.
        /// </summary>
        /// <param name="refreshToken">The refresh token to revoke.</param>
        /// <returns>The result of the logout operation.</returns>
        [HttpPost("Logout")]
        public async Task<IActionResult> Logout(string refreshToken)
        {
            var logout = await _refreshToken.RevokeRefreshToken(refreshToken);
            return Ok(logout);
        }

        /// <summary>
        /// Resends the email confirmation message to the specified email address.
        /// </summary>
        /// <param name="email">The email address associated with the user account.</param>
        /// <returns>A message confirming that the email was resent successfully.</returns>
        [HttpPost("ResendConfirmEmail")]
        public async Task<ActionResult<MessageDTO>> ResendConfirmEmail(string email)
        {
            await _accountService.ResendConfirmEmail(email);
            return Ok(new MessageDTO
            {
                Message = "Confirmation email has been resent successfully"
            });
        }

        /// <summary>
        /// Confirms a user's email address using the confirmation token.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="token">The email confirmation token.</param>
        /// <returns>A message confirming that the email was verified successfully.</returns>
        [HttpGet("ConfirmEmail")]
        public async Task<ActionResult<MessageDTO>> ConfirmEmail(string userId, string token)
        {
            await _accountService.ConfirmEmail(userId, token);
            return Ok(new MessageDTO
            {
                Message = "Email has been confirmed successfully"
            });
        }

        /// <summary>
        /// Initiates the password recovery process by sending a password reset link.
        /// </summary>
        /// <param name="email">The email address associated with the account.</param>
        /// <returns>A message describing the result of the password recovery request.</returns>
        [HttpPost("ForgetPassword")]
        public async Task<ActionResult<MessageDTO>> ForgetPassword(string email)
        {
            var result = await _accountService.ForgotPassword(email);
            return Ok(result);
        }

        /// <summary>
        /// Resets the user's password using the provided reset information.
        /// </summary>
        /// <param name="dto">The password reset details, including the reset token.</param>
        /// <returns>A message describing the result of the password reset operation.</returns>
        [HttpPost("ResetPassword")]
        public async Task<ActionResult<MessageDTO>> ResetPassword(ResetPasswordDTO dto)
        {
            var result = await _accountService.ResetPassword(dto);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves all registered users.
        /// </summary>
        /// <returns>A list containing all registered users.</returns>
        /// <remarks>
        /// Requires Admin role authorization.
        /// </remarks>
        [HttpGet("GetAllUsers")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<UserDTO>>> GetAllUsers()
        {
            var result = await _accountService.GetAllUsers();
            return Ok(result);
        }

        /// <summary>
        /// Deletes a user account by its unique identifier.
        /// </summary>
        /// <param name="userId">The unique identifier of the user to delete.</param>
        /// <returns>A message confirming that the user was deleted successfully.</returns>
        /// <remarks>
        /// Requires Admin role authorization.
        /// </remarks>
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

        /// <summary>
        /// Retrieves the currently authenticated user's information.
        /// </summary>
        /// <returns>
        /// The current user's ID, email, full name, roles, and authentication status.
        /// </returns>
        /// <remarks>
        /// Requires a valid JWT access token.
        /// </remarks>
        [Authorize]
        [HttpGet("me")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult GetCurrentUser()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var email = User.FindFirstValue(ClaimTypes.Email);
            var name = User.FindFirstValue(ClaimTypes.Name) ?? User.FindFirstValue("name");
            var roles = User.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();

            return Ok(new
            {
                UserId = userId,
                Email = email,
                FullName = name,
                Roles = roles,
                IsAuthenticated = User.Identity?.IsAuthenticated ?? false
            });
        }
    }
}
