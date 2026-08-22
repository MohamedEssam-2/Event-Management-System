using Business_Logic_Layer;
using Data_Access_Layer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Presentation_Logic_Layer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController(UserManager<ApplicationUser> _userManager, RoleManager<IdentityRole> _roleManager) : ControllerBase
    {

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var roles = await _roleManager.Roles.Select(e => e.Name).ToListAsync();
            return Ok(roles);
        }


        [HttpPost("{roleName:alpha}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(string roleName)
        {
            var roleExists = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExists)
            {
                var result = await _roleManager.CreateAsync(new IdentityRole(roleName));
                return Ok(new ServiceResponse<bool> { Success = true, Message = "Role Created Successfuly" });
            }
            else
            {
                return BadRequest(new ServiceResponse<bool> { Success = false, Message = "The Role Already Exists." });
            }
        }

        [HttpPost("AssignRole/{userId}/{roleName:alpha}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AssignRole(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return BadRequest(new ServiceResponse<bool> { Success = false, Message = $"User With Id = {userId} Is Not Found ." });
            }
            var roleExists = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExists)
            {
                return NotFound(new ServiceResponse<bool> { Success = false, Message = $"Cannot Find Role With This Name = {roleName}" });
            }
            var result = await _userManager.AddToRoleAsync(user, roleName);
            if (result.Succeeded)
            {
                return Ok(new ServiceResponse<bool> { Success = true, Message = $"Role '{roleName}' Assigned to user '{user.UserName}' Successfully." });

            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

        [HttpDelete("UnAssignRole/{userId}/{roleName:alpha}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UnAssignRole(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return BadRequest(new ServiceResponse<bool> { Success = false, Message = "User Not Found" });
            }
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                return BadRequest(new ServiceResponse<bool> { Success = false, Message = "Role Not Found" });
            }
            var result = await _userManager.RemoveFromRoleAsync(user, roleName);
            if (result.Succeeded)
            {
                return Ok(new ServiceResponse<bool> { Success = true, Message = "All Roles Removed Successfuly" });
            }
            return BadRequest(new ServiceResponse<bool> { Success = false, Message = "Failed To Remove All Roles" });

        }



        [HttpDelete("{roleName:alpha}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                return BadRequest(new ServiceResponse<bool> { Success = false, Message = "Role Not Found." });
            }
            var users = await _userManager.GetUsersInRoleAsync(roleName);
            if (users != null)
            {
                foreach (var user in users)
                {
                    var result = await _userManager.RemoveFromRoleAsync(user, roleName);
                    if (!result.Succeeded)
                    {
                        return BadRequest(new ServiceResponse<bool> { Success = false, Message = $"Failed to remove role from user {user.UserName}." });
                    }
                }
            }
            var restult = await _roleManager.DeleteAsync(role);
            if (restult.Succeeded)
                return Ok(new ServiceResponse<bool> { Success = true, Message = "Role Removed Successfuly" });

            return BadRequest(new ServiceResponse<bool> { Success = false, Message = "Remove the role is failed." });
        }


    }


}
