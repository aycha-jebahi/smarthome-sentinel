using DotnetWebProject1.Services;
using Xunit;

namespace SmartHomeSentinel.Tests
{
    public class AnomalyDetectionServiceTests
    {
        private readonly AnomalyDetectionService _service = new();

        [Theory]
        [InlineData(45.0, true)]   // au-dessus du seuil
        [InlineData(22.0, false)]  // normal
        [InlineData(40.0, false)]  // limite exacte (pas strictement supérieur)
        public void Evaluate_Temperature(double value, bool expectedAnomaly)
        {
            var (isAnomaly, _) = _service.Evaluate("Temperature", value);
            Assert.Equal(expectedAnomaly, isAnomaly);
        }

        [Theory]
        [InlineData(4000.0, true)]
        [InlineData(800.0, false)]
        public void Evaluate_Electricity(double value, bool expectedAnomaly)
        {
            var (isAnomaly, _) = _service.Evaluate("Electricity", value);
            Assert.Equal(expectedAnomaly, isAnomaly);
        }

        [Theory]
        [InlineData(300.0, true)]
        [InlineData(25.0, false)]
        public void Evaluate_NetworkLatency(double value, bool expectedAnomaly)
        {
            var (isAnomaly, _) = _service.Evaluate("NetworkLatency", value);
            Assert.Equal(expectedAnomaly, isAnomaly);
        }

        [Theory]
        [InlineData(5.0, true)]    // trop sec
        [InlineData(90.0, true)]   // trop humide
        [InlineData(50.0, false)]  // normal
        public void Evaluate_Humidity(double value, bool expectedAnomaly)
        {
            var (isAnomaly, _) = _service.Evaluate("Humidity", value);
            Assert.Equal(expectedAnomaly, isAnomaly);
        }

        [Fact]
        public void Evaluate_ReasonMessage_ContainsValue()
        {
            var (isAnomaly, reason) = _service.Evaluate("Temperature", 45.0);
            Assert.True(isAnomaly);
            Assert.Contains("Surchauffe", reason);
        }
    }
}
