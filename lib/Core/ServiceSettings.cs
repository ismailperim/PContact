using Core.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Core
{
    public class ServiceSettings : IOptions<ServiceOptions>
    {
        public ServiceSettings()
        {
            var configuration = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile("appsettings.json",
                              optional: false,
                              reloadOnChange: true)
              .AddEnvironmentVariables()
              .Build();

            Value = configuration.GetSection("ServiceOptions").Get<ServiceOptions>();

        }
        public ServiceOptions Value { get; set; }
    }
}