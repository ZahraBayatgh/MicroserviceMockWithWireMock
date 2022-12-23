using AutoFixture;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Net;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace Microservice1.IntegrationTest
{
    public class WeatherForecastControllerTest : IClassFixture<TestAppFactory<Program>>
    {
        private readonly TestAppFactory<Program> _factory;
        private readonly WireMockServer _wireMockServer;

        public WeatherForecastControllerTest(TestAppFactory<Program> factory)
        {
            _factory = factory;
            _wireMockServer = _factory.Services.GetRequiredService<WireMockServer>();
        }
        [Fact]
        public async Task GetUser_ShouldReturnUser_WhenUserExists()
        {
            // Arrange
           var summaries = new[]
             {
              "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
             };
            var result = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = summaries[Random.Shared.Next(summaries.Length)]
            })
           .ToArray();

            _wireMockServer
                .Given(Request.Create().WithPath($"/WeatherForecast"))
                .RespondWith(
                    Response.Create().WithBodyAsJson(result)
                        .WithStatusCode(HttpStatusCode.OK)
                    );

            var httpClient = _factory.CreateClient();

            // Act
            var response = await httpClient.GetAsync($"/WeatherForecast");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

    }

}