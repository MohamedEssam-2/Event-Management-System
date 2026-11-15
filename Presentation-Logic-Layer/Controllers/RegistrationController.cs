
using Business_Logic_Layer.DTO.PaginationDTO;
using Business_Logic_Layer;
using Business_Logic_Layer.DTO.RegistrationDTO;
using Business_Logic_Layer.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation_Logic_Layer.Controllers
{
    /// <summary>
    /// Handles event registration operations, including creation,
    /// retrieval, deletion, and PDF export.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class RegistrationController(IRegistrationService _registrationService) : ControllerBase
    {
        /// <summary>
        /// Retrieves all registrations with pagination and sorting support.
        /// </summary>
        /// <param name="PageIndex">The page number to retrieve. Defaults to 1.</param>
        /// <param name="PageSize">The number of registrations per page. Defaults to 5.</param>
        /// <param name="sortBy">Optional field used to sort the registrations.</param>
        /// <returns>A paginated list of all registrations.</returns>
        /// <remarks>
        /// Requires Admin or Organizer role authorization.
        /// </remarks>
        [HttpGet]
        [Authorize(Roles = "Admin,Organizer")]
        public async Task<ActionResult<ServiceResponse<PagedResultDTO<ReadAllRegistrationDTO>>>> GetAllRegistrations(
            [FromQuery] int PageIndex = 1,
            [FromQuery] int PageSize = 5,
            [FromQuery] string? sortBy = null)
        {
            var registrations = await _registrationService.GetAllRegistration(PageIndex, PageSize, sortBy);
            return Ok(registrations);
        }

        /// <summary>
        /// Creates a new registration for an event.
        /// </summary>
        /// <param name="registrationDTO">The registration details submitted through form data.</param>
        /// <returns>The unique identifier of the newly created registration.</returns>
        /// <remarks>
        /// Requires Admin or Attendee role authorization.
        /// Accepts multipart/form-data.
        /// </remarks>
        [HttpPost]
        [Authorize(Roles = "Admin,Attendee")]
        public async Task<ActionResult<ServiceResponse<int>>> CreateRegistration(
            [FromForm] RegistrationDTO registrationDTO)
        {
            var createdRegistration = await _registrationService.CreateRegistration(registrationDTO);
            return Ok(createdRegistration);
        }

        /// <summary>
        /// Retrieves all registrations associated with the currently authenticated user.
        /// </summary>
        /// <returns>A list of registrations belonging to the current user.</returns>
        /// <remarks>
        /// Requires Admin or Attendee role authorization.
        /// </remarks>
        [HttpGet("MyRegistrations")]
        [Authorize(Roles = "Admin,Attendee")]
        public async Task<ActionResult<ServiceResponse<List<ReadAllRegistrationDTO>>>> GetMyRegistrations()
        {
            var myRegistrations = await _registrationService.GetMyRegistration();
            return Ok(myRegistrations);
        }

        /// <summary>
        /// Retrieves a specific registration by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the registration.</param>
        /// <returns>The requested registration information.</returns>
        /// <remarks>
        /// Requires Admin, Organizer, or Attendee role authorization.
        /// </remarks>
        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin,Organizer,Attendee")]
        public async Task<ActionResult<ServiceResponse<ReadAllRegistrationDTO>>> GetRegistrationById(int id)
        {
            var registration = await _registrationService.GetRegistrationById(id);
            return Ok(registration);
        }

        /// <summary>
        /// Retrieves all registrations associated with a specific event,
        /// with pagination support.
        /// </summary>
        /// <param name="eventId">The unique identifier of the event.</param>
        /// <param name="PageIndex">The page number to retrieve. Defaults to 1.</param>
        /// <param name="PageSize">The number of registrations per page. Defaults to 5.</param>
        /// <returns>A paginated list of registrations for the specified event.</returns>
        /// <remarks>
        /// Requires Admin or Organizer role authorization.
        /// </remarks>
        [HttpGet("Event/{eventId:int}")]
        [Authorize(Roles = "Admin,Organizer")]
        public async Task<ActionResult<ServiceResponse<PagedResultDTO<ReadAllRegistrationDTO>>>> GetRegistrationsByEventId(
            int eventId,
            [FromQuery] int PageIndex = 1,
            [FromQuery] int PageSize = 5)
        {
            var registrations = await _registrationService.GetRegistrationsByEventId(
                eventId, PageIndex, PageSize);

            return Ok(registrations);
        }

        /// <summary>
        /// Deletes an existing registration.
        /// </summary>
        /// <param name="id">The unique identifier of the registration to delete.</param>
        /// <returns>True if the registration was deleted successfully; otherwise, false.</returns>
        /// <remarks>
        /// Requires Admin or Attendee role authorization.
        /// </remarks>
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin,Attendee")]
        public async Task<ActionResult<ServiceResponse<bool>>> DeleteRegistration(int id)
        {
            var deleted = await _registrationService.DeleteRegistration(id);
            return Ok(deleted);
        }

        /// <summary>
        /// Exports a registration as a PDF document.
        /// </summary>
        /// <param name="id">The unique identifier of the registration to export.</param>
        /// <returns>A downloadable PDF file containing the registration details.</returns>
        /// <remarks>
        /// Requires Attendee or Admin role authorization.
        /// Returns HTTP 404 if the registration does not exist.
        /// Returns HTTP 403 if the user is not authorized to access the registration.
        /// </remarks>
        [HttpGet("{id:int}/export-pdf")]
        [Authorize(Roles = "Attendee,Admin")]
        public async Task<IActionResult> ExportRegistrationAsPdf(int id)
        {
            try
            {
                var pdfBytes = await _registrationService.ExportRegistrationAsPdfAsync(id);
                var fileName = $"registration_{id}.pdf";

                return File(pdfBytes, "application/pdf", fileName);
            }
            catch (Business_Logic_Layer.Exceptions.RegistrationExceptions.RegistrationNotFoundException)
            {
                return NotFound(new { Message = "Registration not found." });
            }
            catch (Business_Logic_Layer.Exceptions.UnauthorizedException)
            {
                return Forbid();
            }
        }
    }
}
