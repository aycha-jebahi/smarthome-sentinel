namespace DotnetWebProject1.Services
{
    public class AnomalyDetectionService
    {
        public (bool isAnomaly, string reason) Evaluate(string sensorType, double value)
        {
            return sensorType switch
            {
                "Temperature" when value > 40.0 =>
                    (true, $"Surchauffe détectée ({value}°C > 40°C)"),

                "Electricity" when value > 3500.0 =>
                    (true, $"Pic de consommation anormal ({value}W > 3500W)"),

                "NetworkLatency" when value > 200.0 =>
                    (true, $"Latence réseau critique ({value}ms > 200ms)"),

                "Humidity" when value < 20.0 || value > 80.0 =>
                    (true, $"Taux d'humidité anormal ({value}%)"),

                _ => (false, string.Empty)
            };
        }
    }
}