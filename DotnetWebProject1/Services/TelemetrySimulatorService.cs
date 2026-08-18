using DotnetWebProject1.Data;
using DotnetWebProject1.Hubs;
using DotnetWebProject1.Models;
using Microsoft.AspNetCore.SignalR;

namespace DotnetWebProject1.Services
{
    public class TelemetrySimulatorService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IHubContext<AnomalyHub> _hubContext;
        private readonly Random _random = new();

        public TelemetrySimulatorService(IServiceProvider serviceProvider, IHubContext<AnomalyHub> hubContext)
        {
            _serviceProvider = serviceProvider;
            _hubContext = hubContext;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    var detectionService = scope.ServiceProvider.GetRequiredService<AnomalyDetectionService>();
                    var sensors = context.Sensors.ToList();

                    if (sensors.Any())
                    {
                        var sensor = sensors[_random.Next(sensors.Count)];
                        double value = Math.Round(GenerateValue(sensor.Type), 2);
                        var (isAnomaly, reason) = detectionService.Evaluate(sensor.Type, value);

                        var reading = new Reading
                        {
                            SensorId = sensor.Id,
                            Value = value,
                            Timestamp = DateTime.UtcNow,
                            IsAnomaly = isAnomaly
                        };

                        context.Readings.Add(reading);
                        await context.SaveChangesAsync(stoppingToken);

                        // Diffusion en direct de chaque lecture (pour le graphique/tableau live)
                        await _hubContext.Clients.All.SendAsync("NewReading", new
                        {
                            sensorName = sensor.Name,
                            sensorType = sensor.Type,
                            value,
                            isAnomaly,
                            timestamp = reading.Timestamp
                        }, stoppingToken);

                        if (isAnomaly)
                        {
                            var anomaly = new Anomaly
                            {
                                ReadingId = reading.Id,
                                Reason = reason,
                                DetectedAt = DateTime.UtcNow
                            };
                            context.Anomalies.Add(anomaly);
                            await context.SaveChangesAsync(stoppingToken);

                            // Diffusion spécifique pour les alertes
                            await _hubContext.Clients.All.SendAsync("NewAnomaly", new
                            {
                                sensorName = sensor.Name,
                                reason,
                                value,
                                detectedAt = anomaly.DetectedAt
                            }, stoppingToken);
                        }

                        Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] {sensor.Name} = {value} {(isAnomaly ? "⚠️ " + reason : "OK")}");
                    }
                }

                await Task.Delay(3000, stoppingToken);
            }
        }

        private double GenerateValue(string sensorType)
        {
            bool extremeCase = _random.Next(1, 9) == 1;

            return sensorType switch
            {
                "Temperature" => extremeCase
                    ? _random.NextDouble() * (60.0 - 41.0) + 41.0
                    : _random.NextDouble() * (26.0 - 18.0) + 18.0,

                "Electricity" => extremeCase
                    ? _random.NextDouble() * (5000.0 - 3600.0) + 3600.0
                    : _random.NextDouble() * (1500.0 - 200.0) + 200.0,

                "NetworkLatency" => extremeCase
                    ? _random.NextDouble() * (800.0 - 250.0) + 250.0
                    : _random.NextDouble() * (45.0 - 10.0) + 10.0,

                "Humidity" => extremeCase
                    ? (_random.Next(2) == 0 ? _random.NextDouble() * 15.0 : _random.NextDouble() * (95.0 - 81.0) + 81.0)
                    : _random.NextDouble() * (60.0 - 40.0) + 40.0,

                _ => _random.NextDouble() * 100.0
            };
        }
    }
}