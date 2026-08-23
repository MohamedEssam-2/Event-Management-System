using Business_Logic_Layer.DTO.RegistrationDTO;
using Business_Logic_Layer.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;

namespace Presentation_Logic_Layer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegistrationController(IRegistrationService _registrationService) : ControllerBase
    {
        [HttpGet]
        [Authorize(Roles = "Admin,Organizer")]
        public async Task<IActionResult> GetAllRegistrations([FromQuery] int PageIndex=1,[FromQuery] int PageSize=5, [FromQuery] string? sortBy=null)
        {
            var registrations = await _registrationService.GetAllRegistration(PageIndex,PageSize,sortBy);
            return Ok(registrations);
        }
        [HttpPost]
        [Authorize(Roles = "Admin,Attendee")]
        public async Task<IActionResult> CreateRegistration([FromForm] RegistrationDTO registrationDTO)
        {
            var createdRegistration = await _registrationService.CreateRegistration(registrationDTO);
            return Ok(createdRegistration);
        }
        [HttpGet("MyRegistrations")]
        [Authorize(Roles = "Admin,Attendee")]
        public async Task<IActionResult> GetMyRegistrations()
        {
            var myRegistrations = await _registrationService.GetMyRegistration();
            return Ok(myRegistrations);
        }
        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin,Organizer,Attendee")]
        public async Task<IActionResult> GetRegistrationById(int id)
        {
            var registration = await _registrationService.GetRegistrationById(id);
            return Ok(registration);
        }
        [HttpGet("Event/{eventId:int}")]
        [Authorize(Roles = "Admin,Organizer")]
        public async Task<IActionResult> GetRegistrationsByEventId(int eventId, [FromQuery] int PageIndex = 1, [FromQuery] int PageSize = 5)
        {
            var registrations = await _registrationService.GetRegistrationsByEventId(eventId, PageIndex , PageSize);
            return Ok(registrations);
        }
        //[HttpPatch("{id:int}")]
        //[Authorize(Roles = "Admin,Attendee")]
        //public async Task<IActionResult> UpdateRegistration(int id, [FromForm] UpdateRegistrationDTO registrationDTO)
        //{
        //    var updated = await _registrationService.UpdateRegistration(id, registrationDTO);
        //    return Ok(updated);
        //}
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin,Attendee")]
        public async Task<IActionResult> DeleteRegistration(int id)
        {
            var deleted = await _registrationService.DeleteRegistration(id);
            return Ok(deleted);
        }


    }
}
