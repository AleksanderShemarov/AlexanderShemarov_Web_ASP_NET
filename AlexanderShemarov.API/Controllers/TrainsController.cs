using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AlexanderShemarov.API.Data;
using AlexanderShemarov.Domain.Entities;
using AlexanderShemarov.Domain.Models;


namespace AlexanderShemarov.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrainsController : ControllerBase
    {
        private readonly TrainsApiDbContext _context;
        private readonly IWebHostEnvironment _env;

        public TrainsController(TrainsApiDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
            Console.WriteLine($"WebRootPath: {_env.WebRootPath}");
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
        public async Task<IActionResult> PutTrains(int id, TrainUpdateDto train)
        {
            if (id != train.ID)
            {
                return BadRequest();
            }

            var existingTrain = await _context.TrainsAPI.FindAsync(id);
            if (existingTrain == null)
            {
                return NotFound();
            }

            var oldPath = existingTrain.Image;

            existingTrain.Name = train.Name;
            existingTrain.Description = train.Description;
            existingTrain.Speed = train.Speed;
            existingTrain.Cost = train.Cost;
            existingTrain.TrainTypesId = train.TrainTypesId;

            try
            {
                await _context.SaveChangesAsync();
                if (!string.IsNullOrEmpty(oldPath) && !string.Equals(oldPath, existingTrain.Image, StringComparison.OrdinalIgnoreCase))
                {
                    DeleteImageFile(oldPath);
                }
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
            var train = await _context.TrainsAPI.FindAsync(id);
            if (train == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(train.Image))
            {
                try
                {
                    var fileName = Path.GetFileName(train.Image);
                    if (!string.IsNullOrEmpty(fileName))
                    {
                        var imagePath = Path.Combine(_env.WebRootPath, "images", fileName);

                        if (System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                            Console.WriteLine($"An image has been removed: {imagePath}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error during an image removing: {ex.Message}");
                    Console.WriteLine($"Please, remove it manually: '{train.Image}'.");
                }
            }

            _context.TrainsAPI.Remove(train);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TrainsExists(int id)
        {
            return _context.TrainsAPI.Any(e => e.ID == id);
        }


        // Saving an Image for an data item by its id
        [HttpPost("{id}/image")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<string>> UploadImage(int id, IFormFile image)
        {
            var train = await _context.TrainsAPI.FindAsync(id);
            if (train == null) return NotFound();

            // The Old Image Removing
            if (!string.IsNullOrEmpty(train.Image))
            {
                DeleteImageFile(train.Image);
            }

            // New Image Saving
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";
            var filePath = Path.Combine(_env.WebRootPath, "images", fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            // The Only Image Property Updating
            train.Image = $"/images/{fileName}";
            await _context.SaveChangesAsync();

            return Ok(train.Image);
        }


        private void DeleteImageFile(string imagePath)
        {
            if (string.IsNullOrEmpty(imagePath))
            {
                Console.WriteLine("Image path is empty, nothing to delete");
                return;
            }

            try
            {
                string fileName;

                // For an absolute file's path
                if (Uri.TryCreate(imagePath, UriKind.Absolute, out var uri) &&
                    (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps))
                {
                    fileName = Path.GetFileName(uri.LocalPath);
                }
                // For a relative file's path
                else if (imagePath.StartsWith("/"))
                {
                    fileName = Path.GetFileName(imagePath);
                }
                // For a path as an file's name
                else
                {
                    fileName = imagePath;
                }

                if (!string.IsNullOrEmpty(fileName))
                {
                    var fullPath = Path.Combine(_env.WebRootPath, "images", fileName);
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                        Console.WriteLine($"Successfully deleted image: {fullPath}");
                    }
                    else
                    {
                        Console.WriteLine($"File not found: {fullPath}");
                    }
                }
                else
                {
                    Console.WriteLine($"Could not extract filename from path: {imagePath}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Image Removing Error: {ex.Message}");
            }
        }
    }


    public class TrainUpdateDto
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Speed { get; set; }
        public decimal Cost { get; set; }
        public int TrainTypesId { get; set; }
    }
}
