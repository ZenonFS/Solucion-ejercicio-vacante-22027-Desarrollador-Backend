using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TravelAgencyApp.Entities;
using TravelAgencyApp.Models;
using TravelAgencyApp.Repositories.Bookings;
using TravelAgencyApp.Repositories.Collections;
using TravelAgencyApp.Utils;

namespace TravelAgencyApp.Controllers
{
    [Route("api/bookings")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private IBookingsCollection db = new BookingsCollection();
        private readonly ILogger<BookingsCollection> _logger;

        public BookingsController(ILogger<BookingsCollection> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetList()
        {
            List<Booking> json = await db.List();

            return Ok(json);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> DetailsById(string id)
        {
            Booking book = await db.DetalisById(id);

            return Ok(book);
        }

        [HttpPost]
        public async Task<IActionResult> Book([FromBody] Booking booking)
        {
            
            if (booking == null)
            {
                return BadRequest();
            }

            Booking createdBooking = await db.Book(booking);
            new Functions().SendEmail("santiago.suarez6135@gmail.com", "Reserva de habitaciones realizada con éxito.", "Para más información dirigirse al portal.");
            return Created("Asignado con Éxito", createdBooking);
        }

    }
}
