using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DotnetWebProject1.Data;

namespace DotnetWebProject1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnomaliesController : ControllerBase
    {
        private readonly AppDbContext _context;
        public AnomaliesController(AppDbContext context) => _context = context;

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int take = 50)
        {
            var anomalies = await _context.Anomalies
                .Include(a => a.Reading!)
                    .ThenInclude(r => r.Sensor)
                .OrderByDescending(a => a.DetectedAt)
                .Take(take)
                .Select(a => new
                {
                    a.Id,
                    a.Reason,
                    a.DetectedAt,
                    Value = a.Reading!.Value,
                    SensorName = a.Reading.Sensor!.Name
                })
                .ToListAsync();

            return Ok(anomalies);
        }
    }
}