using DotnetWebProject1.Models;

namespace DotnetWebProject1.Data
{
    public static class DbInitializer
    {
        public static void Seed(AppDbContext context)
        {

            if (context.Sensors.Any())
            {
                return; // La base contient déjà des capteurs
            }

            var sensors = new Sensor[]
            {
                new Sensor { Name = "Salon - Température", Type = "Temperature", Unit = "°C" },
                new Sensor { Name = "Cuisine - Consommation", Type = "Electricity", Unit = "W" },
                new Sensor { Name = "Chambre - Humidité", Type = "Humidity", Unit = "%" },
                new Sensor { Name = "Routeur - Latence Réseau", Type = "NetworkLatency", Unit = "ms" }
            };

            context.Sensors.AddRange(sensors);
            context.SaveChanges();
        }
    }
}