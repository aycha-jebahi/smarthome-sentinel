using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DotnetWebProject1.Data;

namespace DotnetWebProject1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReadingsController : ControllerBase
    {
        private readonly AppDbContext _context;
        public ReadingsController(AppDbContext context) => _context = context;

        // Historique d'un capteur précis
        [HttpGet("{sensorId}")]
        public async Task<IActionResult> GetBySensor(int sensorId, [FromQuery] int take = 50)
        {
            var readings = await _context.Readings
                .Where(r => r.SensorId == sensorId)
                .OrderByDescending(r => r.Timestamp)
                .Take(take)
                .ToListAsync();

            return Ok(readings);
        }

        // Dernières lectures tous capteurs confondus (pour le dashboard)
        [HttpGet("latest")]
        public async Task<IActionResult> GetLatest([FromQuery] int take = 20)
        {
            var readings = await _context.Readings
                .Include(r => r.Sensor)
                .OrderByDescending(r => r.Timestamp)
                .Take(take)
                .Select(r => new
                {
                    r.Id,
                    r.Value,
                    r.Timestamp,
                    r.IsAnomaly,
                    SensorName = r.Sensor!.Name,
                    SensorType = r.Sensor.Type,
                    Unit = r.Sensor.Unit
                })
                .ToListAsync();

            return Ok(readings);
        }
    }
}