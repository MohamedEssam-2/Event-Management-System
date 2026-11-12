
using Business_Logic_Layer;
using Data_Access_Layer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Presentation_Logic_Layer.Controllers
{
    /// <summary>
    /// Handles application role management operations, including
    /// retrieving, creating, assigning, unassigning, and deleting roles.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController(UserManager<ApplicationUser> _userManager,RoleManager<IdentityRole> _roleManager) : ControllerBase
    {
        /// <summary>
        /// Retrieves all roles registered in the application.
        /// </summary>
        /// <returns>A list of all available role names.</returns>
        /// <remarks>
        /// Requires Admin role authorization.
        /// </remarks>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<string?>>> GetAll()
        {
            var roles = await _roleManager.Roles.Select(e => e.Name).ToListAsync();
            return Ok(roles);
        }

        /// <summary>
        /// Creates a new application role.
        /// </summary>
        /// <param name="roleName">The name of the role to create.</param>
        /// <returns>A response indicating whether the role was created successfully.</returns>
        /// <remarks>
        /// Requires Admin role authorization.
        /// Returns HTTP 400 if the role already exists.
        /// </remarks>
        [HttpPost("{roleName:alpha}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<bool>>> Create(string roleName)
        {
            var roleExists = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExists)
            {
                var result = await _roleManager.CreateAsync(new IdentityRole(roleName));
                return Ok(new ServiceResponse<bool>
                {
                    Success = true,
                    Message = "Role Created Successfuly"
                });
            }
            else
            {
                return BadRequest(new ServiceResponse<bool>
                {
                    Success = false,
                    Message = "The Role Already Exists."
                });
            }
        }

        /// <summary>
        /// Assigns an existing role to a specific user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="roleName">The name of the role to assign.</param>
        /// <returns>A response indicating whether the role was assigned successfully.</returns>
        /// <remarks>
        /// Requires Admin role authorization.
        /// Returns HTTP 400 if the user does not exist.
        /// Returns HTTP 404 if the specified role does not exist.
        /// </remarks>
        [HttpPost("AssignRole/{userId}/{roleName:alpha}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<bool>>> AssignRole(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return BadRequest(new ServiceResponse<bool>
                {
                    Success = false,
                    Message = $"User With Id = {userId} Is Not Found ."
                });
            }

            var roleExists = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExists)
            {
                return NotFound(new ServiceResponse<bool>
                {
                    Success = false,
                    Message = $"Cannot Find Role With This Name = {roleName}"
                });
            }

            var result = await _userManager.AddToRoleAsync(user, roleName);
            if (result.Succeeded)
            {
                return Ok(new ServiceResponse<bool>
                {
                    Success = true,
                    Message = $"Role '{roleName}' Assigned to user '{user.UserName}' Successfully."
                });
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

        /// <summary>
        /// Removes a specific role from a user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="roleName">The name of the role to remove from the user.</param>
        /// <returns>A response indicating whether the role was removed successfully.</returns>
        /// <remarks>
        /// Requires Admin role authorization.
        /// Returns HTTP 400 if the user or role does not exist.
        /// </remarks>
        [HttpDelete("UnAssignRole/{userId}/{roleName:alpha}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<bool>>> UnAssignRole(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return BadRequest(new ServiceResponse<bool>
                {
                    Success = false,
                    Message = "User Not Found"
                });
            }

            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                return BadRequest(new ServiceResponse<bool>
                {
                    Success = false,
                    Message = "Role Not Found"
                });
            }

            var result = await _userManager.RemoveFromRoleAsync(user, roleName);
            if (result.Succeeded)
            {
                return Ok(new ServiceResponse<bool>
                {
                    Success = true,
                    Message = "All Roles Removed Successfuly"
                });
            }

            return BadRequest(new ServiceResponse<bool>
            {
                Success = false,
                Message = "Failed To Remove All Roles"
            });
        }

        /// <summary>
        /// Deletes an existing role from the application.
        /// </summary>
        /// <param name="roleName">The name of the role to delete.</param>
        /// <returns>A response indicating whether the role was deleted successfully.</returns>
        /// <remarks>
        /// Requires Admin role authorization.
        /// All users assigned to this role will have the role removed before deletion.
        /// Returns HTTP 400 if the role does not exist or an operation fails.
        /// </remarks>
        [HttpDelete("{roleName:alpha}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<bool>>> Delete(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                return BadRequest(new ServiceResponse<bool>
                {
                    Success = false,
                    Message = "Role Not Found."
                });
            }

            var users = await _userManager.GetUsersInRoleAsync(roleName);
            if (users != null)
            {
                foreach (var user in users)
                {
                    var result = await _userManager.RemoveFromRoleAsync(user, roleName);
                    if (!result.Succeeded)
                    {
                        return BadRequest(new ServiceResponse<bool>
                        {
                            Success = false,
                            Message = $"Failed to remove role from user {user.UserName}."
                        });
                    }
                }
            }

            var restult = await _roleManager.DeleteAsync(role);
            if (restult.Succeeded)
                return Ok(new ServiceResponse<bool>
                {
                    Success = true,
                    Message = "Role Removed Successfuly"
                });

            return BadRequest(new ServiceResponse<bool>
            {
                Success = false,
                Message = "Remove the role is failed."
            });
        }
    }
}
