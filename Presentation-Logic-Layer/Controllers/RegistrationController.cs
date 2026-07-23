using Business_Logic_Layer.DTO.RegistrationDTO;
using Business_Logic_Layer.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;

namespace Presentation_Logic_Layer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegistrationController(IRegistrationService _registrationService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllRegistrations()
        {
            var registrations = await _registrationService.GetAllRegistration();
            return Ok();
        }
        [HttpPost]
        public async Task<IActionResult> CreateRegistration([FromForm] RegistrationDTO registrationDTO)
        {
            var createdRegistration = await _registrationService.CreateRegistration(registrationDTO);
            return Ok(createdRegistration);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetRegistrationById(int id)
        {
            var registration = await _registrationService.GetRegistrationById(id);
            return Ok(registration);
        }
        [HttpPatch("{id:int}")]
        public async Task<IActionResult> UpdateRegistration(int id, [FromForm] UpdateRegistrationDTO registrationDTO)
        {
            var updated = await _registrationService.UpdateRegistration(id, registrationDTO);
            return Ok(updated);
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteRegistration(int id)
        {
            var deleted = await _registrationService.DeleteRegistration(id);
            return Ok(deleted);
        }


    }
}
