using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DotnetWebProject1.Data;

namespace DotnetWebProject1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SensorsController : ControllerBase
    {
        private readonly AppDbContext _context;
        public SensorsController(AppDbContext context) => _context = context;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var sensors = await _context.Sensors.ToListAsync();
            return Ok(sensors);
        }
    }
}