using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using TravelAgencyApp.Models;
using TravelAgencyApp.Repositories;
using TravelAgencyApp.Repositories.Bedrooms;
using TravelAgencyApp.Repositories.Collections;

namespace TravelAgencyApp.Controllers
{
    [Route("api/bedrooms")]
    [ApiController]
    public class BedroomsController : ControllerBase
    {
        private IBedroomsCollection db = new BedroomsCollection();
        private readonly ILogger<BedroomsController> _logger;

        public BedroomsController(ILogger<BedroomsController> logger)
        {
            _logger = logger;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody] Bedroom bedroom, string id)
        {
            if (bedroom == null)
            {
                return BadRequest();
            }

            bedroom.Id = id;

            return Ok(await db.Update(bedroom));
        }

        [HttpPatch("inactivate")]
        public async Task<IActionResult> Inactivate(string id)
        {

            return Ok(await db.Inactivate(id));
        }

        [HttpPatch("activate")]
        public async Task<IActionResult> Activate(string id)
        {

            return Ok(await db.Activate(id));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> DetailsById(string id)
        {
            return Ok(await db.DetalisById(id));
        }
    }
}
