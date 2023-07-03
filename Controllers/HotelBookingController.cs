using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using simpleapi.Models;
using simpleapi.Data;

namespace simpleapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelBookingController : ControllerBase
    {
        /** 
         *  Once you use a async function then every thing should be async  
         **/

        // these two func are the default for DbContext 
        // Depency Injection in the constructor 
        private readonly ApiContext _context;

        // using constructor we inject the DataContext
        public HotelBookingController(ApiContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<HotelBooking>>> Get()
        {
            return Ok(await _context.Bookings.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<List<HotelBooking>>> Get(int id)
        {
            var book = await _context.Bookings.FindAsync(id);
            if(book == null)
            {
                return BadRequest("Booking not foound!");
            }

            return Ok(book);
        }

        [HttpPost]
        public async Task<ActionResult<List<HotelBooking>>> Add(HotelBooking booking)
        {
            _context.Bookings.Add(booking);

            await _context.SaveChangesAsync();

            return Ok(await _context.Bookings.ToListAsync());
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<List<HotelBooking>>> Update(HotelBooking request)
        {
            var dbBooking = await _context.Bookings.FindAsync(request.Id);
            if(dbBooking == null)
            {
                return BadRequest("Booking not found!");
            }

            dbBooking.RoomNumber = request.RoomNumber;
            dbBooking.Client = request.Client;

            await _context.SaveChangesAsync();

            return Ok(await _context.Bookings.ToListAsync());
        }

        [HttpDelete]
        public async Task<ActionResult<List<HotelBooking>>> Delete(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if(booking == null )
            {
                return BadRequest("Booking not found!");
            }

            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();

            return Ok(await _context.Bookings.ToListAsync());

        }
        
    }
}
