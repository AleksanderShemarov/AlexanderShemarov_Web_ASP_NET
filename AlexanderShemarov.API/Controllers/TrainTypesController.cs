using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AlexanderShemarov.API.Data;
using AlexanderShemarov.Domain.Entities;

namespace AlexanderShemarov.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrainTypesController : ControllerBase
    {
        private readonly TrainsApiDbContext _context;

        public TrainTypesController(TrainsApiDbContext context)
        {
            _context = context;
        }

        // GET: api/TrainTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TrainTypes>>> GetTrainTypesAPI()
        {
            return await _context.TrainTypesAPI.ToListAsync();
        }

        // GET: api/TrainTypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TrainTypes>> GetTrainTypes(int id)
        {
            var trainTypes = await _context.TrainTypesAPI.FindAsync(id);

            if (trainTypes == null)
            {
                return NotFound();
            }

            return trainTypes;
        }

        // PUT: api/TrainTypes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTrainTypes(int id, TrainTypes trainTypes)
        {
            if (id != trainTypes.ID)
            {
                return BadRequest();
            }

            _context.Entry(trainTypes).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TrainTypesExists(id))
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

        // POST: api/TrainTypes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TrainTypes>> PostTrainTypes(TrainTypes trainTypes)
        {
            _context.TrainTypesAPI.Add(trainTypes);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTrainTypes", new { id = trainTypes.ID }, trainTypes);
        }

        // DELETE: api/TrainTypes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrainTypes(int id)
        {
            var trainTypes = await _context.TrainTypesAPI.FindAsync(id);
            if (trainTypes == null)
            {
                return NotFound();
            }

            _context.TrainTypesAPI.Remove(trainTypes);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TrainTypesExists(int id)
        {
            return _context.TrainTypesAPI.Any(e => e.ID == id);
        }
    }
}
