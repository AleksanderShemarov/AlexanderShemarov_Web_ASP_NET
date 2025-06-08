using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AlexanderShemarov.API.Data;
using AlexanderShemarov.Domain.Entities;
using AlexanderShemarov.Domain.Models;
using System.Diagnostics;

namespace AlexanderShemarov.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrainsController : ControllerBase
    {
        private readonly TrainsApiDbContext _context;

        public TrainsController(TrainsApiDbContext context)
        {
            _context = context;
        }

        // GET: api/Trains
        [HttpGet]
        public async Task<ActionResult<ResponseData<ListModel<Trains>>>> GetTrainsAPI(
            string? trainType, int pageNo = 1, int pageSize = 3
        )
        {
            var result = new ResponseData<ListModel<Trains>>();

            int? trainTypesID = null;
            if (trainType != null)
            {
                trainTypesID = _context.TrainTypesAPI.FirstOrDefault(tt => tt.NormalizedName.Equals(trainType))?.ID;
            }

            var data = _context.TrainsAPI.Where(trainAPI => trainTypesID == null || trainAPI.TrainTypesId == trainTypesID)?.ToList();
            
            int totalPages = (int)Math.Ceiling(data.Count / (double)pageSize);
            if (pageNo > totalPages)
            {
                pageNo = totalPages;
            }

            result.Data = new ListModel<Trains>()
            {
                Items = data.Skip((pageNo - 1) * pageSize).Take(pageSize).ToList(),
                CurrentPage = pageNo,
                TotalPages = totalPages
            };

            if (data.Count == 0)
            {
                result.Success = false;
                result.ErrorMessage = "Няма аб'ектаў адабранай катэгорыі!";
            }

            return result;
        }

        // GET: api/Trains/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Trains>> GetTrains(int id)
        {
            var trains = await _context.TrainsAPI.FindAsync(id);

            if (trains == null)
            {
                return NotFound();
            }

            return trains;
        }

        // PUT: api/Trains/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTrains(int id, Trains trains)
        {
            if (id != trains.ID)
            {
                return BadRequest();
            }

            _context.Entry(trains).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TrainsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Trains
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Trains>> PostTrains(Trains trains)
        {
            _context.TrainsAPI.Add(trains);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTrains", new { id = trains.ID }, trains);
        }

        // DELETE: api/Trains/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrains(int id)
        {
            var trains = await _context.TrainsAPI.FindAsync(id);
            if (trains == null)
            {
                return NotFound();
            }

            _context.TrainsAPI.Remove(trains);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TrainsExists(int id)
        {
            return _context.TrainsAPI.Any(e => e.ID == id);
        }
    }
}
