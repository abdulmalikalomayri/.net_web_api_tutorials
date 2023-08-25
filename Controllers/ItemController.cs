using Humanizer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using simpleapi.Models;
using System.Collections;

namespace simpleapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly DataContext _context;

        public ItemController(DataContext context)
        {
            _context = context;

            // auto seed database when Item is null
            if(_context.Items.Count() == 0)
            {
                _context.Items.Add(new Item
                {
                    Name = "Naruto",
                    Barcode = "0109293",
                    Qty = 900
                });
                
                _context.Items.Add(new Item
                {
                    Name = "Baki",
                    Barcode = "301193",
                    Qty = 520
                });

                _context.Items.Add(new Item
                {
                    Name = "Goku",
                    Barcode = "9090",
                    Qty = 99999
                });

                _context.Items.Add(new Item
                {
                    Name = "Light",
                    Barcode = "111111",
                    Qty = 10
                });

                _context.Items.Add(new Item
                {
                    Name = "TomoChan",
                    Barcode = "1010101",
                    Qty = 20
                });
                _context.SaveChanges();
            }
        }

        [HttpGet]
        public IEnumerable GetAll()
        {
            return _context.Items.ToList();
        }

        [HttpGet("id")]
        public IActionResult GetById(int id)
        {
            var item = _context.Items.Where(item => item.Id == id);

            if(item == null)
            {
                return NotFound();
            }

            return new ObjectResult(item);
        }

        [HttpGet("name")]
        public IEnumerable GetByName(string name)
        {
            var result = _context.Items.Where(item => item.Name == name);

            return result;
        }

        [HttpGet]
        [Route("GetMaxQty")]
        public IEnumerable GetMax()
        {
            /*
            var res = (from item in _context.Items
                       group item by item.Qty into q
                       
                       select new
                       {
                           itemQty = q.ToList()
                       }
                       ).ToList();
            */

            var maxQty = _context.Items.Max(item => item.Qty);

            var res = _context.Items.Where(item => item.Qty >= maxQty );

            return res;
        }

        [HttpPost]
        public IActionResult Create([FromBody] Item item)
        {
            if(item == null)
            {
                return BadRequest();
            }

            _context.Items.Add(item);
            _context.SaveChanges();

            return Ok(item);
            // return CreatedAtRoute("GetById", new { id = item.Id }, item);

        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Item item)
        {
            if(item == null || item.Id != id)
            {
                return BadRequest();
            }

            var result = _context.Items.FirstOrDefault(item => item.Id == id);
            if(result == null)
            {
                return BadRequest();
            }

            result.Name = item.Name;
            result.Barcode = item.Barcode;
            result.Qty = item.Qty;

            _context.Items.Update(result);
            _context.SaveChanges();

            return Ok("Item Updated");

        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var item = _context.Items.FirstOrDefault(item => item.Id == id);

            if(item == null)
            {
                return NotFound();
            }

            _context.Items.Remove(item);
            _context.SaveChanges();

            return Ok("Item has been deleted!");
        }


    }
}
