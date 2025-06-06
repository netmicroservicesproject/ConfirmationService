using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Presentation.Services;

// Confirmation of bookings-controller, copilot assisted
namespace Presentation.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class ConfirmationController : ControllerBase {
        
        // Needed for contacting bookings microservice
        private readonly IHttpClientFactory _httpClientFactory; 
        private readonly ConfirmationService _confirmationService;

        public ConfirmationController(IHttpClientFactory httpClientFactory, ConfirmationService confirmationService) {
            _httpClientFactory = httpClientFactory;
            _confirmationService = confirmationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllConfirmations() {
            var result = await _confirmationService.GetAllAsync();
            return Ok(result);
        }

        // Add bookings from booking microservice to confirmed, and delete all from bookings.
        [HttpPost]
        public async Task<IActionResult> ConfirmBookings([FromBody] ConfirmationRequest request) {
            Console.WriteLine($"Received request: Name = {request.Name}, Email = {request.Email}");


            var client = _httpClientFactory.CreateClient();

            // Fetch all bookings from the Booking api
            var bookingsResponse = await client.GetAsync("https://localhost:7235/api/booking");
            if (!bookingsResponse.IsSuccessStatusCode) return NotFound("Failed to fetch bookings.");

            var bookings = await bookingsResponse.Content.ReadFromJsonAsync<List<BookingEntity>>();
            if (bookings == null || !bookings.Any()) return NotFound("No bookings found.");

            // Transfer bookings to Confirmation database with user info
            var confirmedBookings = bookings.Select(b => new ConfirmationEntity {
                Id = Guid.NewGuid().ToString(),
                EventName = b.Name,
                Information = b.Information,
                UserName = request.Name,
                UserEmail = request.Email
            }).ToList();

            var confirmationSuccess = await _confirmationService.AddConfirmationsAsync(confirmedBookings);
            if (!confirmationSuccess) {
                return BadRequest("Failed to confirm bookings.");

            } else {
                // Delete all bookings from the Booking project's API if bookings got confirmed.
                var deletionResponse = await client.DeleteAsync("https://localhost:7235/api/booking");
                return deletionResponse.IsSuccessStatusCode
                    ? Ok("Bookings confirmed and deleted.")
                    : BadRequest("Bookings confirmed but deletion failed.");
            }



        }
    }
}