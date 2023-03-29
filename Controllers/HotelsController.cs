using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System.Globalization;
using TravelAgencyApp.Models;
using TravelAgencyApp.Repositories.Collections;

namespace TravelAgencyApp.Controllers
{
    [Route("api/hotels")]
    [ApiController]
    public class HotelsController : ControllerBase
    {
        private IHotelsCollection db = new HotelsCollection();
        private readonly ILogger<HotelsController> _logger;

        public HotelsController(ILogger<HotelsController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetList(string? startDate, string? endDate, int? numberPassagers, string? city )
        {
            if (DateTime.TryParseExact(startDate, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime startDatePayload) && DateTime.TryParseExact(endDate, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime endDatePayload))
            {
                List<Hotel> json = await db.List(startDatePayload, endDatePayload, numberPassagers, city);
            
                return Ok( json );
            }
            else
            {
                List<Hotel> json = await db.List(null, null, numberPassagers, city);

                return Ok(json);
            }

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetailsHotel(string id)
        {
            return Ok(await db.DetalisById(id));
        }

        [HttpPost]
        public async Task<IActionResult> CreateHotel([FromBody] Hotel hotel)
        {
            
            if (hotel == null)
            {
                return BadRequest();
            }

            Hotel createdHotel = await db.Insert(hotel);
            return Created("Creado con Éxito", createdHotel);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateHotel([FromBody] Hotel hotel, string id)
        {
            if (hotel == null)
            {
                return BadRequest();
            }

            hotel.Id = id;
            
            return Ok(await db.Update(hotel));
        }

        [HttpPatch("inactivate")]
        public async Task<IActionResult> InactivateHotel(string id)
        {
            
            return Ok(await db.Inactivate(id));
        }

        [HttpPatch("activate")]
        public async Task<IActionResult> ActivateHotel(string id)
        {

            return Ok(await db.Activate(id));
        }
    }
}
