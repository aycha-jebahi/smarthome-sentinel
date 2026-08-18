namespace DotnetWebProject1.Models
{
    public class Anomaly
    {
        public int Id { get; set; }
        public int ReadingId { get; set; }
        public Reading? Reading { get; set; }// Propriété de navigation EF Core vers la mesure associée

        public string Reason { get; set; } = string.Empty;   // ex: "Valeur trop éloignée de la moyenne (z-score = 3.4)"
        public DateTime DetectedAt { get; set; } = DateTime.UtcNow;
    }
}