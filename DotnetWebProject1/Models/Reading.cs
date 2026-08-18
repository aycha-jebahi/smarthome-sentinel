namespace DotnetWebProject1.Models
{
    public class Reading  //(une mesure brute)
    {
        public int Id { get; set; }
        public int SensorId { get; set; }
        public Sensor? Sensor { get; set; }

        public double Value { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public bool IsAnomaly { get; set; } = false;
    }
}