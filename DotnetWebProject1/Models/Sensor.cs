namespace DotnetWebProject1.Models
{
    public class Sensor
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;      // ex: "Salon - Température"
        public string Type { get; set; } = string.Empty;      // ex: "Temperature", "Electricity", "Humidity", "NetworkLatency"
        public string Unit { get; set; } = string.Empty;      // ex: "°C", "W", "%", "ms"

        public ICollection<Reading> Readings { get; set; } = new List<Reading>();
    }
}