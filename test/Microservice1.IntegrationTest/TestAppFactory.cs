using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WireMock.Server;

namespace Microservice1.IntegrationTest
{
   public class TestAppFactory<TStartup>
        : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            var wiremockServer = WireMockServer.Start();

            builder.ConfigureAppConfiguration(configurationBuilder =>
            {
                configurationBuilder.AddInMemoryCollection(new KeyValuePair<string, string>[]
                {
                    new("URL:BaseAddress", wiremockServer.Urls[0])
                });
            }).ConfigureServices(collection => collection.AddSingleton(wiremockServer));
        }
    }
}
