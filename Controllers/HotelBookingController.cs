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
        // these two func are the default for DbContext 
        // Depency Injection in the constructor 
        private readonly ApiContext _context;

        public HotelBookingController(ApiContext context)
        {
            _context = context;
        }

        /**
         * @method POST
         * Store and Update!
         **/
        [HttpPost]
        public JsonResult CreateEdit(HotelBooking booking)
        {
            // if Booking are id Not exsist in DB Add booking
            if (booking.Id == 0)
            {
                _context.Bookings.Add(booking);

            }
            // if Booking ID Exsist 
            else
            {
                var bookingInDb = _context.Bookings.Find(booking.Id);

                if (bookingInDb == null)
                {
                    return new JsonResult(NotFound());
                }

                // update booking
                bookingInDb = booking;
            }

            // always save changes of the Database Mulipating 
            // it's like Model.save(); in Laravel 
            _context.SaveChanges();

            return new JsonResult(Ok(booking));
        }

        /***
         * @method GET
         * @Description get user by ID from Database 
         **/
        [HttpGet]
        public JsonResult GetById(int id)
        {

            var booking = _context.Bookings.Find(id);

            if (booking == null)
            {
                return new JsonResult(NotFound(booking));
            }

            return new JsonResult(Ok(booking));
        }

        [HttpGet]
        public JsonResult Get(int id)
        {
          

            return new JsonResult(Ok());
        }

        /*
        [HttpPut]
        public JsonResult Put(int id, HotelBooking booking)
        {
            var book = _context.Bookings.Find(id);


            return Ok(Put(id));
        }
        */
        
        /*
        [HttpGet]
        public async Task<ActionResult<List<HotelBooking>>> Get()
        {
            return Ok();
        }
        */

        /***
         * @method Delete
         * @Description Delete a booking from Database 
         **/
        [HttpDelete]
        public JsonResult Delete(int id)
        {
            var booking = _context.Bookings.Find(id);

            if(booking == null)
            {
                return new JsonResult(NotFound(booking));
            }

            _context.Bookings.Remove(booking);

            // for every DML query I must write this code to Save Changes!
            _context.SaveChanges();

            // return new JsonResult(NoContent());
            return new JsonResult(Ok("Deleted!"));
        }
    }
}
